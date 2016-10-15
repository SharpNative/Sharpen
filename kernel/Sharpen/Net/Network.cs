#define NETWORK_DEBUG

using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{ 

    public unsafe class Network
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

        // Network packet type handler
        public unsafe delegate void PackerHandler(byte *buffer, uint size);

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


        private static NetDevice m_dev;

        public static NetworkSettings *Settings { get; private set; }

        private static PackerHandler[] m_handlers;

        public static NetDevice Device
        {
            get { return m_dev; }
        }

        public static void Init()
        {
            m_handlers = new PackerHandler[65536];

            Settings = (NetworkSettings*)Heap.Alloc(sizeof(NetworkSettings));
            Memory.Memset(Settings, 0, sizeof(NetworkSettings));
        }

        public static void RegisterHandler(ushort protocol, PackerHandler handler)
        {
#if NETWORK_DEBUG
            Console.Write("[NET] Registered protocol handler for: ");
            Console.WriteHex(protocol);
            Console.WriteLine("");
#endif
            m_handlers[protocol] = handler;
        }

        /// <summary>
        /// Set network device
        /// </summary>
        /// <param name="device">Network device</param>
        public static void Set(NetDevice device)
        {
#if NETWORK_DEBUG
            Console.Write("[NET] Primary network device set with ID ");
            Console.WriteHex((int)device.ID);
            Console.WriteLine("");
#endif
            m_dev = device;
        }

        /// <summary>
        /// Transmit packet
        /// </summary>
        /// <param name="bytes">byte buffer</param>
        /// <param name="size">packet size</param>
        public static unsafe void Transmit (NetPacketDesc* packet)
        {
            int size = packet->end - packet->start;
            
            byte* buffer = (byte *)Heap.Alloc(size);
            Memory.Memcpy(buffer, packet->buffer + packet->start, size);

            // LOGIC

#if NETWORK_DEBUG
            Console.Write("[NET] Transmit packet with ");
            Console.WriteNum((int)size);
            Console.WriteLine(" bytes");
#endif

            if (m_dev.ID != 0)
                m_dev.Transmit(buffer, (uint)size);

            Heap.Free(buffer);
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
            byte* bufPtr = (byte*)Util.ObjectToVoidPtr(buffer);

            EthernetHeader* header = (EthernetHeader*)bufPtr;
            
            ushort proto = ByteUtil.ReverseBytes(header->Protocol);
            
            m_handlers[proto]?.Invoke(bufPtr + sizeof(EthernetHeader), (uint)size);
        }

    }
}
