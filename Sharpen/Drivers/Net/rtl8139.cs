using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    class rtl8139
    {
        private static readonly ushort CONFIG_1 = 0x52;

        private static readonly ushort REG_MAC = 0x00;
        private static readonly ushort REG_CMD = 0x37;
        private static readonly ushort REG_IM = 0x3C;
        private static readonly ushort REG_IS = 0x3E;
        private static readonly ushort REG_TC = 0x40;
        private static readonly ushort REG_RC = 0x44;
        private static readonly ushort REG_MS = 0x58;

        private static readonly ushort CMD_RXEMPTY = 0x01;
        private static readonly ushort CMD_TXE = 0x04;
        private static readonly ushort CMD_RXE = 0x08;
        private static readonly ushort CMD_RST = 0x10;

        private static readonly byte MS_LINKDWN = 0x04;
        private static readonly byte MS_SPEED_10 = 0x08;

        private static readonly byte[] m_mac = new byte[6];

        private static ushort m_io_base;
        private static int m_linkSpeed;
        private static bool m_connected = true;
        private static int m_irqNum;

        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static unsafe void initHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port1;

            turnOn();

            readMac();

            setInterruptMask(0xFFFF);
            IRQ.SetHandler(m_irqNum, handler);
            enableTxRx();
            updateLinkStatus();

            // Write irq "10"
            uint outVal = PCI.PCIReadWord(dev, 0x3C);
            outVal &= 0x00;
            outVal |= 10;

            PCI.PCIWrite(dev.Bus, dev.Slot, dev.Function, 0x3C, outVal);

            m_irqNum = PCI.PCIReadWord(dev, 0x3C) & 0xFF;

            Console.WriteLine("");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("RTL8139 registers");
            Console.WriteLine("");
            Console.WriteLine("Status:");
            Console.Write("MAC: ");
            for(int i = 0; i < 6; i++)
            {
                Console.WriteHex(m_mac[i]);
                Console.PutChar(' ');
            } 
            Console.WriteLine("");
            Console.Write("Linkspeed: ");
            Console.WriteNum(m_linkSpeed);
            Console.WriteLine("");
            Console.Write("Linkstate: ");
            Console.WriteLine((m_connected) ? "UP" : "DOWN");
            Console.Write("IRQ number: ");
            Console.WriteNum(m_irqNum);
            Console.WriteLine("");

        }

        private static unsafe void handler(Regs* regsPtr)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("RTL8139 ");
            Console.WriteLine("-------------------------");
        }

        /// <summary>
        /// Turn device on
        /// </summary>
        private static void turnOn()
        {
            PortIO.Out8((ushort)(m_io_base + CONFIG_1), 0x00);
        }

        /// <summary>
        /// Read permanent mac address from device
        /// </summary>
        private static void readMac()
        {
            for(int i = 0; i < 6; i++)
                m_mac[i] = PortIO.In8((ushort)(m_io_base + REG_MAC + i));
        }

        /// <summary>
        /// Enable TX and RX
        /// </summary>
        private static void enableTxRx()
        {
            // Enable TX and RX
            PortIO.Out8((ushort)(m_io_base + REG_CMD), (byte)(CMD_TXE | CMD_RXE));

            // Settings RC and TC values
            PortIO.Out32((ushort)(m_io_base + REG_TC), 0x700);
            PortIO.Out32((ushort)(m_io_base + REG_RC), 0x800B780);
        }

        /// <summary>
        /// Set interrupt mask
        /// </summary>
        private static void setInterruptMask(ushort mask)
        {
            PortIO.Out16((ushort)(m_io_base + REG_IM), mask); // for now enable everything!
        }
        
        /// <summary>
        /// Acknowledge interrupt
        /// </summary>
        private static void ackowledgeInterrupts()
        {
            PortIO.Out16((ushort)(m_io_base + REG_IS), 0xFF);
        }

        private static void updateLinkStatus()
        {

            byte mediaState = PortIO.In8((ushort)(m_io_base + REG_MS));

            m_linkSpeed = ((mediaState & MS_SPEED_10) > 0) ? 10 : 100;
            m_connected = ((mediaState & MS_LINKDWN) > 0);
        }

        /// <summary>
        /// Exit handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void exitHandler(PCI.PciDevice dev)
        {
        }

        /// <summary>
        /// Initializes
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
