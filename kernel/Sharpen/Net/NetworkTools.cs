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

            EthernetHeader* header = Ethernet.CreateHeaderPtr(broadcast, ownmac, EthernetTypes.WoL);

            byte* buffer = (byte*)Heap.Alloc(116);
            Memory.Memcpy(buffer, header, sizeof(EthernetHeader));
            Heap.Free(header);

            int offset = sizeof(EthernetHeader);
            for (int i = 0; i < 6; i++)
                buffer[offset + i] = 0xFF;

            offset += 6;

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 6; j++)
                    buffer[offset + (i * 6) + j] = mac[j];

            Network.Transmit(buffer, 116);

            Heap.Free(buffer);
        }
    }
}
