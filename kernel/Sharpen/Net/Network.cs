using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{ 

    public class Network
    {

        /// <summary>
        /// Network device struct
        /// </summary>
        public struct NetDevice
        {
            public uint ID;
            public TransmitAction Transmit;
            public GetMACAction GetMac;
        }
        
        /// <summary>
        /// Transmit packet
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="size"></param>
        public unsafe delegate void TransmitAction(byte *bytes, uint size);
        
        /// <summary>
        /// Get mac address
        /// </summary>
        /// <param name="mac">6 byte struct to read the mac address in</param>
        public unsafe delegate void GetMACAction(byte *mac);


        public static NetDevice m_dev;

        public static NetDevice Device
        {
            get { return m_dev; }
        }

        /// <summary>
        /// Set network device
        /// </summary>
        /// <param name="device">Network device</param>
        public static void Set(NetDevice device)
        {
            Console.Write("[NET] Primary network device set with ID ");
            Console.WriteHex((int)device.ID);
            Console.WriteLine("");
            m_dev = device;
        }

        /// <summary>
        /// Transmit packet
        /// </summary>
        /// <param name="bytes">byte buffer</param>
        /// <param name="size">packet size</param>
        public static unsafe void Transmit (byte* bytes, uint size)
        {
            Console.Write("[NET] Transmit packet with ");
            Console.WriteNum((int)size);
            Console.WriteLine(" bytes");

            if (m_dev.ID != 0)
                m_dev.Transmit(bytes, size);
        }

        /// <summary>
        /// Get mac address
        /// </summary>
        /// <param name="mac">6 byte struct to read the mac address in</param>
        public static unsafe void GetMac(byte* mac)
        {
            if (m_dev.ID != 0)
                m_dev.GetMac(mac);
        }

        public static unsafe void HandlePacket(byte[] buffer, int size)
        {
            Console.WriteLine("Incoming packet!");

            byte* bufPtr = (byte*)Util.ObjectToVoidPtr(buffer);

            EthernetHeader* header = (EthernetHeader*)bufPtr;
            
            ushort proto = ByteUtil.ReverseBytes(header->Protocol);

            // We only handle IPV4 for now :)
            if (proto != 0x800)
                return;
            
            IPV4Header* headerr = (IPV4Header*)bufPtr;

            Console.WriteLine("\nID:");
            Console.WriteHex(ByteUtil.ReverseBytes(headerr->ID));

            // WE NEED A UDP PACKET :)
            if (headerr->Protocol != 0x11)
                return;


            // we now have an udp packet :)
            UDPHeader* udp = (UDPHeader*)bufPtr;

            int sizePacket = ByteUtil.ReverseBytes(udp->Length) - 8;

            Console.Write("\nSize:");
            Console.WriteNum(sizePacket);

            for (;;) ;
        }

    }
}
