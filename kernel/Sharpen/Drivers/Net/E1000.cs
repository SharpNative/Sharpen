using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    class E1000
    {
        private const ushort MANUID_INTEL = 0x8086;
        private const ushort DEVID_EMU = 0x100E;
        private const ushort DEVID_I217 = 0x153A;
        private const ushort DEVID_82577LM = 0x10EA;

        private const ushort EEP_DONE = 0x04;

        private const ushort REG_EEPROM = 0x0014;

        private static byte[] m_mac;
        private static ushort m_io_base;
        private static ushort m_mem_base;
        private static bool m_has_epp = false;

        // RX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct RX_DESC
        {
            public ulong Address;
            public ushort Length;
            public ushort Checksum;
            public byte Status;
            public byte Errors;
            public ushort Special;
        }

        // TX_DESC
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TX_DESC
        {
            public ulong Address;
            public ushort Length;
            public byte CSO;
            public byte CMD;
            public byte STA;
            public byte CSS;

            public ushort Special;

        }


        private static void initHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port2;
            m_mem_base = dev.Port1; // 0?

            ushort cmd = PCI.PCIReadWord(dev, PCI.COMMAND);

            cmd |= 0x04;

            // Enable bus mastering
            PCI.PCIWrite(dev.Bus, dev.Slot, dev.Function, PCI.COMMAND, cmd);

            DetectEEP();
            ReadMac();

            for (int i = 0; i < 0x80; i++)
                WriteToDevice((ushort)(0x5200 + i * 4), 0);



            for (int i = 0; i < 6; i++)
                Console.WriteHex(m_mac[i]);
            Console.WriteLine("");

        }

        private static void exitHandler(PCI.PciDevice dev)
        {

        }

        private static unsafe  void WriteToDevice(ushort address, uint value)
        {
            // Set address
            PortIO.Out32(m_io_base, address);

            // Write to address
            PortIO.Out32((ushort)(m_io_base + 4), value);
        }

        private static unsafe uint ReadFromDevice(ushort address)
        {
            // Set address
            PortIO.Out32(m_io_base, address);

            // Read from address
            return PortIO.In32((ushort)(m_io_base + 4));
        }

        private static unsafe void ReadMac()
        {
            m_mac = new byte[6];

            // Do we have an EPP?
            if(m_has_epp)
            {
                uint tmp;
                tmp = EEPRead(0);
                m_mac[0] = (byte)(tmp & 0xff);
                m_mac[1] = (byte)(tmp >> 8);
                tmp = EEPRead(1);
                m_mac[2] = (byte)(tmp & 0xff);
                m_mac[3] = (byte)(tmp >> 8);
                tmp = EEPRead(2);
                m_mac[4] = (byte)(tmp & 0xff);
                m_mac[5] = (byte)(tmp >> 8);
                Console.WriteHex(tmp);
                for (;;) ;
            }
            else
            {
                byte* base_8 = (byte*)(m_mem_base + 0x5400);
                uint* base_32 = (uint*)(m_mem_base + 0x5400);

                if (*base_32 != 0)
                {
                    for (int i = 0; i < 6; i++)
                        m_mac[i] = base_8[i];
                    Console.WriteLine("YES");
                }
                else
                {
                    Console.WriteLine("Mayday, soldier down!");
                }
            }
        }

        private static bool DetectEEP()
        {
            uint val = 0;
            WriteToDevice(REG_EEPROM, 0x01);

            for (int i = 0; i < 1000 && !m_has_epp; i++)
            {
                val = ReadFromDevice(REG_EEPROM);
                if ((val & 0x10) > 0)
                    m_has_epp = true;
                else
                    m_has_epp = false;
            }

            if(!m_has_epp)
                Console.WriteLine("Oh dear, we're fucked");

            return m_has_epp;
        }

        private static uint EEPRead(byte adr)
        {
            ushort data = 0;
            uint tmp = 0;
            if(m_has_epp)
            {
                WriteToDevice(REG_EEPROM, (1) | ((uint)(adr) << 8));
                while (((tmp = ReadFromDevice(REG_EEPROM)) & EEP_DONE) == 0) ;
            }
            else
            {
                WriteToDevice(REG_EEPROM, (1) | ((uint)(adr) << 2));
                while (((tmp = ReadFromDevice(REG_EEPROM)) & EEP_DONE) == 0) ;
            }
            data = (ushort)((tmp >> 16) & 0xFFFF);
        
            return data;
        }

        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "E1000 Driver";
            driver.Exit = exitHandler;
            driver.Init = initHandler;

            PCI.RegisterDriver(MANUID_INTEL, DEVID_EMU, driver);
            PCI.RegisterDriver(MANUID_INTEL, DEVID_I217, driver);
            PCI.RegisterDriver(MANUID_INTEL, DEVID_82577LM, driver);
        }
    }
}
