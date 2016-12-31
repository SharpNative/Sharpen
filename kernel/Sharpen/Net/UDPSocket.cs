using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class UDPSocket
    {
        private const ushort BACKLOG = 20;

        private unsafe struct UDPBacklogEntry
        {
            public fixed byte Buffer[2048];
            public uint Size;
            public bool InUse;
            public fixed byte IP[4];
        }

        private unsafe struct UDPPacketHeader
        {
            public fixed byte IP[4];
            public uint Size;
        }

        private UDPBacklogEntry[] m_packets;

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
        /// <returns></returns>
        public bool Connect(string ip, ushort port)
        {
            m_ip = NetworkTools.StringToIp(ip);
            if (m_ip == null)
                return false;

            bool found = Route.FindRoute(m_ip);
            if (!found)
                return false;
            
            m_connected = true;

            m_packets = new UDPBacklogEntry[BACKLOG];
            for (int i = 0; i < BACKLOG; i++)
                m_packets[i].InUse = false;

            m_targetPort = port;

            m_ipSpecified = true;

            // Register a sourcePort
            UDP.BindSocketRequest(this);

            return true;
        }


        /// <summary>
        ///  Bind to port
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns></returns>
        public bool Bind(ushort port)
        {
            m_sourcePort = port;

            // Register a sourcePort
            UDP.BindSocket(this);

            m_ipSpecified = false;
            m_connected = true;

            m_packets = new UDPBacklogEntry[BACKLOG];
            for (int i = 0; i < BACKLOG; i++)
                m_packets[i].InUse = false;
            
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
            
            int i = 0;

            while (i < BACKLOG)
            {

                if( ! m_packets[i].InUse)
                {
                    if (size >= 2048)
                        size = 2048;

                    m_packets[i].Size = size;
                    fixed(byte *ptr = m_packets[i].Buffer)
                    {
                        Memory.Memset(ptr, 0, 2048);
                        Memory.Memcpy(ptr, buffer, (int)size);
                    }

                    m_packets[i].InUse = true;
                    
                    fixed (byte* ptr = m_packets[i].IP)
                    {
                        Memory.Memcpy(ptr, Util.ObjectToVoidPtr(ip), 4);
                    }


                    break;
                }

                i++;
            }
        }

        /// <summary>
        /// Read  from UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public unsafe uint Read(byte *buffer, uint size)
        {
            // We can't do this if we just connect
            if (!m_ipSpecified)
                return 0;

            if (GetSize() == 0)
                return 0;
            
            bool found = false;

            int i = 0;
            while (i < BACKLOG)
            {
                if (m_packets[i].InUse)
                {

                    if (size > m_packets[i].Size)
                        size = m_packets[i].Size;

                    fixed(byte *ptr = m_packets[i].Buffer)
                    {
                        for (int j = 0; j < size; j++)
                            buffer[j] = ptr[j];
                    }


                    m_packets[i].InUse = false;
                    found = true;
                    break;
                }
            }

            if (!found)
                return 0;

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

            if (GetSize() == 0)
                return 0;

            bool found = false;

            int i = 0;
            while (i < BACKLOG)
            {
                if (m_packets[i].InUse)
                {

                    if (size > m_packets[i].Size + sizeof(UDPPacketHeader))
                        size = m_packets[i].Size + (uint)sizeof(UDPPacketHeader);

                    uint offset = (uint)sizeof(UDPPacketHeader);
                    uint sizeData = size - offset;
                    
                    UDPPacketHeader* packet = (UDPPacketHeader*)buffer;
                    packet->Size = sizeData;


                    fixed(byte *ptr = m_packets[i].IP)
                        Memory.Memcpy(packet->IP, ptr, 4);

                    if(sizeData > 0)
                        fixed (byte* ptr = m_packets[i].Buffer)
                            Memory.Memcpy(buffer + offset, ptr, (int)sizeData);
                    
                    m_packets[i].InUse = false;

                    found = true;
                    break;
                }
            }

            if (!found)
                return 0;

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
        public uint GetSize()
        {
            if (!m_connected)
                return 0;

            int i = 0;
            while (i < BACKLOG)
            {

                if (m_packets[i].InUse)
                    return m_packets[i].Size;

                i++;
            }

            return 0;
        }

        /// <summary>
        /// Close socket
        /// </summary>
        public void Close()
        {
            // Free buffer here :)

            UDP.UnBindSocket(this);
            m_connected = false;
        }
    }
}
