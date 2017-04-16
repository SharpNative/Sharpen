#define RTL_DEBUG

using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Net
{
    unsafe class RTL8139
    {
        /**
         * Registers
         */
        private const ushort REG_CONF1 = 0x52;
        private const ushort REG_CMD = 0x37;
        private const ushort REG_MAC = 0x00;
        private const ushort REG_BUF = 0x30;
        private const ushort REG_CBR = 0x3A;
        private const ushort REG_CAPR = 0x38;
        private const ushort REG_IMR = 0x3C;
        private const ushort REG_ISR = 0x3E;
        private const ushort REG_TC = 0x40;
        private const ushort REG_RC = 0x44;
        private const ushort REG_MS = 0x58;
        private const ushort REG_TSAD0 = 0x20;
        private const ushort REG_TSAD1 = 0x24;
        private const ushort REG_TSAD2 = 0x28;
        private const ushort REG_TSAD3 = 0x2C;
        private const ushort REG_TSD0 = 0x10;
        private const ushort REG_TSD1 = 0x14;
        private const ushort REG_TSD2 = 0x18;
        private const ushort REG_TSD3 = 0x1C;

        /**
         * Commands
         */
        private const byte CMD_BUFE = (1 << 0);
        private const byte CMD_TE = (1 << 2);
        private const byte CMD_RE = (1 << 3);
        private const byte CMD_RST = (1 << 4);

        /**
         * Receive configuration register
         */
        private const uint RC_AAP = (1 << 0);
        private const uint RC_APM = (1 << 1);
        private const uint RC_AM = (1 << 2);
        private const uint RC_AB = (1 << 3);
        private const uint RC_AR = (1 << 4);
        private const uint RC_WRAP = (1 << 7);

        /**
         * Transmit configuration register
         */
        private const uint TC_DMA_2048 = (7 << 8);
        private const uint TC_DMA_1024 = (6 << 8);
        private const uint TC_DMA_512 = (5 << 8);
        private const uint TC_DMA_256 = (4 << 8);

        /**
         * TX status bits
         */
        private const uint TX_HOST_OWNS = 0x2000;
        private const uint TX_UNDERRUN = 0x4000;
        private const uint TX_STATUS_OK = 0x8000;

        /**
         * RX status bits
         */
        private const uint RX_STATUS_OK = 0x1;
        private const uint RX_BAD_ALIGN = 0x2;
        private const uint RX_CRC_ERROR = 0x4;
        private const uint RX_TOO_LONG = 0x8;
        private const uint RX_RUNT = 0x10;

        /**
         * Interrupt mask register
         */
        private const ushort IMR_ROK = (1 << 0);
        private const ushort IMR_RER = (1 << 1);
        private const ushort IMR_TOK = (1 << 2);
        private const ushort IMR_TER = (1 << 3);

        /**
         * Media Status Register
         */
        private const byte MS_RXPF = (1 << 0);
        private const byte MS_TXPF = (1 << 1);
        private const byte MS_LINKB = (1 << 2);
        private const byte MS_SPEED_10 = (1 << 3);
        private const byte MS_AUX_STATUS = (1 << 4);
        private const byte MS_RXFCE = (1 << 6);
        private const byte MS_TXFCE = (1 << 7);

        /**
         * Interrupt status register
         */
        private const ushort ISR_ROK = (1 << 0);
        private const ushort ISR_RER = (1 << 1);
        private const ushort ISR_TOK = (1 << 2);
        private const ushort ISR_TER = (1 << 3);

        private static byte[] m_mac;

        private static ushort m_io_base;

        /**
         * Buffers
         */
        private static byte* m_buffer;
        private static byte*[] m_transmits;

        private static Mutex[] m_mutexes;

        private static bool m_linkFail;
        private static bool m_100mbit;
        private static ushort m_irq_num;
        private static uint m_curTX = 0;
        private static uint m_curRX = 0;

        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static unsafe void initHandler(PciDevice dev)
        {
            m_io_base = (ushort)(dev.BAR0.Address);

            /**
             * Check if I/O bar
             */
            if ((dev.BAR0.flags & PCI.BAR_IO) == 0)
            {
                Console.WriteLine("[RTL8139] RTL8139 should be an I/O bar, not a memory bar!");
                return;
            }

            /**
             * Read IRQ number
             */
            m_irq_num = (ushort)PCI.PCIRead(dev.Bus, dev.Slot, dev.Function, 0x3C, 1);
            IRQ.SetHandler(m_irq_num, handler);

            /**
             * Enable bus mastering
             */
            ushort cmd = PCI.PCIReadWord(dev, PCI.COMMAND);
            cmd |= 0x04;
            PCI.PCIWrite(dev.Bus, dev.Slot, dev.Function, PCI.COMMAND, cmd);

            /**
             * Enable device
             */
            PortIO.Out8((ushort)(m_io_base + REG_CONF1), 0);

            /**
             *  Do a software reset
             */
            softwareReset();

            /**
             * Initalize and allocate buffers
             */
            initializeBuffers();

            /**
             * Setup interrupts
             */
            setInterruptMask(IMR_TOK | IMR_TER | IMR_ROK | IMR_RER);

            /**
             * Initalize transmit
             */
            txInit();

            /**
             * Initalize receive
             */
            rxInit();

            /**
             * Read mac address
             */
            readMac();

            /**
             * Update link status
             */
            updateLinkStatus();

            /**
             * Enable receiving and transmitting!
             */
            PortIO.Out8((ushort)(m_io_base + REG_CMD), CMD_TE | CMD_RE);

            Console.WriteLine("[RTL8139] Initialized");

            /**
             * Register device as the main network device
             */
            Network.NetDevice netDev = new Network.NetDevice();
            netDev.ID = dev.Device;
            netDev.Transmit = Transmit;
            netDev.GetMac = GetMac;

            Network.Set(netDev);
        }

        /// <summary>
        /// Update link status
        /// </summary>
        private static void updateLinkStatus()
        {
            byte b = PortIO.In8((ushort)(m_io_base + REG_MS));

            m_linkFail = ((b & MS_LINKB) == 0);
            m_100mbit = ((b & MS_SPEED_10) == 0);
        }

        /// <summary>
        /// Set interrupt mask
        /// </summary>
        private static void setInterruptMask(ushort value)
        {
            PortIO.Out16((ushort)(m_io_base + REG_IMR), value);
        }

        /// <summary>
        /// Transmit initalization
        /// </summary>
        private static void txInit()
        {
            /**
             * Set transmit buffers
             */
            for (int i = 0; i < 4; i++)
            {
                PortIO.Out32((ushort)(m_io_base + REG_TSAD0 + (i * 4)), (uint)Paging.GetPhysicalFromVirtual(m_transmits[i]));
            }

            /**
             * Set transmit configuration register
             */
            PortIO.Out32((ushort)(m_io_base + REG_TC), TC_DMA_2048);

            // Mutexes so we can't reuse a TX buffer before it has been processed
            m_mutexes = new Mutex[4];
            for (int i = 0; i < 4; i++)
            {
                m_mutexes[i] = new Mutex();
            }
        }

        /// <summary>
        /// Receive initalization
        /// </summary>
        private static void rxInit()
        {
            /**
             * Set receive buffer
             */
            PortIO.Out32((ushort)(m_io_base + REG_BUF), (uint)Paging.GetPhysicalFromVirtual(m_buffer));

            /**
             * Set receive configuration register
             */
            PortIO.Out32((ushort)(m_io_base + REG_RC), RC_AB | RC_AM | RC_APM | RC_WRAP);
        }

        /// <summary>
        /// Software reset device
        /// </summary>
        private static void softwareReset()
        {
            PortIO.Out8((ushort)(m_io_base + REG_CMD), CMD_RST);

            /**
             * Wait for device to reset, bit will be set down on reset
             */
            while ((PortIO.In8((ushort)(m_io_base + REG_CMD)) & CMD_RST) > 0)
                CPU.HLT();
        }

        /// <summary>
        /// Initialize buffers
        /// </summary>
        private static unsafe void initializeBuffers()
        {
            m_buffer = (byte*)Heap.AlignedAlloc(0x1000, 8192);
            m_transmits = new byte*[4];
            for (int i = 0; i < 4; i++)
            {
                m_transmits[i] = (byte*)Heap.AlignedAlloc(0x1000, 1536);
            }
            m_mac = new byte[6];
        }

        /// <summary>
        /// Read mac address from device
        /// </summary>
        private static void readMac()
        {
            for (int i = 0; i < 6; i++)
                m_mac[i] = PortIO.In8((ushort)(m_io_base + i));
        }

        /// <summary>
        /// Transmit packet
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="size"></param>
        public static unsafe void Transmit(byte* bytes, uint size)
        {
            /**
             * Max packetsize
             */
            if (size >= 1536)
                return;

            // Acquire mutex on current descriptor
            m_mutexes[m_curTX].Lock();

            // Copy data to buffer
            byte* bufferPtr = m_transmits[m_curTX];
            Memory.Memcpy(bufferPtr, bytes, (int)size);

            // Set status on this TX
            uint status = size & 0x1FFF;
            PortIO.Out32((ushort)(m_io_base + REG_TSD0 + (m_curTX * 4)), status);

            // Next one
            m_curTX++;
            if (m_curTX == 4)
                m_curTX = 0;
        }

        /// <summary>
        /// Get mac address implementation
        /// </summary>
        /// <param name="mac">Pointer to write mac address to</param>
        private static unsafe void GetMac(byte* mac)
        {
            for (int i = 0; i < 6; i++)
                mac[i] = m_mac[i];
        }

        /// <summary>
        /// Handle interrupt
        /// </summary>
        /// <param name="regsPtr"></param>
        private static unsafe void handler(Regs* regsPtr)
        {
            ushort ISR = PortIO.In16((ushort)(m_io_base + REG_ISR));

            if ((ISR & ISR_TOK) > 0)
            {
                // We need to read every TX
                for (int i = 0; i < 4; i++)
                {
                    if (((int)PortIO.In32((ushort)(m_io_base + REG_TSD0 + (i * 4))) & TX_STATUS_OK) > 0)
                    {
                        m_mutexes[i].Unlock();
                    }
                }
            }

#if RTL_DEBUG
            if ((ISR & ISR_TER) > 0)
            {
                Console.WriteLine("[RTL8139] Transmit error!");
            }

            if ((ISR & ISR_RER) > 0)
            {
                Console.WriteLine("[RTL8139] Receive error!");
            }
#endif

            if ((ISR & ISR_ROK) > 0)
            {
                HandlePackets();
            }

            PortIO.Out16((ushort)(m_io_base + REG_ISR), ISR);
        }

        /// <summary>
        /// Handles incoming packets
        /// </summary>
        private static unsafe void HandlePackets()
        {
            /**
             * While buffer is not empty...
             */
            while ((PortIO.In8((ushort)(m_io_base + REG_CMD)) & CMD_BUFE) == 0)
            {
                uint offset = m_curRX % 8192;

                uint status = *(uint*)(m_buffer + offset);
                uint size = (status >> 16);
                status &= 0xFFFF;

                // Add packet
                byte[] buffer = new byte[size];
                Memory.Memcpy(Util.ObjectToVoidPtr(buffer), &m_buffer[offset + 4], (int)size);
                Network.QueueReceivePacket(buffer, (int)size);
                Heap.Free(buffer);

                // Next packet and align
                m_curRX += 4 + size;
                m_curRX = (uint)((m_curRX + 3) & ~3);
                if (m_curRX > 8192)
                    m_curRX -= 8192;

                // Update receive pointer
                PortIO.Out16((ushort)(m_io_base + REG_CAPR), (ushort)(m_curRX - 16));
            }
        }

        /// <summary>
        /// Exit handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void exitHandler(PciDevice dev)
        {
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "RTL8139 Driver";
            driver.Exit = exitHandler;
            driver.Init = initHandler;

            PCI.RegisterDriver(0x10EC, 0x8139, driver);
        }
    }
}