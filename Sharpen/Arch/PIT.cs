namespace Sharpen.Arch
{
    public sealed class PIT
    {
        /// <summary>
        /// Timer frequency in Hz
        /// </summary>
        public static int Frequency
        {
            get
            {
                return m_frequency;
            }

            set
            {
                m_frequency = value;

                // Send command to change
                PortIO.Out8(PIT_CMD, (byte)(Channel(0) | Access(3) | Operating(3) | Mode(0)));

                // Send divisor
                ushort divisor = (ushort)(PIT_OSCILLATOR / value);
                PortIO.Out8(PIT_DATA, (byte)(divisor & 0xFF));
                PortIO.Out8(PIT_DATA, (byte)((divisor >> 8) & 0xFF));
            }
        }

        // In MHz
        public static readonly int PIT_OSCILLATOR = 1193182;

        // PIT channel 0 data port
        // We use channel 0 because it is linked to IRQ 0
        public static readonly ushort PIT_DATA = 0x40;

        // PIT mode / command port
        public static readonly ushort PIT_CMD = 0x43;

        // Frequency in Hz
        private static int m_frequency;

        // Timer ticks
        private static int m_subTicks = 0;
        private static int m_fullTicks = 0;

        #region Helpers

        /// <summary>
        /// Used to indicate the channel
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Channel(int a)
        {
            // 0 -> channel 0
            // 1 -> channel 1
            // 2 -> channel 2
            // 3 -> readback command
            return (a << 0x6);
        }

        /// <summary>
        /// Access mode
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Access(int a)
        {
            // 0 -> Latch count value command
            // 1 -> lowbyte only
            // 2 -> highbyte only
            // 3 -> lowbyte / highbyte
            return (a << 0x4);
        }

        /// <summary>
        /// Operating mode
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Operating(int a)
        {
            // 0 -> Interrupt on terminal count
            // 1 -> Hardware retriggerable oneshot
            // 2 -> Rate generator
            // 3 -> Square wave generator
            // 4 -> Software triggered strobe
            // 5 -> Hardware triggered strobe
            // 6 -> Mode 2 (Rate generator)
            // 7 -> Mode 3 (Square wave generator)
            return (a << 0x1);
        }

        /// <summary>
        /// BCD / Binary mode
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Mode(int a)
        {
            // 0 -> 16bit binary
            // 1 -> fourdigit BCD
            return a;
        }

        #endregion

        /// <summary>
        /// Initializes the PIT
        /// </summary>
        public static unsafe void Init()
        {
            // Set to 100Hz
            Frequency = 100;

            // Install the IRQ handler
            IRQ.SetHandler(0, Handler);
        }

        /// <summary>
        /// PIT handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(Regs* regsPtr)
        {
            // Update ticks
            m_subTicks++;

            // One second
            if (m_subTicks == m_frequency)
            {
                // Update ticks
                m_subTicks = 0;
                m_fullTicks++;

                // Re-read the CMOS time every hour
                Time.Seconds++;
                if (Time.Seconds == 60)
                {
                    Time.Seconds = 0;
                    Time.Minutes++;

                    if (Time.Minutes == 60)
                    {
                        // Resync with CMOS
                        CMOS.UpdateTime();
                    }
                }

                Console.Write("It is ");
                Console.WriteNum(Time.Hours);
                Console.Write(":");
                Console.WriteNum(Time.Minutes);
                Console.Write(":");
                Console.WriteNum(Time.Seconds);
                Console.PutChar('\n');
            }
        }
    }
}
