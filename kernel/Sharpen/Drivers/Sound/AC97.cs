using Sharpen.Arch;
using Sharpen.FileSystem;

namespace Sharpen.Drivers.Sound
{
    unsafe class AC97
    {
        private const int REG_BDBAR = 0x10;
        private const int REG_CIV = 0x14;
        private const int REG_LVI = 0x15;
        private const int REG_SR = 0x16;
        private const int REG_PICB = 0x18;
        private const int REG_CR = 0x1B;

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

        private const int BDL_COUNT = 32;

        private unsafe struct BDL_Entry
        {
            public void* pointer;
            public int cl;
        }

        private static ushort[][] m_bufs;
        private static BDL_Entry[] m_bdls;

        /// <summary>
        /// Driver initialization
        /// </summary>
        /// <param name="dev">PCI Device</param>
        private static void initHandler(PciDevice dev)
        {
            m_dev = dev;
            m_nambar = (ushort)dev.BAR0.Address;
            m_nabmbar = (ushort)dev.BAR1.Address;

            // Set IRQ handler and bus mastering and I/O space
            Pci.SetInterruptHandler(dev, handler);
            Pci.EnableBusMastering(dev);
            Pci.EnableIOSpace(dev);

            // Enable all interrupts
            PortIO.Out8((ushort)(m_nabmbar + REG_CR), (CR_FEIE | CR_IOCE | CR_LVBIE));
            
            // Volume
            ushort volume = 0x03 | (0x03 << 8);
            PortIO.Out16((ushort)(m_nambar + MASTER_VOLUME), volume);
            PortIO.Out16((ushort)(m_nambar + PCM_OUT_VOLUME), volume);

            // Buffers
            m_bdls = new BDL_Entry[BDL_COUNT];
            m_bufs = new ushort[BDL_COUNT][];
            for (int i = 0; i < BDL_COUNT; i++)
            {
                m_bufs[i] = new ushort[AudioFS.BufferSize];
                fixed (void* ptr = m_bufs[i])
                {
                    m_bdls[i].pointer = Paging.GetPhysicalFromVirtual(ptr);
                }

                // Length and interrupt-on-clear
                m_bdls[i].cl = AudioFS.BufferSize & 0xFFFF;
                m_bdls[i].cl |= CL_IOC;
            }

            // Tell BDL location
            fixed (void* ptr = m_bdls)
            {
                PortIO.Out32((ushort)(m_nabmbar + REG_BDBAR), (uint)Paging.GetPhysicalFromVirtual(ptr));
            }

            // Set last valid index
            m_lvi = 3;
            PortIO.Out8((ushort)(m_nabmbar + REG_LVI), (byte)m_lvi);

            // Set audio to playing
            PortIO.Out8((ushort)(m_nabmbar + REG_CR), (byte)(PortIO.In8((ushort)(m_nabmbar + REG_CR)) | CR_RPBM));

            Console.WriteLine("[AC97] Initialized");
        }

        /// <summary>
        /// IRQ Handler
        /// </summary>
        /// <returns>If we handled the irq</returns>
        private static bool handler()
        {
            ushort sr = PortIO.In16((ushort)(m_nabmbar + REG_SR));
            
            if ((sr & SR_LVBCI) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + REG_SR), SR_LVBCI);
            }
            else if ((sr & SR_BCIS) > 0)
            {
                // Load next one already
                int next = m_lvi + 2;
                if (next >= BDL_COUNT)
                    next -= BDL_COUNT;
                
                AudioFS.RequestBuffer(AudioFS.BufferSize, m_bufs[next]);

                // Set current one
                m_lvi++;
                if (m_lvi == BDL_COUNT)
                    m_lvi = 0;
                
                PortIO.Out8((ushort)(m_nabmbar + REG_LVI), (byte)m_lvi);
                PortIO.Out16((ushort)(m_nabmbar + REG_SR), SR_BCIS);
            }
            else if ((sr & SR_FIFOE) > 0)
            {
                PortIO.Out16((ushort)(m_nabmbar + REG_SR), SR_FIFOE);
            }
            else
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Called when the exit is called on the PCI
        /// </summary>
        /// <param name="dev">Reference to the device</param>
        private static void exitHander(PciDevice dev)
        {
        }

        /// <summary>
        /// Registers the AC97 driver
        /// </summary>
        public static void Init()
        {
            Pci.PciDriver driver = new Pci.PciDriver();
            driver.Name = "AC97 Driver";
            driver.Exit = exitHander;
            driver.Init = initHandler;

            Pci.RegisterDriver(0x8086, 0x2415, driver);

            // TODO
            AudioFS.SoundDevice device = new AudioFS.SoundDevice();
            device.Name = "AC97 audio device";
            device.Writer = Writer;
            device.Reader = Reader;

            AudioFS.SetDevice(device);
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
