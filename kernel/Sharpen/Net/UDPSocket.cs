using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPSocket
    {
        private unsafe struct UDPBacklogEntry
        {
            public fixed byte Buffer[2048];
            public uint Size;
            public fixed byte IP[4];
        }

        private unsafe struct UDPPacketHeader
        {
            public fixed byte IP[4];
            public uint Size;
        }

        private Queue m_queue;

        private ushort m_sourcePort;
        private ushort m_targetPort;
        private bool m_connected = false;
        private byte[] m_ip;

        private bool m_ipSpecified = false;

        public ushort SourcePort
        {
            get { return m_sourcePort; }
            set { m_sourcePort = value; }
        }

        public ushort TargetPort
        {
            get { return m_targetPort; }
        }

        /// <summary>
        /// Connect to UDP client
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns>If the connection was successful</returns>
        public bool Connect(string ip, ushort port)
        {
            m_ip = NetworkTools.StringToIp(ip);
            if (m_ip == null)
                return false;

            bool found = Route.FindRoute(m_ip);
            if (!found)
                return false;

            m_connected = true;

            m_queue = new Queue();

            m_targetPort = port;

            m_ipSpecified = true;

            // Register a sourcePort
            UDP.BindSocketRequest(this);

            return true;
        }
        
        /// <summary>
        /// Bind to port
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns>If the bind was successful</returns>
        public bool Bind(ushort port)
        {
            m_sourcePort = port;

            // Register a sourcePort
            UDP.BindSocket(this);

            m_ipSpecified = false;
            m_connected = true;

            m_queue = new Queue();

            return true;
        }

        /// <summary>
        /// Receive on socket
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe void Receive(byte[] ip, byte* buffer, uint size)
        {
            // Do not accept empty packets
            if (size == 0)
                return;

            UDPBacklogEntry* entry = (UDPBacklogEntry*)Heap.Alloc(sizeof(UDPBacklogEntry));

            if (size >= 2048)
                size = 2048;

            Memory.Memcpy(entry->Buffer, buffer, (int)size);

            entry->Size = size;

            Memory.Memcpy(entry->IP, Util.ObjectToVoidPtr(ip), 4);

            m_queue.Push(entry);
        }

        /// <summary>
        /// Read  from UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public unsafe uint Read(byte* buffer, uint size)
        {
            // We can't do this if we just connect
            if (!m_ipSpecified)
                return 0;

            UDPBacklogEntry* entry = (UDPBacklogEntry*)m_queue.Pop();
            if (entry == null)
                return 0;

            if (size > entry->Size)
                size = entry->Size;

            Memory.Memcpy(buffer, entry->Buffer, (int)size);

            Heap.Free(entry);

            return size;
        }
        
        /// <summary>
        /// Read packet from UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public unsafe uint ReadPacket(byte* buffer, uint size)
        {
            // Minimum size of 6
            if (size < 6)
                return 0;

            UDPBacklogEntry* entry = (UDPBacklogEntry*)m_queue.Pop();

            if (entry == null)
                return 0;

            if (size > entry->Size + sizeof(UDPPacketHeader))
                size = entry->Size + (uint)sizeof(UDPPacketHeader);

            uint offset = (uint)sizeof(UDPPacketHeader);
            uint sizeData = size - offset;

            UDPPacketHeader* packet = (UDPPacketHeader*)buffer;
            packet->Size = sizeData;
            
            Memory.Memcpy(packet->IP, entry->IP, 4);

            if (sizeData > 0)
                Memory.Memcpy(buffer + offset, entry->Buffer, (int)sizeData);

            Heap.Free(entry);

            return size;
        }

        /// <summary>
        /// Send to UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe void Send(byte* buffer, uint size)
        {
            // We can't do this if we just connect
            if (!m_ipSpecified)
                return;

            NetPacketDesc* packet = NetPacket.Alloc();

            Memory.Memcpy(packet->buffer + packet->start, buffer, (int)size);

            packet->end += (short)size;

            UDP.Send(packet, m_ip, m_sourcePort, m_targetPort);

            NetPacket.Free(packet);
        }

        /// <summary>
        /// Send to UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe void SendByPacket(byte* buffer, uint size)
        {
            // Minimal size :O
            if (size < 6)
                return;

            UDPPacketHeader* header = (UDPPacketHeader*)buffer;

            // Uhhh no
            if (size < header->Size + sizeof(UDPPacketHeader))
                return;

            NetPacketDesc* packet = NetPacket.Alloc();

            Memory.Memcpy(packet->buffer + packet->start + sizeof(UDPPacketHeader), buffer, (int)header->Size);

            packet->end += (short)header->Size;

            UDP.Send(packet, Util.PtrToArray(header->IP), m_sourcePort, m_targetPort);

            NetPacket.Free(packet);
        }

        /// <summary>
        /// Get size
        /// </summary>
        /// <returns></returns>
        public unsafe uint GetSize()
        {
            if (!m_connected)
                return 0;

            UDPBacklogEntry* entry = (UDPBacklogEntry*)m_queue.Peek();

            return entry->Size;
        }

        /// <summary>
        /// Close socket
        /// </summary>
        public void Close()
        {
            UDP.UnBindSocket(this);
            m_connected = false;
            Heap.Free(m_queue);
            m_queue = null;
        }
    }
}
