using Sharpen.Arch;
using Sharpen.Mem;
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
        private const ushort REG_APROM = 0x00; // 00-0FH
        private const ushort REG_RDP = 0x10; // 10h - 13h
        private const ushort REG_RAP = 0x14; // 14h - 17h
        private const ushort REG_RESET = 0x18; // 18h - 1Bh
        private const ushort REG_BDP = 0x1C; // 1Ch - 1Fh3
        private const ushort EPROM = 0x00;
        private const ushort EPROM4 = 0x04;

        private static PCI.PciDevice m_dev;
        private static ushort m_io_base;
        private static byte[] m_mac;

        private static byte[] m_rx_buffer;
        private static byte[] m_tx_buffer;

        // RX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private unsafe struct CARD_REG
        {
            public ushort MODE;
            public byte RLEN;
            public byte TLEN;
            public fixed byte MAC[6];
            public fixed byte RES1[2];
            public fixed byte LADR[8];
            public uint first_rec_entry;
            public uint first_transmit_entry;
            public fixed byte JUNK[4];
        }

        private static void initHandler(PCI.PciDevice dev)
        {
            m_dev = dev;
            m_io_base = dev.Port1;

            FixCommand();

            // Do a software reset because we want 32bitjes :)
            SoftwareReset();

            // Read the current MAC
            ReadMac();
            writeCSR16(0, 0x04); // STOP

            // Initialize buffers
            InitBuffers();

            InitCard();
            Console.WriteLine("JA");

            // Enable card
            writeCSR32(0, 0x41);

            writeCSR32(4, 0x4C00 | ReadCSR32(4));

            writeCSR32(0, 0x0042);
        }

        private static void exitHandler(PCI.PciDevice dev)
        {

        }

        private static void SoftwareReset()
        {
            PortIO.In32((ushort)(m_io_base  + 0x18));
            PortIO.In16((ushort)(m_io_base + 0x14));

            PortIO.In32(0x80);

            PortIO.Out32((ushort)(m_io_base + 0x10), 0);

        }

        private static void InitBuffers()
        {
            m_rx_buffer = new byte[2048 * 12];
            m_tx_buffer = new byte[2048 * 12];
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

            uint tmp = PortIO.In32((ushort)(m_io_base + EPROM));
            m_mac[0] = (byte)((tmp) & 0xFF);
            m_mac[1] = (byte)((tmp >> 8) & 0xFF);
            m_mac[2] = (byte)((tmp >> 16) & 0xFF);
            m_mac[3] = (byte)((tmp >> 24) & 0xFF);
            tmp = PortIO.In32((ushort)(m_io_base + EPROM4));
            m_mac[4] = (byte)((tmp) & 0xFF);
            m_mac[5] = (byte)((tmp >> 8) & 0xFF);
        }

        private static void WriteRap32(uint val)
        {
            PortIO.Out32(REG_RAP, val);
        }

        private static void WriteRap16(ushort val)
        {
            PortIO.Out16(0x12, val);
        }

        private static uint ReadCSR32(uint val)
        {
            WriteRap32(val);
            return PortIO.In32((ushort)(m_io_base + REG_RDP));
        }

        private static void writeCSR32(uint csr_no, uint val)
        {
            WriteRap32(csr_no);
            PortIO.Out32((ushort)(m_io_base + REG_RDP), val);
        }
        
        private static unsafe void InitCard()
        {
            CARD_REG* reg = (CARD_REG*)Heap.Alloc(sizeof(CARD_REG));
            reg->MODE = 0x0180;
            reg->TLEN = 3;
            reg->RLEN = 3;
            for (int i = 0; i < 6; i++)
                reg->MAC[i] = m_mac[i];
            for (int i = 0; i < 8; i++)
                reg->LADR[i] = 0;
            reg->first_rec_entry = (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_rx_buffer));
            reg->first_transmit_entry = (uint)Paging.GetPhysicalFromVirtual(Util.ObjectToVoidPtr(m_tx_buffer));

            uint reg_adr = (uint)Paging.GetPhysicalFromVirtual(reg);
            writeCSR32(0x01, (ushort)reg_adr);
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
