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
        WoL = 0x0842,
        ARP = 0x0806
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct EthernetHeader
    {
        public fixed byte Destination[6];
        public fixed byte Source[6];
        public UInt16 Protocol;
    }

    /// <summary>
    /// LAYER 2 - ETHERNET
    /// </summary>
    class Ethernet
    {

        /// <summary>
        /// Add header to packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="dest">Destination MAC</param>
        /// <param name="src">Source MAC</param>
        /// <param name="protocol">Protocol</param>
        /// <returns></returns>
        private static unsafe EthernetHeader *addHeader(NetPacketDesc *packet, byte[] dest, byte[] src, EthernetTypes protocol)
        {
            packet->start -= (short)sizeof(EthernetHeader);

            EthernetHeader* header = (EthernetHeader*)(packet->buffer + packet->start);
            
            for (int i = 0; i < 6; i++)
                header->Destination[i] = dest[i];

            for (int i = 0; i < 6; i++)
                header->Source[i] = src[i];

            header->Protocol = (UInt16)Utilities.Byte.ReverseBytes((ushort)protocol);

            return header;
        }

        /// <summary>
        /// Send ethernet packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="destMAC">Destination MAC</param>
        /// <param name="protocol">Protocol</param>
        public static unsafe void Send(NetPacketDesc *packet, byte[] destIP, EthernetTypes protocol)
        {
            // 1 TIME PLEASE
            byte[] srcMAC = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(srcMAC));

            // Get MAC from ARP :D
            byte* dstMac = (byte*)Heap.Alloc(6);

            bool found = Route.FindRoute(destIP, dstMac);
            
            if (!found)
                return;
            
            addHeader(packet, Util.PtrToArray(dstMac), srcMAC, protocol);

            Network.Transmit(packet);

            Heap.Free(dstMac);
        }


        /// <summary>
        /// Send ethernet packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="destMAC">Destination MAC</param>
        /// <param name="protocol">Protocol</param>
        public static unsafe void SendMAC(NetPacketDesc* packet, byte[] destMac, EthernetTypes protocol)
        {
            // 1 TIME PLEASE
            byte[] srcMAC = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(srcMAC));
            
            addHeader(packet, destMac, srcMAC, protocol);

            Network.Transmit(packet);
        }
    }
}
