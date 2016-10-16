//#define NETWORK_DEBUG

using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Task;
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

        public struct NetRecBuffer
        {
            public int Size;
            public byte* Buffer;
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

        private static Queue m_recPacketQueue;

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

            m_recPacketQueue = new Queue();
            
            Task.Task newTask = Tasking.CreateTask(Util.MethodToPtr(handlePackets), TaskPriority.NORMAL, null, 0, Tasking.SpawnFlags.KERNEL);
            newTask.PageDir = Paging.KernelDirectory;
            //Tasking.ScheduleTask(newTask);
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

        /// <summary>
        /// Add packet for handling
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void QueueReceivePacket(byte[] buffer, int size)
        {
            //NetRecBuffer* netBuf = (NetRecBuffer*)Heap.Alloc(sizeof(NetRecBuffer));
            //netBuf->Size = size;
            //netBuf->Buffer = (byte*)Heap.Alloc(size);
            //Memory.Memcpy(netBuf->Buffer, Util.ObjectToVoidPtr(buffer), size);

            // Queue packet
            //m_recPacketQueue.Push(netBuf);
            handlePacket(buffer, size);
        }

        /// <summary>
        /// Wait for packets and handle it!
        /// </summary>
        private static unsafe void handlePackets()
        {
            while(true)
            {
                NetRecBuffer* buffer = (NetRecBuffer *)m_recPacketQueue.Pop();
                if (buffer == null)
                {
                    continue;
                }
                
                //handlePacket(Util.PtrToArray(buffer->Buffer), buffer->Size);

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
            
            ushort proto = ByteUtil.ReverseBytes(header->Protocol);

            
            m_handlers[proto]?.Invoke(bufPtr + sizeof(EthernetHeader), (uint)size);

        }

    }
}
