using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Utilities;
using System.Runtime.InteropServices;

namespace Sharpen.Drivers.Net
{
    /// <summary>
    /// TODO: Fix magic values and documentation
    /// </summary>
    class PCNet2
    {
        private const ushort BUF_COUNT = 256;
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

        private const ushort RMD1_OWN = 0x8000;
        private const ushort RMD1_STP = 0x200;
        private const ushort RMD1_ENP = 0x100;


        private const ushort STATUS_MASK = 0x0FFF;


        private static PciDevice m_dev;
        private static ushort m_io_base;
        private static byte[] m_mac;

        private static REC_DESCRIPTOR[] m_rx_descriptors;
        private static TRANS_DESCRIPTOR[] m_tx_descriptors;
        private static byte[] m_tx_buffer;
        private static byte[] m_rx_buffer;
        private static int m_currentRescDesc = 0;
        private static int m_currentTransDesc = 0;

        private static byte[] buffer;

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
        private unsafe struct REC_DESCRIPTOR
        {
            public uint address;
            public ushort buf_len;
            public ushort status;
            public ushort mcnt;
            public byte rpc;
            public byte rcc;
            public uint reserved;
        }

        // TX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private unsafe struct TRANS_DESCRIPTOR
        {
            public uint address;
            public ushort buf_len;
            public ushort status;
            public uint misc;
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

            if(csr_no == 0)
                PortIO.Out32((ushort)(m_io_base + REG_RDP), val | 0x10);
            else
                PortIO.Out32((ushort)(m_io_base + REG_RDP), val);
        }

        private static uint readCSR(byte csr_no)
        {
            writeRap(csr_no);
            return PortIO.In32((ushort)(m_io_base + REG_RDP));
        }

        private static unsafe void initHandler(PciDevice dev)
        {
            buffer = new byte[2048];
            m_dev = dev;
            m_io_base = (ushort)(dev.BAR0.Address);

            Pci.EnableBusMastering(dev);
            Pci.EnableIOSpace(dev);
            
            /**
             * Read current mac
             */
            ReadMac();
            
            /**
             * Do a software reset because we want 32bitjes :)
             */
            SoftwareReset();

            writeCSR(0, 0x04);
            
            /**
             * Initalize buffers
             */
            InitBuffers();
            
            InitCard();

            /**
             * Set interrupt
             */
            Pci.SetInterruptHandler(dev, handler);
            

            // Enable card
            writeCSR(0, 0x41);


            writeCSR(4, 0x4C00 | readCSR(4));

            writeCSR(0, 0x42);


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
        /// Copies the MAC address to a buffer
        /// </summary>
        /// <param name="mac">The buffer</param>
        private static unsafe void GetMac(byte* mac)
        {
            for (int i = 0; i < 6; i++)
                mac[i] = m_mac[i];
        }

        private static unsafe void Transmit(byte* bytes, uint size)
        {
            if (!m_init)
                return;

            if (size > 2048)
                return;

            Memory.Memcpy((void*)m_tx_descriptors[m_currentTransDesc].reserved, bytes, (int)size);

            m_tx_descriptors[m_currentTransDesc].buf_len = (ushort)(0xF000 | (ushort)((-size) & 0xFFF));
            m_tx_descriptors[m_currentTransDesc].status = 0xA300;

            m_currentTransDesc++;
            if (m_currentTransDesc == BUF_COUNT)
                m_currentTransDesc = 0;
        }
        
        /// <summary>
        /// IRQ handler
        /// </summary>
        /// <returns>If we handled the IRQ</returns>
        private static unsafe bool handler()
        {
            // acknowledge IRQ
            uint csr0 = readCSR(0);

            if ((csr0 & CSR0_INTR) == 0)
                return false;

            writeCSR(0, csr0);

            if(!m_init && (csr0 & CSR0_IDON) != 0)
            {
                Console.WriteLine("[PCNET] Init done");

                m_init = true;
                return true;
            }

            // W00th?
            if (!m_init)
                return true;

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

                return true;
            }

            if((csr0 & CSR0_TINT) > 0)
            {
                // Transmitted ;)
            }

            if ((csr0 & CSR0_RINT) > 0)
            {
                while((m_rx_descriptors[m_currentRescDesc].status & RMD1_OWN) == 0)
                {
                    if ((m_rx_descriptors[m_currentRescDesc].status & RMD1_STP) == RMD1_STP &&
                         (m_rx_descriptors[m_currentRescDesc].status & RMD1_ENP) == RMD1_ENP
                        )
                    {
                        int offset = m_currentRescDesc * 2048;

                        Memory.Memcpy(Util.ObjectToVoidPtr(buffer), (byte *)Util.ObjectToVoidPtr(m_rx_buffer) + offset, 2048);


                        // TODO: REAL PACKET SIZE PLEASE!
                        Network.QueueReceivePacket(buffer, 2048);
                    }
                    
                    m_rx_descriptors[m_currentRescDesc].status = 0xF000 | (-2048 & STATUS_MASK);
                    m_rx_descriptors[m_currentRescDesc].mcnt = 0;
                    m_rx_descriptors[m_currentRescDesc].rcc = 0;
                    m_rx_descriptors[m_currentRescDesc].rpc = 0;

                    m_currentRescDesc++;
                    if (m_currentRescDesc == BUF_COUNT)
                        m_currentRescDesc = 0;
                }
            }

            return true;
        }

        private static void exitHandler(PciDevice dev)
        {
        }

        private static void SoftwareReset()
        {
            // RESET
            PortIO.In32((ushort)(m_io_base + 0x18));
            PortIO.In16((ushort)(m_io_base + 0x14));

            Sleep(5);

            // SET BCR
            PortIO.Out32((ushort)(m_io_base + REG_RDP), 0);

            // Enable 32bit :)
            writeBCR(20, 1);

            // sws style 2 please
            uint csr58 = readCSR(58);
            csr58 &= 0xFFF0;
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
            m_rx_descriptors = new REC_DESCRIPTOR[BUF_COUNT];
            m_tx_descriptors = new TRANS_DESCRIPTOR[BUF_COUNT];
            m_rx_buffer = new byte[2048 * BUF_COUNT];
            m_tx_buffer = new byte[2048 * BUF_COUNT];

            int rx_buf_adr = (int)Util.ObjectToVoidPtr(m_rx_buffer);
            int tx_buf_adr = (int)Util.ObjectToVoidPtr(m_tx_buffer);

            for (int i = 0; i < BUF_COUNT; i++)
            {
                m_rx_descriptors[i].address = (uint)Paging.GetPhysicalFromVirtual((void *)(rx_buf_adr + i * 2048));
                m_rx_descriptors[i].buf_len = 0xF000 | (-2048 & 0xFFF);
                m_rx_descriptors[i].status = 0x8000;
                m_rx_descriptors[i].mcnt = 0;
                m_rx_descriptors[i].rcc = 0;
                m_rx_descriptors[i].rpc = 0;
                m_rx_descriptors[i].reserved = (uint)(rx_buf_adr + i * 2048);
                
                m_tx_descriptors[i].address = (uint)Paging.GetPhysicalFromVirtual((void*)(tx_buf_adr + i * 2048));
                m_tx_descriptors[i].status = 0xF000;
                m_tx_descriptors[i].misc = 0;
                m_tx_descriptors[i].reserved = (uint)(tx_buf_adr + i * 2048);
            }

        }

        /// <summary>
        /// Reads the MAC address
        /// </summary>
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
        
        private static unsafe void InitCard()
        {
            // Card register needs to be 32-byte aligned
            CARD_REG* reg = (CARD_REG*)Heap.AlignedAlloc(0x1000, sizeof(CARD_REG));
            reg->MODE = 0x0180;
            // TLEN and RLEN are the 2log of their descriptor lengths shifted to the left by 4
            reg->TLEN = 8 << 4;
            reg->RLEN = 8 << 4;

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
            Pci.PciDriver driver = new Pci.PciDriver();
            driver.Name = "PC-NET 2 driver Driver";
            driver.Exit = exitHandler;
            driver.Init = initHandler;

            Pci.RegisterDriver(0x1022, 0x2000, driver);
        }
    }
}
