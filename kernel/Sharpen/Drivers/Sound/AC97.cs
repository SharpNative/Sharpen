using Sharpen.Arch;
using Sharpen.Lib;

namespace Sharpen.Drivers.Sound
{
    unsafe class AC97
    {
        private const int BDBAR = 0x10;
        private const int CIV = 0x14;
        private const int LVI = 0x15;
        private const int m_sr = 0x16;
        private const int PICB = 0x18;
        private const int CR = 0x1B;

        private const int CR_RPBM = (1 << 0); // Run or Pause
        private const int CR_PR = (1 << 1); // Reset registers
        private const int CR_LVBIE = (1 << 2); // Enable last valid buffer interrupt
        private const int CR_FEIE = (1 << 3); // Enable FIFO error interrupt
        private const int CR_IOCE = (1 << 4); // Enable interrupt on completion

        private const int RESET = 0x00;
        private const int MASTER_VOLUME = 0x02;
        private const int AUX_OUT_VALUE = 0x04;
        private const int MONO_VOLUME = 0x06;
        private const int PCM_OUT_VOLUME = 0x18;

        private const int CL_BUP = (1 << 30);
        private const int CL_IOC = (1 << 31);

        private const ushort SR_DCH = (1 << 0);
        private const ushort SR_CELV = (1 << 1);
        private const ushort SR_LVBCI = (1 << 2);
        private const ushort SR_BCIS = (1 << 3);
        private const ushort SR_FIFOE = (1 << 4);

        private static PciDevice m_dev;
        private static ushort m_nambar;
        private static ushort m_nabmbar;
        private static ushort m_lvi;
        
        private unsafe struct BDL_Entry
        {
            public void* pointer;
            public int cl;
        }

        private static ushort[][] m_bufs;
        private static BDL_Entry[] m_bdls;

        /// <summary>
        /// Driver initalization
        /// </summary>
        /// <param name="dev">PCI Device</param>
        private static unsafe void InitHandler(PciDevice dev)
        {
            
            m_dev = dev;
            m_nambar = (ushort)dev.BAR0.Address;
            m_nabmbar = (ushort)dev.BAR1.Address;

            // Set IRQ handler
            /*uint irqNum = PCI.PCIRead(dev.Bus, dev.Slot, dev.Function, 0x3C, 1);
            int intPIN = PCI.PCIReadWord(dev, 0x3C) >> 8;

            //IRQ.SetHandler((int)irqNum, IRQHandler);
            Console.Write("AC97(");
            Console.WriteNum(dev.Bus);
            Console.Write(',');
            Console.WriteNum(dev.Slot);
            Console.Write(")");*/

            /*int irq = 32+((dev.Slot << 2) | (intPIN - 1));
            Console.WriteNum(irq);
            Console.WriteLine("");
            IOApicManager.blar(11, (uint)irq);*/
            
            // Enable all interrupts
            PortIO.Out8((ushort)(m_nabmbar + CR), (byte)(CR_FEIE | CR_IOCE | CR_LVBIE));

            // Enable bus mastering
            PCI.PCIWrite(dev.Bus, dev.Slot, dev.Function, PCI.COMMAND, 0x05);

            // Volume
            ushort volume = 0x03 | (0x03 << 8);
            PortIO.Out16((ushort)(m_nambar + MASTER_VOLUME), volume);
            PortIO.Out16((ushort)(m_nambar + PCM_OUT_VOLUME), volume);

            // Buffers
            m_bdls = new BDL_Entry[32];
            m_bufs = new ushort[32][];
            for (int i = 0; i < 32; i++)
            {
                m_bufs[i] = new ushort[0x1000];
                fixed (void* ptr = m_bufs[i])
                {
                    m_bdls[i].pointer = Paging.GetPhysicalFromVirtual(ptr);
                }

                // Length and interrupt-on-clear
                m_bdls[i].cl = 0x1000 & 0xFFFF;
                m_bdls[i].cl |= CL_IOC;
            }

            // Tell BDL location
            fixed (void* ptr = m_bdls)
            {
                PortIO.Out32((ushort)(m_nabmbar + BDBAR), (uint)Paging.GetPhysicalFromVirtual(ptr));
            }

            // Set last valid index to 2
            PortIO.Out8((ushort)(m_nabmbar + LVI), 2);
            m_lvi = 2;

            // Set audio to playing
            PortIO.Out8((ushort)(m_nabmbar + CR), (byte)(PortIO.In8((ushort)(m_nabmbar + CR)) | CR_RPBM));

            Console.WriteLine("[AC97] Initalized");
        }

        /// <summary>
        /// IRQ Handler
        /// </summary>
        /// <param name="regsPtr">Register pointer</param>
        private static unsafe void IRQHandler(Regs* regsPtr)
        {
            ushort sr = PortIO.In16((ushort)(m_nabmbar + m_sr));

            if ((sr & SR_LVBCI) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_LVBCI);
            }
            else if ((sr & SR_BCIS) > 0)
            {
                int tmp = m_lvi + 2;
                uint start = (uint)(tmp & (32 - 1));

                // Fill buffer
                for (int i = 0; i < 0x1000 * 4; i += 128)
                {
                    ushort* shr;
                    fixed (void* ptr = m_bufs[start])
                    {
                        shr = (ushort*)((int)ptr + i);
                    }

                    Audio.RequestBuffer(128, shr);
                }

                tmp = m_lvi + 1;
                m_lvi = (ushort)((tmp) % 32);

                PortIO.Out8((ushort)(m_nabmbar + LVI), (byte)m_lvi);
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_BCIS);
            }
            else if ((sr & SR_FIFOE) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + m_sr), SR_FIFOE);
            }
        }

        /// <summary>
        /// Called when the exit is called on the PCI
        /// </summary>
        /// <param name="dev">Reference to the device</param>
        private static void ExitHander(PciDevice dev)
        {

        }

        /// <summary>
        /// Registers the AC97 driver
        /// </summary>
        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "AC97 Driver";
            driver.Exit = ExitHander;
            driver.Init = InitHandler;

            PCI.RegisterDriver(0x8086, 0x2415, driver);

            Audio.SoundDevice device = new Audio.SoundDevice();
            device.Name = "AC97 audio device";
            device.Writer = Writer;
            device.Reader = Reader;

            Audio.SetDevice(device);
        }

        /// <summary>
        /// Reading operation
        /// </summary>
        /// <param name="action">The audio action</param>
        /// <returns>The read value</returns>
        private static uint Reader(AudioActions action)
        {
            return 0;
        }

        /// <summary>
        /// Writing operation
        /// </summary>
        /// <param name="action">The audio action</param>
        /// <param name="value">The value to write</param>
        private static void Writer(AudioActions action, uint value)
        {
            if (action == AudioActions.Master)
            {
                value = ~value;

                // It's a 6bit value!
                value >>= 26;

                ushort encoded = (ushort)(value | (value << 8));
                PortIO.Out16((ushort)(m_nambar + MASTER_VOLUME), encoded);
            }
            else if (action == AudioActions.PCM_OUT)
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
