using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    class PCNet2
    {
        private const ushort REG_APROM = 0x00;
        private const ushort REG_RDP = 0x10;
        private const ushort REG_RAP = 0x14;
        private const ushort REG_RESET = 0x18;
        private const ushort REG_BDP = 0x1C;

        private const ushort EPROM = 0x00;
        private const ushort EPROM2 = 0x02;
        private const ushort EPROM4 = 0x04;

        private const ushort CSR0_INTR = 0x40;
        private const ushort CSR0_IDON = 0x100;
        private const ushort CSR0_ERR = 0x8000;
        private const ushort CSR0_CERR = 0x2000;
        private const ushort CSR0_MISS = 0x1000;
        private const ushort CSR0_MERR = 0x800;
        private const ushort CSR0_TINT = 0x200;
        private const ushort CSR0_RINT = 0x400;

        private static PCI.PciDevice m_dev;
        private static ushort m_io_base;
        private static byte[] m_mac;

        private static DESCRIPTOR[] m_rx_descriptors;
        private static DESCRIPTOR[] m_tx_descriptors;
        private static byte[] m_tx_buffer;
        private static byte[] m_rx_buffer;
        private static int m_currentRescDesc = 0;
        private static int m_currentTransDesc = 0;

        private static bool m_init = false;

        // RX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private unsafe struct CARD_REG
        {
            public ushort MODE;
            public byte RLEN;
            public byte TLEN;
            public fixed byte MAC[6];
            public ushort _res;
            public fixed ushort RES1[4];
            public uint first_rec_entry;
            public uint first_transmit_entry;
            public fixed ushort RES2[1];
        }

        // RX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private unsafe struct DESCRIPTOR
        {
            public uint address;
            public ushort buf_len;
            public ushort status;
            public uint flags2;
            public uint reserved;
        }


        private static void writeBCR(ushort bcr, ushort val)
        {
            writeRap(bcr);
            PortIO.Out32((ushort)(m_io_base + REG_BDP), val);
        }


        private static void writeCSR(byte csr_no, uint val)
        {
            writeRap(csr_no);
            PortIO.Out32((ushort)(m_io_base + REG_RDP), val);
        }

        private static uint readCSR(byte csr_no)
        {
            writeRap(csr_no);
            return PortIO.In32((ushort)(m_io_base + REG_RDP));
        }

        private static unsafe void initHandler(PCI.PciDevice dev)
        {
            m_dev = dev;
            m_io_base = dev.Port1;
            FixCommand();

            ReadMac();

            // Do a software reset because we want 32bitjes :)
            SoftwareReset();

            // Read the current MAC
            writeCSR(0, 0x04); // STOP

            // Initialize buffers
            InitBuffers();

            InitCard();

            int interrupt = (PCI.PCIReadWord(dev, 0x3C) & 0xFF);

            IRQ.SetHandler(interrupt, handler);


            // Enable card
            writeCSR(0, 0x41);


            writeCSR(4, 0x4C00 | readCSR(4));
            writeCSR(0, 0x0042);



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

        private static unsafe void Transmit(byte* bytes, uint size)
        {
            if (!m_init)
                return;
            

            m_currentTransDesc++;
            if (m_currentTransDesc == 8)
                m_currentTransDesc = 0;
        }

        private static unsafe void handler(Regs* regsPtr)
        {
            Console.WriteLine("[PCNET] IRQ!");
            Console.WriteHex(m_rx_descriptors[0].status);

            // acknowledge IRQ
            uint csr0 = readCSR(0);

            if ((csr0 & CSR0_INTR) == 0)
                return;

            writeCSR(0, csr0);

            if(!m_init && (csr0 & CSR0_IDON) != 0)
            {
                Console.WriteLine("[PCNET] Init done");

                m_init = true;
                return;
            }

            // W00th?
            if (!m_init)
                return;

            
            Console.WriteHex(m_rx_descriptors[m_currentRescDesc].status);
            Console.WriteLine("");

            if ((csr0 & CSR0_ERR) != 0)
            {
                if ((csr0 & CSR0_MISS) != 0)
                    Console.WriteLine("[PCNET] Error: MISS");
                else if ((csr0 & CSR0_MERR) != 0)
                    Console.WriteLine("[PCNET] Error: MERR");
                else if ((csr0 & CSR0_CERR) != 0)
                    Console.WriteLine("[PCNET] Error: CERR");
                else
                    Console.WriteLine("[PCNET] Error: HELP :(");

                return;
            }

            if((csr0 & CSR0_RINT) > 0)
            {
                while((m_rx_descriptors[1].status & 0x8000) == 0)
                {
                    if((m_rx_descriptors[1].status & 0x4000) == 0 &&
                         (m_rx_descriptors[1].status & 0x3000) == 0x0300
                        )
                    {
                        // Receive :)
                    }

                    m_rx_descriptors[m_currentRescDesc].status = 0x8000;
                    m_rx_descriptors[m_currentRescDesc].buf_len = 0xF000 | (-2048 & 0xFFF);
                    m_rx_descriptors[m_currentRescDesc].flags2 = 0;
                    
                    m_currentRescDesc++;
                    if (m_currentRescDesc == 8)
                        m_currentRescDesc = 0;
                }
            }
        }

        private static void exitHandler(PCI.PciDevice dev)
        {

        }

        private static void SoftwareReset()
        {
            /// RESET
            PortIO.In32((ushort)(m_io_base + 0x18));
            PortIO.In16((ushort)(m_io_base + 0x14));

            Sleep(5);

            // SET BCR
            PortIO.Out32((ushort)(m_io_base + REG_RDP), 0);

            // Enable 32bit :)
            writeBCR(20, 1);

            // sws style 2 please
            uint csr58 = readCSR(58);
            csr58 &= 0xfff0;
            csr58 |= 2;
            writeCSR(58, csr58);
        }

        private static void Sleep(int cnt)
        {
            for(int  i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }

        private static unsafe void InitBuffers()
        {
            m_rx_descriptors = new DESCRIPTOR[8];
            m_tx_descriptors = new DESCRIPTOR[8];
            m_rx_buffer = new byte[2048 * 8];
            m_tx_buffer = new byte[2048 * 8];

            int rx_buf_adr = (int)Util.ObjectToVoidPtr(m_rx_buffer);
            int tx_buf_adr = (int)Util.ObjectToVoidPtr(m_tx_buffer);

            for (int i = 0; i < 8; i++)
            {
                m_rx_descriptors[i].address = (ushort)Paging.GetPhysicalFromVirtual((void *)(rx_buf_adr + i * 2048));
                m_rx_descriptors[i].buf_len = 0xF000 | (-2048 & 0xFFF);
                m_rx_descriptors[i].status = 0x8000;
                m_rx_descriptors[i].flags2 = 0;
                m_rx_descriptors[i].reserved = (uint)(rx_buf_adr + i * 2048);
                
                m_tx_descriptors[i].address = (ushort)Paging.GetPhysicalFromVirtual((void*)(tx_buf_adr + i * 2048));
                m_tx_descriptors[i].status = 0xF000;
                m_tx_descriptors[i].flags2 = 0;
                m_tx_descriptors[i].reserved = (uint)(tx_buf_adr + i * 2048);
            }

        }

        private static void FixCommand()
        {

            ushort cmd = PCI.PCIReadWord(m_dev, PCI.COMMAND);
            cmd |= 0x05; // set bits 0 and 2

            PCI.PCIWrite(m_dev.Bus, m_dev.Slot, m_dev.Function, PCI.COMMAND, cmd);
        }

        private static void ReadMac()
        {
            m_mac = new byte[6];

            uint tmp = PortIO.In16((ushort)(m_io_base + EPROM));
            m_mac[0] = (byte)((tmp) & 0xFF);
            m_mac[1] = (byte)((tmp >> 8) & 0xFF);
            tmp = PortIO.In16((ushort)(m_io_base + EPROM2));
            m_mac[2] = (byte)((tmp) & 0xFF);
            m_mac[3] = (byte)((tmp >> 8) & 0xFF);
            tmp = PortIO.In16((ushort)(m_io_base + EPROM4));
            m_mac[4] = (byte)((tmp) & 0xFF);
            m_mac[5] = (byte)((tmp >> 8) & 0xFF);
        }
        
        private static void writeRap(ushort val)
        {
            PortIO.Out32((ushort)(m_io_base + REG_RAP), val);
        }

        private static unsafe uint switchShorts(uint address)
        {
            ushort top = (ushort)((address >> 16) & 0xFFFF);
            ushort bottom = (ushort)((address) & 0xFFFF);
            return (uint)(bottom << 16 | top);
        }

        private static unsafe void InitCard()
        {
            CARD_REG* reg = (CARD_REG*)Heap.AlignedAlloc(0x1000,sizeof(CARD_REG));
            reg->MODE = 0x0180;
            reg->TLEN = 3;
            reg->RLEN = 3;
            for (int i = 0; i < 6; i++)
                reg->MAC[i] = m_mac[i];
            reg->first_rec_entry = (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_rx_descriptors));
            reg->first_transmit_entry = (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_tx_descriptors));


            uint reg_adr = (uint)Paging.GetPhysicalFromVirtual(reg);
            writeCSR(0x01, (ushort)(reg_adr & 0xFFFF));
            writeCSR(0x02, (ushort)((reg_adr >> 16) & 0xFFFF));
        }

        /// <summary>
        /// Initializes
        /// </summary>
        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "PC-NET 2 driver Driver";
            driver.Exit = exitHandler;
            driver.Init = initHandler;

            PCI.RegisterDriver(0x1022, 0x2000, driver);
        }
    }
}
