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
    unsafe struct UDPHeader
    {
        public UInt16 SourcePort;
        public UInt16 DestinationPort;
        public UInt16 Length;
        public UInt16 Checksum;
    }

    /// <summary>
    /// LAYER 2 - UDP
    /// </summary>
    class UDP
    {
        private const byte PROTOCOL_UDP = 17;

        // TODO: find better way to handle this, this is wasted space
        private static UDPPacketHandler[] m_handlers;

        /// <summary>
        /// UDP packet handler
        /// </summary>
        /// <param name="xid"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public unsafe delegate void UDPPacketHandler(uint xid, byte* buffer, uint size);

        /// <summary>
        /// Initializes UDP
        /// </summary>
        public static unsafe void Init()
        {
            m_handlers = new UDPPacketHandler[65536];
            IPV4.RegisterHandler(0x11, handler);

#if UDP_DEBUG
            Console.WriteLine("[UDP] Registered handler on 0x11");
#endif
        }

        /// <summary>
        /// Bind port
        /// </summary>
        /// <param name="port">The port</param>
        /// <param name="handler">The callback</param>
        public static void Bind(short port, UDPPacketHandler handler)
        {
#if UDP_DEBUG
            Console.Write("[UDP] Bind Port: ");
            Console.WriteNum(port);
            Console.WriteLine("");
#endif

            m_handlers[port] = handler;
        }

        private static unsafe void handler(uint xid, byte *buffer, uint size)
        {

            UDPHeader* header = (UDPHeader*)buffer;

            ushort destPort = (ushort)ByteUtil.ReverseBytes(header->DestinationPort);

#if UDP_DEBUG_PACKETS
            
            ushort sourcePort = (ushort)ByteUtil.ReverseBytes(header->SourcePort);

            Console.Write("[UDP] Receive from ");
            Console.WriteNum(sourcePort);
            Console.Write(" to ");
            Console.WriteNum(destPort);
            Console.WriteLine("");
#endif

            m_handlers[destPort]?.Invoke(xid, buffer + sizeof(UDPHeader), (uint)(header->Length - 8));
        }
        

        private static unsafe UDPHeader* FillHeader(NetBufferDescriptor *packet, byte[] destIP, UInt16 sourcePort, UInt16 DestinationPort)
        {
            packet->start -= (short)sizeof(UDPHeader);

            UDPHeader* header = (UDPHeader*)(packet->buffer + packet->start);


            header->SourcePort = ByteUtil.ReverseBytes(sourcePort);
            header->DestinationPort = ByteUtil.ReverseBytes(DestinationPort);
            header->Length = ByteUtil.ReverseBytes((ushort)(packet->end - packet->start));
            header->Checksum = 0; // Isn't required :)

            return header;
        }
        
        public static unsafe void Send(byte[] destMac, byte[] destIP, ushort srcPort, ushort DestPort, byte[] data, int size)
        {
            // No support for packets over 1500 bytes
            if (size >= 1500)
                return;

            NetBufferDescriptor* buf = NetBuffer.Alloc();

            Memory.Memcpy(buf->buffer + buf->start, Util.ObjectToVoidPtr(data), size);

            FillHeader(buf, destIP, srcPort, DestPort);

            IPV4.Send(buf, destMac, destIP, PROTOCOL_UDP);
        }


        public static unsafe void Send(NetBufferDescriptor *packet, byte[] destMac, byte[] destIP, ushort srcPort, ushort DestPort)
        {
            // No support for packets over 1500 bytes
            if (packet->end - packet->start >= 1500)
                return;
            
            FillHeader(packet, destIP, srcPort, DestPort);

            IPV4.Send(packet, destMac, destIP, PROTOCOL_UDP);
        }
    }
}
