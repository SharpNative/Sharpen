using Sharpen.Arch;
using Sharpen.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Sound
{
    unsafe class AC97
    {
        private static readonly int BDBAR = 0x10;
        private static readonly int CIV = 0x14;
        private static readonly int LVI = 0x15;
        private static readonly int m_sr = 0x16;
        private static readonly int PICB = 0x18;
        private static readonly int CR = 0x1B;

        private static readonly int CR_RPBM = (1 << 0); // Run or Pause
        private static readonly int CR_PR = (1 << 1); // Reset registers
        private static readonly int CR_LVBIE = (1 << 2); // Enable last valid buffer interrupt
        private static readonly int CR_FEIE = (1 << 3); // Enable FIFO error interrupt
        private static readonly int CR_IOCE = (1 << 4); // Enable interrupt on completion

        private static readonly int RESET = 0x00;
        private static readonly int MASTER_VOLUME = 0x02;
        private static readonly int AUX_OUT_VALUE = 0x04;
        private static readonly int MONO_VOLUME = 0x06;
        private static readonly int PCM_OUT_VOLUME = 0x18;

        private static readonly int CL_BUP = (1 << 30);
        private static readonly int CL_IOC = (1 << 31);

        private static readonly ushort SR_DCH = (1 << 0);
        private static readonly ushort SR_CELV = (1 << 1);
        private static readonly ushort SR_LVBCI = (1 << 2);
        private static readonly ushort SR_BCIS = (1 << 3);
        private static readonly ushort SR_FIFOE = (1 << 4);

        private static PCI.PciDevice m_dev;
        private static ushort m_nambar;
        private static ushort m_nabmbar;
        private static ushort m_lvi;

        
        private struct BDL_Entry
        {
            public uint pointer;
            public int cl;
        }

        private static ushort** m_bufs;
        private static BDL_Entry[] m_bdls = new BDL_Entry[32];

        /// <summary>
        /// Driver initalization
        /// </summary>
        /// <param name="dev">PCI Device</param>
        private static unsafe void InitHandler(PCI.PciDevice dev)
        {
            m_dev = dev;

            m_nambar = dev.Port1;
            m_nabmbar = dev.Port2;

            // "Map" IRQ
            uint irqNum = PCI.PciRead(dev.Bus, dev.Slot, dev.Function, 0x3C, 1);
            IRQ.SetHandler((int)irqNum, irqHandler);
            
            // Enable all interrupts
            PortIO.Out8((ushort)(m_nabmbar + CR), (byte)(CR_FEIE | CR_IOCE | CR_LVBIE));

            // Enable bus mastering
            PCI.PciWrite(dev.Bus, dev.Slot, dev.Function, PCI.COMMAND, 0x05);

            ushort volume = 0x03 | (0x03 << 8);
            PortIO.Out16((ushort)(m_nambar + MASTER_VOLUME), volume);
            PortIO.Out16((ushort)(m_nambar + PCM_OUT_VOLUME), volume);

            m_bufs = (ushort**)Heap.Alloc(sizeof(ushort*) * 32);

            for(int i = 0; i < 32; i++)
            {
                m_bufs[i] = (ushort *)Heap.Alloc(sizeof(ushort) * 0x1000);

                m_bdls[i].pointer = (uint)m_bufs[i];
                m_bdls[i].cl = 0x1000 & 0xFFFF;

                m_bdls[i].cl |= CL_IOC;
            }

            // Tell BDL location
            PortIO.Out32((ushort)(m_nabmbar + BDBAR), (uint)Util.ObjectToVoidPtr(m_bdls));

            // Set last valid index to 2
            PortIO.Out8((ushort)(m_nabmbar + LVI), 2);
            m_lvi = 2;

            // Audio on
            PortIO.Out8((ushort)(m_nabmbar + CR), (byte)(PortIO.In8((ushort)(m_nabmbar + CR)) | CR_RPBM));
            
            Console.WriteLine("[AC97] Initalized");
        }

        /// <summary>
        /// IRQ Handler
        /// </summary>
        /// <param name="regsPtr">Register pointer</param>
        private static unsafe void irqHandler(Regs* regsPtr)
        {
            ushort sr = PortIO.In16((ushort)(m_nabmbar + m_sr));

            if((sr & SR_LVBCI) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_LVBCI);
            }
            else if ((sr & SR_BCIS) > 0)
            {
                int tmp = m_lvi + 2;
                uint start = (uint)((tmp) % 32);
                // Fill buffer ;
                for(int i = 0; i < 0x1000 * 4; i+= 128)
                {
                    ushort * shr = m_bufs[start] + i;

                    Audio.RequestBuffer(128, shr);
                }

                tmp = m_lvi + 1;
                m_lvi = (ushort)((tmp) % 32);

                PortIO.Out8((ushort)(m_nabmbar + LVI), (byte)m_lvi);
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_BCIS);
            }
            else if((sr & SR_FIFOE) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_FIFOE);
            }
        }

        private static void ExitHander(PCI.PciDevice dev)
        {

        }
        
        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "AC97 Driver";
            driver.Exit = ExitHander;
            driver.Init = InitHandler;

            PCI.RegisterDriver(0x8086, 0x2415, driver);

            Audio.SoundDevice device = new Audio.SoundDevice();
            device.Name = "AC97 audio device";
            device.Writer = writer;
            device.Reader = reader;

            Audio.SetDevice(device);
        }

        private static uint reader(AudioActions action)
        {
            return 0;
        }

        private static void writer(AudioActions action, uint value)
        {
            if(action == AudioActions.Master)
            {
                value = ~value;

                // It's a 6bit value!
                value >>= 26;

                ushort encoded = (ushort)(value | (value << 8));
                PortIO.Out16((ushort)(m_nambar + MASTER_VOLUME), encoded);
            }
            else if(action == AudioActions.PCM_OUT)
            {
                value = ~value;

                // It's a 5 bit value!
                value >>= 27;

                ushort encoded = (ushort)(value | (value << 8));
                PortIO.Out16((ushort)(m_nambar + PCM_OUT_VOLUME), encoded);
            }
        }
    }
}
