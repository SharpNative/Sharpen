using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{

    class ARP
    {

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe struct ARPHeader
        {
            public EthernetHeader Ethernet;

            public ushort HardwareType;
            public ushort ProtocolType;
            public byte HardwareAdrLength;
            public byte ProtocolAdrLength;
            public ushort Opcode;
            public fixed byte SrcHw[6];
            public fixed byte SrcIP[4];
            public fixed byte DstHw[6];
            public fixed byte DstIP[4];
        }

        /// <summary>
        /// Register ARP protocol
        /// </summary>
        public static unsafe void Init()
        {
            Network.RegisterHandler(0x0806, handler);
        }

        /// <summary>
        /// Handle packet
        /// </summary>
        /// <param name="buffer">Buffer pointer</param>
        /// <param name="size">Size</param>
        private static unsafe void handler(byte* buffer, uint size)
        {
            ARPHeader* header = (ARPHeader*)buffer;

            // Only IPV4 - Ethernet ARP packages allowed! :)
            if (ByteUtil.ReverseBytes(header->ProtocolType) != 0x0800 || ByteUtil.ReverseBytes(header->HardwareType) != 1)
                return;

            // Fancy wireshark message ;)
            Console.Write("Who has ");
            for(int i =0; i < 4; i++)
            {
                Console.WriteNum(header->DstIP[i]);
                if(i < 3)
                    Console.Write(".");
            }
            Console.Write("? tell ");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteNum(header->SrcIP[i]);
                if (i < 3)
                    Console.Write(".");
            }
            Console.WriteLine("");
        }

    }
}
