namespace Sharpen.Arch
{
    public sealed class CMOS
    {
        // CMOS ports
        public static readonly ushort CMOS_CMD = 0x70;
        public static readonly ushort CMOS_DATA = 0x71;

        // RTC CMOS registers
        public static readonly byte CMOS_RTC_SECONDS = 0x00;
        public static readonly byte CMOS_RTC_MINUTES = 0x02;
        public static readonly byte CMOS_RTC_HOURS = 0x04;
        public static readonly byte CMOS_RTC_WEEKDAY = 0x06;
        public static readonly byte CMOS_RTC_MONTHDAY = 0x07;
        public static readonly byte CMOS_RTC_MONTH = 0x08;
        public static readonly byte CMOS_RTC_YEAR = 0x09;

        // CMOS status
        public static readonly byte CMOS_STATUS_A = 0x0A;
        public static readonly byte CMOS_STATUS_B = 0x0B;

        // RTC flags in CMOS status
        public static readonly int CMOS_RTC_UPDATING = (1 << 7); // Bit 7 in status register A is set when RTC update is happening
        public static readonly int CMOS_RTC_24H = (1 << 1);      // Bit 1 in status register B is set for 24h mode
        public static readonly int CMOS_RTC_BIN_MODE = (1 << 2); // Bit 2 in status register B is set for binary mode
        public static readonly int CMOS_RTC_HOURS_PM = (1 << 7); // Bit 7 is set on read hours value if it's in pm

        /// <summary>
        /// Gets data from a CMOS register
        /// </summary>
        /// <param name="reg">The register to read the data from</param>
        /// <returns>The data</returns>
        public static byte GetData(byte reg)
        {
            PortIO.Out8(CMOS_CMD, reg);
            return PortIO.In8(CMOS_DATA);
        }

        /// <summary>
        /// Converts BCD number format to BIN number format
        /// </summary>
        /// <param name="x">The input value in BCD format</param>
        /// <returns>The output number in BIN format</returns>
        public static int BCD_TO_BIN(int x)
        {
            return (((x >> 4) * 10) + (x & 0x0F));
        }

        /// <summary>
        /// Updates the current time to the CMOS time
        /// </summary>
        public static void UpdateTime()
        {
            // Get status
            byte statusB = GetData(CMOS_STATUS_B);
            bool is24h = ((statusB & CMOS_RTC_24H) > 0);
            bool isBin = ((statusB & CMOS_RTC_BIN_MODE) > 0);

            // The values of the CMOS RTC might change while we're reading
            // this happens for example when the seconds can be set to zero while minutes need to be incremented
            // To prevent this problem, we read the values twice and see if they're the same
            // if not, we keep reading until that's the case

            // The old values
            int oldSeconds, oldMinutes, oldHours, oldDay, oldMonth, oldYear;
            
            // First, wait until the CMOS is not updating
            while ((GetData(CMOS_STATUS_A) & CMOS_RTC_UPDATING) > 0) ;

            // Let's do the job
            do
            {
                // Store old values
                oldSeconds = Time.Seconds;
                oldMinutes = Time.Minutes;
                oldHours = Time.Hours;
                oldDay = Time.Day;
                oldMonth = Time.Month;
                oldYear = Time.Year;

                // Wait until the CMOS is not updating to check again
                while ((GetData(CMOS_STATUS_A) & CMOS_RTC_UPDATING) > 0) ;

                // Read time and date
                Time.Seconds = GetData(CMOS_RTC_SECONDS);
                Time.Minutes = GetData(CMOS_RTC_MINUTES);
                Time.Hours = GetData(CMOS_RTC_HOURS);
                Time.Day = GetData(CMOS_RTC_MONTHDAY);
                Time.Month = GetData(CMOS_RTC_MONTH);
                Time.Year = GetData(CMOS_RTC_YEAR);

                // Convert to 24h if needed
                if (!is24h && ((Time.Hours & CMOS_RTC_HOURS_PM) > 0))
                {
                    Time.Hours &= ~CMOS_RTC_HOURS_PM;
                    Time.Hours += 12;

                    // Midnight is actually reported as 12 and not 0, fix this
                    if (Time.Hours == 24)
                    {
                        Time.Hours = 0;
                    }
                }

                // Convert seconds, minutes and hours if required
                if (!isBin)
                {
                    Time.Seconds = BCD_TO_BIN(Time.Seconds);
                    Time.Minutes = BCD_TO_BIN(Time.Minutes);
                    Time.Hours = BCD_TO_BIN(Time.Hours);
                    Time.Day = BCD_TO_BIN(Time.Day);
                    Time.Month = BCD_TO_BIN(Time.Month);
                    Time.Year = BCD_TO_BIN(Time.Year);
                }

                // Add century to year
                Time.Year += 2000;
            }
            while (oldSeconds != Time.Seconds ||
                   oldMinutes != Time.Minutes ||
                   oldHours != Time.Hours ||
                   oldDay != Time.Day ||
                   oldMonth != Time.Month ||
                   oldYear != Time.Year);
        }
    }
}
