using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    enum EthernetTypes
    {
        IPV4 = 0x0800,
        WoL = 0x0842
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct EthernetHeader
    {
        public fixed byte Destination[6];
        public fixed byte Source[6];
        public UInt16 Protocol;
    }

    class Ethernet
    {

        public static unsafe EthernetHeader *CreateHeaderPtr(byte[] dest, byte[] src, EthernetTypes protocol)
        {

            EthernetHeader* header = (EthernetHeader*)Heap.Alloc(sizeof(EthernetHeader));

            for (int i = 0; i < 6; i++)
                header->Destination[i] = dest[i];

            for (int i = 0; i < 6; i++)
                header->Source[i] = src[i];

            header->Protocol = (UInt16)ByteUtil.ReverseBytes((ushort)protocol);

            return header;
        }



        public static unsafe EthernetHeader CreateHeader(byte[] dest, byte [] src, EthernetTypes protocol)
        {
            EthernetHeader* ptr = CreateHeaderPtr(dest, src, protocol);
            EthernetHeader header = *ptr;
            Heap.Free(ptr);
            
            return header;
        }



        public static unsafe EthernetHeader *ReadHeader(byte *header)
        {

            EthernetHeader* outHeader = (EthernetHeader*)Heap.Alloc(sizeof(EthernetHeader));
            Memory.Memcpy(outHeader, header, sizeof(EthernetHeader));

            return outHeader;
        }
    }
}
