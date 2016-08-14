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
            byte* buffer = (byte*)Heap.Alloc(102);

            for (int i = 0; i < 6; i++)
                buffer[i] = 0xFF;


            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 6; j++)
                    buffer[6 + (i * 6) + j] = mac[j];

            Network.Transmit(buffer, 102);

            Heap.Free(buffer);
        }
    }
}
