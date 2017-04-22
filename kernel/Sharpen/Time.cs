namespace Sharpen
{
    public sealed class Time
    {
        public struct Timeval
        {
            public ulong tv_sec;
            public ulong tv_usec;
        }

        public static uint SubTicks { get; set; }
        public static uint FullTicks { get; set; }
        public static uint TicksPerSecond { get; set; }
        
        public static uint Seconds { get; set; }
        public static uint Minutes { get; set; }
        public static uint Hours { get; set; }
        public static uint Day { get; set; }
        public static uint Month { get; set; }
        public static uint Year { get; set; }

        /// <summary>
        /// Days per month, assumes no leap year
        /// </summary>
        private static uint[] m_daysPerMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Checks if a year is a leap year
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>If it's a leap year</returns>
        public static bool IsLeapYear(uint year)
        {
            return ((year % 4) == 0 && ((year % 25) != 0 || (year % 400) == 0));
        }

        /// <summary>
        /// Calculate the epoch time
        /// </summary>
        /// <returns>Amount of seconds elapsed since Jan 1 1970</returns>
        public static uint CalculateEpochTime()
        {
            uint daySecs = (Hours * 60 * 60) + (Minutes * 60) + Seconds;
            uint days = Day - 1;
            
            // Zero based month number
            // Count all days of the fully passed months
            uint cap = Month - 1;
            for (int i = 0; i < cap; i++)
            {
                days += m_daysPerMonth[i];
            }
            
            // If february has passed and we're in a leap year
            // then: add the leap day
            if (Month > 1 && IsLeapYear(Year))
                days++;
            
            // Calculate the amount of total days of full years since 1970
            uint currentYear = 1970;
            cap = Year;
            while (currentYear < cap)
            {
                if (IsLeapYear(currentYear))
                    days += 366;
                else
                    days += 365;

                currentYear++;
            }
            
            return (daySecs + (days * 24 * 60 * 60));
        }
    }
}
