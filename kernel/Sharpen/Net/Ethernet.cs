using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Runtime.InteropServices;

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
        private static unsafe EthernetHeader* addHeader(NetPacketDesc* packet, byte[] dest, byte[] src, EthernetTypes protocol)
        {
            packet->start -= (short)sizeof(EthernetHeader);

            EthernetHeader* header = (EthernetHeader*)(packet->buffer + packet->start);
            Memory.Memcpy(header->Destination, Util.ObjectToVoidPtr(dest), 6);
            Memory.Memcpy(header->Source, Util.ObjectToVoidPtr(src), 6);

            header->Protocol = Utilities.Byte.ReverseBytes((ushort)protocol);

            return header;
        }

        /// <summary>
        /// Send ethernet packet
        /// </summary>
        /// <param name="packet">Packet structure</param>
        /// <param name="destMAC">Destination MAC</param>
        /// <param name="protocol">Protocol</param>
        public static unsafe void Send(NetPacketDesc* packet, byte[] destIP, EthernetTypes protocol)
        {
            // 1 TIME PLEASE
            byte* srcMAC = (byte*)Heap.Alloc(6);
            Network.GetMac(srcMAC);

            // Get MAC from ARP :D
            byte* dstMac = (byte*)Heap.Alloc(6);

            bool found = Route.FindRoute(destIP, dstMac);
            if (!found)
            {
                Heap.Free(srcMAC);
                Heap.Free(dstMac);
                return;
            }

            addHeader(packet, Util.PtrToArray(dstMac), Util.PtrToArray(srcMAC), protocol);

            Network.Transmit(packet);

            Heap.Free(srcMAC);
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
            byte* srcMAC = (byte*)Heap.Alloc(6);
            Network.GetMac(srcMAC);

            addHeader(packet, destMac, Util.PtrToArray(srcMAC), protocol);

            Heap.Free(srcMAC);

            Network.Transmit(packet);
        }
    }
}
