using Sharpen.MultiTasking;
using Sharpen.Utilities;

namespace Sharpen.Arch
{
    unsafe class LocalApic
    {
        // Local APIC registers
        public const int LAPIC_ID = 0x20;
        public const int LAPIC_VERSION = 0x30;
        public const int LAPIC_TASK_PRIO = 0x80;
        public const int LAPIC_ARB_PRIO = 0x90;
        public const int LAPIC_CPU_PRIO = 0xA0;
        public const int LAPIC_EOI = 0xB0;
        public const int LAPIC_LOGICAL_DEST = 0xD0;
        public const int LAPIC_DEST_FORMAT = 0xE0;
        public const int LAPIC_SPURIOUS = 0xF0;
        public const int LAPIC_ERROR_STAT = 0x280;
        public const int LAPIC_INT_CMD_LO = 0x300;
        public const int LAPIC_INT_CMD_HI = 0x310;
        public const int LAPIC_LVT_TIMER = 0x320;
        public const int LAPIC_LVT_THERMAL = 0x330;
        public const int LAPIC_LVT_PERFORMANCE = 0x340;
        public const int LAPIC_LVT_LINT0 = 0x350;
        public const int LAPIC_LVT_LINT1 = 0x360;
        public const int LAPIC_LVT_ERROR = 0x370;

        // APIC timer
        public const int LAPIC_TIMER_INIT_COUNT = 0x380;
        public const int LAPIC_TIMER_CURRENT_COUNT = 0x390;
        public const int LAPIC_TIMER_DIVISOR = 0x3E0;
        public const int LAPIC_TIMER_MODE_ONESHOT = (0 << 17);
        public const int LAPIC_TIMER_MODE_PERIODIC = (1 << 17);

        // Delivery modes
        public const int LAPIC_LVT_DELIVERY_FIXED = (0 << 8);
        public const int LAPIC_LVT_DELIVERY_SMI = (2 << 8);
        public const int LAPIC_LVT_DELIVERY_NMI = (4 << 8);
        public const int LAPIC_LVT_DELIVERY_INT = (5 << 8);
        public const int LAPIC_LVT_DELIVERY_EXT_INT = (7 << 8);

        public const int LAPIC_LVT_INT_UNMASKED = (0 << 16);
        public const int LAPIC_LVT_INT_MASKED = (1 << 16);

        public const int LAPIC_SPURIOUS_ENABLE = (1 << 8);

        private static void* m_address;

        /// <summary>
        /// Sets the physical address of the local APIC
        /// </summary>
        /// <param name="address">The physical address</param>
        public static void SetLocalControllerAddress(uint address)
        {
            Console.Write("[APIC] Local APIC address: ");
            Console.WriteHex(address);
            Console.Write('\n');

            // Map to virtual memory
            m_address = Paging.MapToVirtual(Paging.CurrentDirectory, (int)address, 0x1000, Paging.PageFlags.Present | Paging.PageFlags.Writable/* | Paging.PageFlags.NoCache*/);
        }

        /// <summary>
        /// Sends an "End Of Interrupt"
        /// </summary>
        public static void SendEOI()
        {
            Write(LAPIC_EOI, 0);
        }

        /// <summary>
        /// Writes a value to a register
        /// </summary>
        /// <param name="reg">The register</param>
        /// <param name="value">The value</param>
        public static void Write(uint reg, uint value)
        {
            Util.WriteVolatile32((uint)m_address + reg, value);
        }

        /// <summary>
        /// Reads a value from a register
        /// </summary>
        /// <param name="reg">The register</param>
        /// <returns>The value</returns>
        public static uint Read(uint reg)
        {
            return Util.ReadVolatile32((uint)m_address + reg);
        }

        /// <summary>
        /// Initializes the local APIC
        /// </summary>
        public static void InitLocalAPIC()
        {
            // Initialize local APIC
            Write(LAPIC_DEST_FORMAT, 0xFFFFFFFF);
            Write(LAPIC_LOGICAL_DEST, (Read(LAPIC_LOGICAL_DEST) & 0x00FFFFFF) | 1);
            Write(LAPIC_LVT_TIMER, LAPIC_LVT_INT_MASKED);
            Write(LAPIC_LVT_PERFORMANCE, LAPIC_LVT_DELIVERY_NMI);
            Write(LAPIC_LVT_LINT0, LAPIC_LVT_INT_MASKED);
            Write(LAPIC_LVT_LINT1, LAPIC_LVT_INT_MASKED);
            Write(LAPIC_TASK_PRIO, 0);

            // Enabling is done by writing the spurious INT value and setting the enable bit
            Write(LAPIC_SPURIOUS, 0xFF | LAPIC_SPURIOUS_ENABLE);

            // Start APIC timer
            startTimer();
        }

        /// <summary>
        /// Starts the APIC timer
        /// </summary>
        public static void startTimer()
        {
            // Set APIC timer to use divisor 16
            Write(LAPIC_TIMER_DIVISOR, 0x03);

            // Prepare PIT timer to sleep for 10ms
            uint sleepDivisor = PIT.PrepareSleep(10000);

            // Set APIC init counter
            Write(LAPIC_TIMER_INIT_COUNT, 0xFFFFFFFF);

            // Sleep
            PIT.Sleep(sleepDivisor);

            // Stop APIC timer
            Write(LAPIC_LVT_TIMER, LAPIC_LVT_INT_MASKED);

            // We now know how often the APIC timer has ticked in 10ms
            uint ticks = 0xFFFFFFFF - Read(LAPIC_TIMER_CURRENT_COUNT);
            Time.TicksPerSecond = 1000 / 10;

            // Start periodic APIC timer
            Write(LAPIC_LVT_TIMER, 32 | LAPIC_TIMER_MODE_PERIODIC);
            Write(LAPIC_TIMER_DIVISOR, 0x03);
            Write(LAPIC_TIMER_INIT_COUNT, ticks);
        }

        /// <summary>
        /// Handler for timer
        /// </summary>
        /// <param name="context">The old context</param>
        /// <returns>The new context</returns>
        private static void* timerHandler(void* context)
        {
            // Update ticks
            Time.SubTicks++;

            // One second
            if (Time.SubTicks == Time.TicksPerSecond)
            {
                // Update ticks
                Time.SubTicks = 0;
                Time.FullTicks++;

                Time.Seconds++;
                if (Time.Seconds == 60)
                {
                    Time.Seconds = 0;
                    Time.Minutes++;

                    // Re-read the CMOS time every hour
                    if (Time.Minutes == 60)
                    {
                        CMOS.UpdateTime();
                        Time.FullTicks = Time.CalculateEpochTime();
                    }
                }
            }

            // Context switch
            context = Tasking.Scheduler(context);
            SendEOI();
            return context;
        }
    }
}
