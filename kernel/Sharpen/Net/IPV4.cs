using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Runtime.InteropServices;

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
        public unsafe delegate void PackerHandler(byte[] ip, byte* buffer, uint size);

        private static PackerHandler[] m_handlers;

        /// <summary>
        /// Initializes IPV4
        /// </summary>
        public static unsafe void Init()
        {
            m_handlers = new PackerHandler[256];
            Network.RegisterHandler(0x0800, Handle);
        }

        /// <summary>
        /// Handle packet
        /// </summary>
        /// <param name="buffer">Buffer pointer</param>
        /// <param name="size">Packet size</param>
        private static unsafe void Handle(byte[] mac, byte* buffer, uint size)
        {
            IPV4Header* header = (IPV4Header*)buffer;
            
            byte proto = header->Protocol;

            byte[] ip = Util.PtrToArray(header->Source);
            
            // Fake ARP
            if(!(ip[0] == 255 && ip[1] == 255 && ip[2] == 255 && ip[3] == 255))
            {
                ARP.FindOrAdd(ip, mac);
            }
            
            ushort sz = (ushort)(Utilities.Byte.ReverseBytes(header->totalLength) - sizeof(IPV4Header));
            
            m_handlers[proto]?.Invoke(ip, buffer + sizeof(IPV4Header), sz);
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
        /// <param name="sourceIP">Source IP</param>
        /// <param name="destIP">Destination IP</param>
        /// <param name="protocol">Protocol</param>
        /// <returns></returns>
        private static unsafe IPV4Header *addHeader(NetPacketDesc *packet, byte[] sourceIP, byte[] destIP, byte protocol)
        {
            packet->start -= (short)sizeof(IPV4Header);
            
            IPV4Header* header = (IPV4Header*)(packet->buffer + packet->start);

            header->Version = (4 << 4) | 5;
            header->ServicesField = 0;
            header->totalLength = Utilities.Byte.ReverseBytes((ushort)(packet->end - packet->start));
            header->ID = Utilities.Byte.ReverseBytes(0xa836); // TODO: FIX THIS!
            header->FragmentOffset = 0;
            header->TTL = 250;
            header->Protocol = protocol;
            // The checksum calculation needs to be done with header checksum filled in as zero
            // then it is filled in later
            header->HeaderChecksum = 0;

            Memory.Memcpy(header->Source, Util.ObjectToVoidPtr(sourceIP), 4);
            Memory.Memcpy(header->Destination, Util.ObjectToVoidPtr(destIP), 4);
            
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
        public static unsafe void Send(NetPacketDesc* packet, byte[] destIP, byte protocol)
        {
            byte[] sourceIP = Util.PtrToArray(Network.Settings->IP);
            addHeader(packet, sourceIP, destIP, protocol);

            Ethernet.Send(packet, destIP, EthernetTypes.IPV4);
        }
    }
}
