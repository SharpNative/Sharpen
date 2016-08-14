using Sharpen.Arch;
using Sharpen.Net;
using Sharpen.Utilities;
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
        private static readonly ushort REG_BUF = 0x30;
        private static readonly ushort REG_CMD = 0x37;
        private static readonly ushort REG_IM = 0x3C;
        private static readonly ushort REG_IS = 0x3E;
        private static readonly ushort REG_TC = 0x40;
        private static readonly ushort REG_RC = 0x44;
        private static readonly ushort REG_MS = 0x58;
        private static readonly ushort REG_TSAD0 = 0x20;
        private static readonly ushort REG_TSAD1 = 0x24;
        private static readonly ushort REG_TSAD2 = 0x28;
        private static readonly ushort REG_TSAD3 = 0x2C;
        private static readonly ushort REG_TSD0 = 0x10;
        private static readonly ushort REG_TSD1 = 0x14;
        private static readonly ushort REG_TSD2 = 0x18;
        private static readonly ushort REG_TSD3 = 0x1C;


        private static readonly ushort CMD_RXEMPTY = 0x01;
        private static readonly ushort CMD_TXE = 0x04;
        private static readonly ushort CMD_RXE = 0x08;
        private static readonly ushort CMD_RST = 0x10;

        private static readonly byte MS_LINKB = 0x04;
        private static readonly byte MS_SPEED_10 = 0x08;

        private static byte[] m_mac;

        private static ushort m_io_base;
        private static int m_linkSpeed;
        private static bool m_linkFail = true;
        private static int m_irqNum;
        private static int m_curBuffer = 0;

        private static byte[] m_buffer;
        private static byte[] m_transmit0;
        private static byte[] m_transmit1;
        private static byte[] m_transmit2;
        private static byte[] m_transmit3;
        
        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static unsafe void initHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port1;
            m_mac = new byte[6];
            m_buffer = new byte[8192 + 16];
            m_transmit0 = new byte[8192 + 16];
            m_transmit1 = new byte[8192 + 16];
            m_transmit2 = new byte[8192 + 16];
            m_transmit3 = new byte[8192 + 16];



            m_irqNum = PCI.PCIReadWord(dev, 0x3C) & 0xFF;

            // Enable bus mastering
            PCI.PCIWrite(dev.Bus, dev.Slot, dev.Function, PCI.COMMAND, 0x05);

            // Turn device on
            turnOn();

            // Reset
            PortIO.Out8((ushort)(m_io_base + REG_CMD), (byte)CMD_RST);

            // Wait till done resettings :D
            while ((PortIO.In8((ushort)(m_io_base + REG_CMD)) & CMD_RST) != 0) { }
            
            // Set receive buffer
            void* inAdr = Util.ObjectToVoidPtr(m_buffer);
            uint adr = (uint)Paging.GetPhysicalFromVirtual(inAdr);

            PortIO.Out32((ushort)(m_io_base + REG_BUF), 0xFFFF);

            // SET IMR + ISR
            setInterruptMask(0x0005);

            // RCR
            PortIO.Out32((ushort)(m_io_base + REG_RC), 0xf | (1 << 7));

            // Enable receive and Transmit
            PortIO.Out8((ushort)(m_io_base + REG_CMD), (byte)(CMD_TXE | CMD_RXE));

            updateLinkStatus();

            IRQ.SetHandler(11, handler);

            inAdr = Util.ObjectToVoidPtr(m_transmit0);
            adr = (uint)Paging.GetPhysicalFromVirtual(inAdr);

            PortIO.Out32((ushort)(m_io_base + REG_TSAD0), adr);

            inAdr = Util.ObjectToVoidPtr(m_transmit1);
            adr = (uint)Paging.GetPhysicalFromVirtual(inAdr);

            PortIO.Out32((ushort)(m_io_base + REG_TSAD1), adr);

            inAdr = Util.ObjectToVoidPtr(m_transmit2);
            adr = (uint)Paging.GetPhysicalFromVirtual(inAdr);

            PortIO.Out32((ushort)(m_io_base + REG_TSAD2), adr);


            inAdr = Util.ObjectToVoidPtr(m_transmit3);
            adr = (uint)Paging.GetPhysicalFromVirtual(inAdr);

            PortIO.Out32((ushort)(m_io_base + REG_TSAD3), adr);
            
            readMac();

            // Register device as the main network device
            Network.NetDevice netDev = new Network.NetDevice();
            netDev.ID = dev.Device;
            netDev.Transmit = Transmit;
            netDev.GetMac = GetMac;

            Network.Set(netDev);
        }

        private static unsafe void GetMac(byte* mac)
        {
            for (int i = 0; i < 6; i++)
                mac[i] = m_mac[i];
        }

        public static unsafe void Transmit(byte *bytes, uint size)
        {
            byte *dataAddr = (byte *)Util.ObjectToVoidPtr(m_transmit0);
            ushort portAddress = (ushort)(m_io_base + REG_TSD0);

            if(m_curBuffer == 0)
            {
                m_curBuffer++;
            }
            else if(m_curBuffer == 1)
            {
                dataAddr = (byte*)Util.ObjectToVoidPtr(m_transmit1);
                portAddress = (ushort)(m_io_base + REG_TSD1);
                m_curBuffer++;
            }
            else if (m_curBuffer == 2)
            {
                dataAddr = (byte*)Util.ObjectToVoidPtr(m_transmit2);
                portAddress = (ushort)(m_io_base + REG_TSD2);
                m_curBuffer++;
            }
            else if (m_curBuffer == 3)
            {
                dataAddr = (byte*)Util.ObjectToVoidPtr(m_transmit3);
                portAddress = (ushort)(m_io_base + REG_TSD3);
                m_curBuffer = 0;
            }

            // Clear transmit buffer
            Memory.Memset(dataAddr, 0x00, 8192 + 16);
            Memory.Memcpy(dataAddr, bytes, (int)size);

            PortIO.Out32(portAddress, size);

        }

        private static void PrintRes()
        {

            Console.WriteLine("");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("RTL8139 registers");
            Console.WriteLine("");
            Console.Write("MAC: ");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteHex(m_mac[i]);
                Console.PutChar(' ');
            }
            Console.WriteLine("");
            Console.Write("Linkspeed: ");
            Console.WriteNum(m_linkSpeed);
            Console.WriteLine("");
            Console.Write("Linkstate: ");
            Console.WriteLine((m_linkFail) ? "FAIL" : "OK");
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
            m_linkFail = ((mediaState & MS_LINKB) > 0);
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
