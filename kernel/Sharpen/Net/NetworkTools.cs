using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class NetworkTools
    {

        /// <summary>
        /// Wake on lan
        /// </summary>
        /// <param name="mac">MAC address to wake</param>
        public static unsafe void WakeOnLan(byte[] mac)
        {
            Console.Write("Waking up by LAN, MAC: ");
            for(int i =0; i < 6; i++)
            {
                Console.WriteHex(mac[i]);
                if(i != 5)
                    Console.Write(":");
            }
            Console.WriteLine("");

            byte[] ownmac = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(ownmac));

            byte[] broadcast = new byte[6]; 
            for (int i = 0; i < 6; i++)
                broadcast[i] = 0xFF;

            NetBufferDescriptor* packet = NetBuffer.Alloc();

            int offset = 0;
            for (int i = 0; i < 6; i++)
                packet->buffer[packet->start + offset + i] = 0xFF;

            offset += 6;

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 6; j++)
                    packet->buffer[packet->start + offset + (i * 6) + j] = mac[j];

            packet->end += 102;

            Ethernet.Send(packet, ownmac, broadcast, EthernetTypes.WoL);

            Heap.Free(packet);
        }
    }
}
