using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IPV4Header
    {
        public byte Version;
        public byte ServicesField;
        public UInt16 totalLength;
        public UInt16 ID;
        public UInt16 FragmentOffset;
        public byte TTL;
        public byte Protocol;
        public UInt16 HeaderChecksum;
        public fixed byte Source[4];
        public fixed byte Destination[4];
    }

    /// <summary>
    /// LAYER 3 - IPV4
    /// </summary>
    class IPV4
    {
        // Network packet type handler
        public unsafe delegate void PackerHandler(uint xid, byte* buffer, uint size);

        private static PackerHandler[] m_handlers;

        /// <summary>
        /// Initializes IPV4
        /// </summary>
        public static unsafe void Init()
        {
            m_handlers = new PackerHandler[255];
            Network.RegisterHandler(0x0800, Handle);
        }

        /// <summary>
        /// Handle packet
        /// </summary>
        /// <param name="buffer">Buffer pointer</param>
        /// <param name="size">Packet size</param>
        private static unsafe void Handle(byte* buffer, uint size)
        {
            IPV4Header* header = (IPV4Header*)buffer;
            
            byte proto = header->Protocol;
            
            m_handlers[proto]?.Invoke(ByteUtil.ReverseBytes(header->ID), buffer + sizeof(IPV4Header), size);
        }

        /// <summary>
        /// Register packet handler
        /// </summary>
        /// <param name="proto">Protocol</param>
        /// <param name="handler">Handler</param>
        public static void RegisterHandler(byte proto, PackerHandler handler)
        {
            m_handlers[proto] = handler;
        }
        
        /// <summary>
        /// Add IPV4 header to packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="destMac">Destination MAC address</param>
        /// <param name="sourceIP">Source IP</param>
        /// <param name="destIP">Destination IP</param>
        /// <param name="protocol">Protocol</param>
        /// <returns></returns>
        private static unsafe IPV4Header *addHeader(NetPacketDesc *packet, byte[] destMac, byte[] sourceIP, byte[] destIP, byte protocol)
        {
            byte[] mymac = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(mymac));

            packet->start -= (short)sizeof(IPV4Header);


            IPV4Header* header = (IPV4Header*)(packet->buffer + packet->start);

            header->Version = (4 << 4) | 5;
            header->ServicesField = 0;
            header->totalLength = ByteUtil.ReverseBytes((ushort)(packet->end - packet->start));
            header->ID = ByteUtil.ReverseBytes(0xa836); // TODO: FIX THIS!
            header->FragmentOffset = 0;
            header->TTL = 250;
            header->Protocol = protocol;
            header->HeaderChecksum = 0; // CHECKSUM?
            
            for (int i = 0; i < 4; i++)
                header->Source[i] = sourceIP[i];
            for (int i = 0; i < 4; i++)
                header->Destination[i] = destIP[i];


            header->HeaderChecksum = (NetworkTools.Checksum(packet->buffer + packet->start, sizeof(IPV4Header)));

            return header;
        }
        
        /// <summary>
        /// Send IPV4 packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="destMac">Destination mac</param>
        /// <param name="destIP">Destination IP</param>
        /// <param name="protocol">Protocol</param>
        public static unsafe void Send(NetPacketDesc* packet, byte[] destMac, byte[] destIP, byte protocol)
        {
            byte[] sourceIP = new byte[4];
            for (int i = 0; i < 4; i++) sourceIP[i] = Network.Settings->IP[i];

            addHeader(packet, destMac, sourceIP, destIP, protocol);

            Ethernet.Send(packet, destMac, EthernetTypes.IPV4);
        }

    }
}
