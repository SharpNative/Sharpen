using Sharpen.Mem;
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

        private unsafe struct UDPPacket
        {
            public fixed byte Buffer[2048];
            public uint Size;
            public bool InUse;
        }

        private UDPPacket[] m_packets;

        private ushort m_sourcePort;
        private ushort m_targetPort;
        private bool m_connected = false;
        private byte[] m_ip;

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
        /// Connect to port
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns></returns>
        public bool Connect(string ip, ushort port)
        {
            m_ip = NetworkTools.StringToIp(ip);
            if (m_ip == null)
                return false;
            
            bool found = ARP.IpExists(m_ip);
            if (!found)
                return false;
            
            m_connected = true;

            m_packets = new UDPPacket[BACKLOG];
            for (int i = 0; i < BACKLOG; i++)
                m_packets[i].InUse = false;

            m_targetPort = port;
             
            // Register a sourcePort
            UDP.BindSocket(this);

            return true;
        }

        /// <summary>
        /// Receive on socket
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe void Receive(byte* buffer, uint size)
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
        /// Send to UDP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe void Send(byte* buffer, uint size)
        {
            NetPacketDesc* packet = NetPacket.Alloc();

            Memory.Memcpy(packet->buffer + packet->start, buffer, (int)size);

            packet->end += (short)size;
            
            UDP.Send(packet, m_ip, m_sourcePort, m_targetPort);

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

            //UDP.UnBindSocket(this);
            //m_connected = false;
        }
    }
}
