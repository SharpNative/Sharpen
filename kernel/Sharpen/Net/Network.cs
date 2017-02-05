// #define NETWORK_DEBUG

using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Utilities;

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

        public struct NetRecBuffer
        {
            public int Size;
            public byte* Buffer;
        }

        /// <summary>
        /// Packet handler
        /// </summary>
        /// <param name="srcMac">The source MAC address</param>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The size of the buffer</param>
        public unsafe delegate void PackerHandler(byte[] srcMac, byte* buffer, uint size);

        /// <summary>
        /// Transmit packet
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The size of the buffer</param>
        public unsafe delegate void TransmitAction(byte* buffer, uint size);

        /// <summary>
        /// Get mac address
        /// </summary>
        /// <param name="mac">6 byte buffer to put the MAC address in</param>
        public unsafe delegate void GetMACAction(byte* mac);

        private static Queue m_recPacketQueue;

        private static NetDevice m_dev;

        public static NetworkSettings* Settings { get; private set; }

        private static PackerHandler[] m_handlers;


        public static NetDevice Device
        {
            get { return m_dev; }
        }

        /// <summary>
        /// Initializes the main networking
        /// </summary>
        public static void Init()
        {
            m_handlers = new PackerHandler[65536];

            Settings = (NetworkSettings*)Heap.Alloc(sizeof(NetworkSettings));
            Memory.Memclear(Settings, sizeof(NetworkSettings));

            m_recPacketQueue = new Queue();

            Thread packetHandler = new Thread();
            packetHandler.Context.CreateNewContext(Util.MethodToPtr(handlePackets), 0, null, true);
            Tasking.KernelTask.AddThread(packetHandler);
        }

        /// <summary>
        /// Registers a handler for a protocol
        /// </summary>
        /// <param name="protocol">The protocol</param>
        /// <param name="handler">The handler</param>
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
        public static unsafe void Transmit(NetPacketDesc* packet)
        {
            int size = packet->end - packet->start;
            if (size < 0)
                return;
            
            byte* buffer = (byte*)Heap.Alloc(size);
            if (buffer == null)
                return;

            Memory.Memcpy(buffer, packet->buffer + packet->start, size);

#if NETWORK_DEBUG
            Console.Write("[NET] Transmit packet with ");
            Console.WriteNum(size);
            Console.WriteLine(" bytes");
#endif

            m_dev.Transmit?.Invoke(buffer, (uint)size);

            Heap.Free(buffer);
        }

        /// <summary>
        /// Get mac address
        /// </summary>
        /// <param name="mac">6 byte struct to read the mac address in</param>
        public static unsafe void GetMac(byte* mac)
        {
            m_dev.GetMac?.Invoke(mac);
        }

        /// <summary>
        /// Add packet for handling
        /// </summary>
        /// <param name="buffer">The packet buffer</param>
        /// <param name="size">The packet buffer size</param>
        public static unsafe void QueueReceivePacket(byte[] buffer, int size)
        {
            if (size < sizeof(EthernetHeader))
            {
#if NETWORK_DEBUG
                Console.WriteLine("[NET] size < sizeof(EthernetHeader)");
#endif
                return;
            }

            NetRecBuffer* netBuf = (NetRecBuffer*)Heap.Alloc(sizeof(NetRecBuffer));
            if (netBuf == null)
            {
#if NETWORK_DEBUG
                Console.WriteLine("[NET] netBuf = null");
#endif
                return;
            }

            netBuf->Size = size;
            netBuf->Buffer = (byte*)Heap.Alloc(size);
            if (netBuf->Buffer == null)
            {
#if NETWORK_DEBUG
                Console.WriteLine("[NET] netbuf->buffer == null");
#endif
                Heap.Free(netBuf);
                return;
            }

            Memory.Memcpy(netBuf->Buffer, Util.ObjectToVoidPtr(buffer), size);
            m_recPacketQueue.Push(netBuf);
        }

        /// <summary>
        /// Wait for packets and handle it!
        /// </summary>
        private static unsafe void handlePackets()
        {
            while (true)
            {
                while (m_recPacketQueue.IsEmpty())
                    Tasking.Yield();
                
                NetRecBuffer* buffer = (NetRecBuffer*)m_recPacketQueue.Pop();
#if NETWORK_DEBUG
                if (buffer == null)
                {
                    Console.WriteLine("[NET] buffer == null?!");
                    continue;
                }
#endif

                handlePacket(Util.PtrToArray(buffer->Buffer), buffer->Size);

                Heap.Free(buffer->Buffer);
                Heap.Free(buffer);
            }
        }

        /// <summary>
        /// Handle single receive packet
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void handlePacket(byte[] buffer, int size)
        {
            byte* bufPtr = (byte*)Util.ObjectToVoidPtr(buffer);

            EthernetHeader* header = (EthernetHeader*)bufPtr;

            ushort proto = Byte.ReverseBytes(header->Protocol);
            
            m_handlers[proto]?.Invoke(Util.PtrToArray(header->Source), bufPtr + sizeof(EthernetHeader), (uint)size);
        }

        /// <summary>
        /// Get hostname
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return String.Clone("SHARPEN");
        }
    }
}
