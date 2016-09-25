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

    class UDP
    {
        private const byte PROTOCOL_UDP = 17;

        public static unsafe void Init()
        {
            IPV4.RegisterHandler(0x11, handler);
        }

        private static unsafe void handler(uint xid, byte *buffer, uint size)
        {
            // :-)
            UDPHeader* header = (UDPHeader*)buffer;

        }

        private static unsafe UDPHeader* FillHeader(NetBufferDescriptor *packet, byte[] destIP, UInt16 sourcePort, UInt16 DestinationPort)
        {
            UDPHeader* header = (UDPHeader*)packet->buffer;

            packet->start -= (short)sizeof(UDPHeader);


            header->SourcePort = ByteUtil.ReverseBytes(sourcePort);
            header->DestinationPort = ByteUtil.ReverseBytes(DestinationPort);
            header->Length = ByteUtil.ReverseBytes((ushort)(packet->end - packet->start));
            header->Checksum = 0x1F59; // Isn't required :)

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
    }
}
