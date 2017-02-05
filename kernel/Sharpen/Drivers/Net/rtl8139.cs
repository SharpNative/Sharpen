#define RTL_DEBUG

using Sharpen.Arch;
using Sharpen.Drivers.Char;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    /// <summary>
    /// NOTE: Redo this driver and fix magic values and documentation
    /// </summary>
    class rtl8139
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
         * CONFIG1 bits
         */
        private const byte CONF1_PMEM = (1 << 0);

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
        private const uint TC_DMA_512  = (5 << 8);
        private const uint TC_DMA_256  = (4 << 8);

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
        private const byte MS_RXPF          = (1 << 0);
        private const byte MS_TXPF          = (1 << 1);
        private const byte MS_LINKB         = (1 << 2);
        private const byte MS_SPEED_10      = (1 << 3);
        private const byte MS_AUX_STATUS    = (1 << 4);
        private const byte MS_RXFCE         = (1 << 6);
        private const byte MS_TXFCE         = (1 << 7);

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
        private static byte[] m_buffer;
        private static byte[] m_transmit0;
        private static byte[] m_transmit1;
        private static byte[] m_transmit2;
        private static byte[] m_transmit3;

        private static bool m_linkFail;
        private static bool m_100mbit;
        private static ushort m_irq_num;
        private static uint m_curBuffer;

        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static unsafe void initHandler(PciDevice dev)
        {
            m_io_base = (ushort)(dev.BAR0.Address);

            /**
             * Check if portio
             */
            if((dev.BAR0.flags & PCI.BAR_IO) == 0)
            {
                Console.WriteLine("[RTL8139] MMIO not supported!");

                return;
            }

            /**
             * Initalize and allocate buffers
             */
            initalizeBuffers();

            /**
             * Read IRQ number and map
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
            PortIO.Out8((ushort)(m_io_base + CONF1_PMEM), CONF1_PMEM);

            /**
             *  Do a software reset
             */
            softwareReset();

            /**
             * Setup interrupts
             */
            setInterruptMask(IMR_TOK | IMR_TER);

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
             * Enable receiving and transmitting!
             */
            PortIO.Out8((ushort)(m_io_base + REG_CMD), CMD_TE | CMD_RE);

            Console.WriteLine("[RTL8139] Intialized");

            /**
             * Register device as the main network device
             */
            Network.NetDevice netDev = new Network.NetDevice();
            netDev.ID = dev.Device;
            netDev.Transmit = Transmit;
            netDev.GetMac = GetMac;

            Network.Set(netDev);

            for (int i = 0; i < 6; i++)
            {
                Console.WriteHex(m_mac[i]);
                Console.Write(':');
            }
            Console.WriteLine("");
            
        }

        /// <summary>
        /// Update link status
        /// </summary>
        private static unsafe void updateLinkStatus()
        {
            byte b = PortIO.In8((ushort)(m_io_base + REG_MS));

            m_linkFail = (b & MS_LINKB) == 0;
            m_100mbit = (b & MS_SPEED_10) == 0;
        }

        /// <summary>
        /// Set interrupt mask
        /// </summary>
        private static unsafe void setInterruptMask(ushort value)
        {

            PortIO.Out16((ushort)(m_io_base + REG_IMR), value);
        }

        /// <summary>
        /// Transmit initalization
        /// </summary>
        private static unsafe void txInit()
        {

            m_curBuffer = 0;

            /**
             * Set transmit buffers
             */
            PortIO.Out32((ushort)(m_io_base + REG_TSAD0), (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_transmit0)));
            PortIO.Out32((ushort)(m_io_base + REG_TSAD1), (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_transmit1)));
            PortIO.Out32((ushort)(m_io_base + REG_TSAD2), (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_transmit2)));
            PortIO.Out32((ushort)(m_io_base + REG_TSAD3), (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_transmit3)));

            /**
             * Set transmit configuration register
             */
            PortIO.Out32((ushort)(m_io_base + REG_TC), TC_DMA_2048);
        }

        /// <summary>
        /// Receive initalization
        /// </summary>
        private static unsafe void rxInit()
        {

            /**
             * Set receive buffer
             */
            PortIO.Out32((ushort)(m_io_base + REG_BUF), (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_buffer)));

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
        /// Initalize buffers
        /// </summary>
        private static void initalizeBuffers()
        {

            m_buffer = new byte[8 * 1024];
            m_transmit0 = new byte[8192 + 16];
            m_transmit1 = new byte[8192 + 16];
            m_transmit2 = new byte[8192 + 16];
            m_transmit3 = new byte[8192 + 16];
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
             * Max packetsize 4096
             */
            if (size >= 4096)
                return;

            /**
             * Select buffer and descriptor address
             */
            byte* bufferPtr = (byte*)Util.ObjectToVoidPtr(m_transmit0);
            ushort address = (ushort)(m_io_base + REG_TSD0);

            if(m_curBuffer == 1)
            {
                bufferPtr = (byte*)Util.ObjectToVoidPtr(m_transmit1);
                address = (ushort)(m_io_base + REG_TSD1);
            }
            else if(m_curBuffer == 2)
            {
                bufferPtr = (byte*)Util.ObjectToVoidPtr(m_transmit2);
                address = (ushort)(m_io_base + REG_TSD2);
            }
            else if(m_curBuffer == 3)
            {
                bufferPtr = (byte*)Util.ObjectToVoidPtr(m_transmit3);
                address = (ushort)(m_io_base + REG_TSD3);
            }

            /**
             * Avoid overflow
             */
            m_curBuffer++;
            if (m_curBuffer > 3)
                m_curBuffer = 0;

            /**
             * Set data
             */
            Memory.Memcpy(bufferPtr, bytes, (int)size);

            /**
             * Write size to chip to fire!
             */
            PortIO.Out32(address, size);
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
            setInterruptMask(0);

#if RTL_DEBUG
            if((ISR & ISR_TOK) > 0)
            {
                Console.WriteLine("[RTL8139] Transmit OK!");
            }

            if ((ISR & ISR_TER) > 0)
            {
                Console.WriteLine("[RTL8139] Transmit Error!");
            }
#endif

            if ((ISR & ISR_ROK) > 0)
            {
                HandlePackets();
            }
            
            PortIO.Out16((ushort)(m_io_base + REG_ISR), ISR);
            setInterruptMask(IMR_TOK | IMR_TER);
        }

        private static unsafe void HandlePackets()
        {
            /**
             * While buffer is not empty...
             */
            while((PortIO.In8((ushort)(m_io_base + REG_CMD)) & CMD_BUFE) == 0)
            {

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