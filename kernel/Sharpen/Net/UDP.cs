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
        public IPV4Header ipv4;

        public UInt16 SourcePort;
        public UInt16 DestinationPort;
        public UInt16 Length;
        public UInt16 Checksum;
    }

    class UDP
    {

        public static unsafe UDPHeader* createHeader(byte[] destMac, byte[] destIP, UInt16 sourcePort, UInt16 DestinationPort, UInt16 length)
        {
            UDPHeader* header = (UDPHeader*)Heap.Alloc(sizeof(UDPHeader));

            length += 8;

            // TODO: FIX THIS!
            IPV4Header* hdrPtr = IPV4.CreateHeaderPtr(destMac, new byte[4], destIP, 0x11, (UInt16)(length + (ushort)20));
            header->ipv4 = *hdrPtr;
            Heap.Free(hdrPtr);

            header->SourcePort = ByteUtil.ReverseBytes(sourcePort);
            header->DestinationPort = ByteUtil.ReverseBytes(DestinationPort);
            header->Length = ByteUtil.ReverseBytes(length);
            header->Checksum = 0x0000; // Isn't required :)

            return header;
        }
    }
}
