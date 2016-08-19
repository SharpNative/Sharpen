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
        public EthernetHeader EthernetHeader;
        
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

    class IPV4
    {
        public static unsafe IPV4Header CreateHeader(byte[] dest)
        {
            byte[] mymac = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(mymac));

            IPV4Header header = new IPV4Header();
            header.EthernetHeader = Ethernet.CreateHeader(dest, mymac, EthernetTypes.IPV4);

            header.Version = 0x45;
            header.ServicesField = 0;
            header.totalLength = 0x2C01;
            header.ID = 0x36A8;
            header.FragmentOffset = 0;
            header.TTL = 250;
            header.Protocol = 0x11;
            header.HeaderChecksum = 0x178B;


            return header;
        }

        public static unsafe IPV4Header *CreateHeaderPtr(byte[] destMac, byte[] sourceIP, byte[] destIP, byte protocol, UInt16 size)
        {
            byte[] mymac = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(mymac));


            IPV4Header* header = (IPV4Header*)Heap.Alloc(sizeof(IPV4Header));
            header->EthernetHeader = Ethernet.CreateHeader(destMac, mymac, EthernetTypes.IPV4);

            header->Version = 0x45;
            header->ServicesField = 0;
            header->totalLength = ByteUtil.ReverseBytes(size);
            header->ID = 0x36A8;
            header->FragmentOffset = 0;
            header->TTL = 250;
            header->Protocol = protocol;
            header->HeaderChecksum = 0xB817;
            for (int i = 0; i < 4; i++)
                header->Source[i] = sourceIP[i];
            for (int i = 0; i < 4; i++)
                header->Destination[i] = destIP[i];


            return header;
        }
    }
}
