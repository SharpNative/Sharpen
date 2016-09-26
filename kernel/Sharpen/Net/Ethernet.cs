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

    /// <summary>
    /// LAYER 4 - ETHERNET
    /// </summary>
    class Ethernet
    {

        public static unsafe EthernetHeader *FillHeader(NetBufferDescriptor *packet, byte[] dest, byte[] src, EthernetTypes protocol)
        {
            packet->start -= (short)sizeof(EthernetHeader);

            EthernetHeader* header = (EthernetHeader*)(packet->buffer + packet->start);
            
            for (int i = 0; i < 6; i++)
                header->Destination[i] = dest[i];

            for (int i = 0; i < 6; i++)
                header->Source[i] = src[i];

            header->Protocol = (UInt16)ByteUtil.ReverseBytes((ushort)protocol);

            return header;
        }

        public static unsafe void Send(NetBufferDescriptor *packet, byte[] destMAC, EthernetTypes protocol)
        {
            // 1 TIME PLEASE
            byte[] srcMAC = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(srcMAC));

            FillHeader(packet, destMAC, srcMAC, protocol);

            Network.Transmit(packet);
        }
    }
}
