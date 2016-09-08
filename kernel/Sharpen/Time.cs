﻿namespace Sharpen
{
    public sealed class Time
    {
        public struct Timeval
        {
            public ulong tv_sec;
            public ulong tv_usec;
        }

        /// <summary>
        /// The current seconds
        /// </summary>
        public static int Seconds { get; set; }

        /// <summary>
        /// The current minutes
        /// </summary>
        public static int Minutes { get; set; }

        /// <summary>
        /// The current hours
        /// </summary>
        public static int Hours { get; set; }

        /// <summary>
        /// The current day (of the month)
        /// </summary>
        public static int Day { get; set; }

        /// <summary>
        /// The current month
        /// </summary>
        public static int Month { get; set; }

        /// <summary>
        /// The current year
        /// </summary>
        public static int Year { get; set; }

        /// <summary>
        /// Days per month, assumes no leap year
        /// </summary>
        private static int[] m_daysPerMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Checks if a year is a leap year
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>If it's a leap year</returns>
        public static bool IsLeapYear(int year)
        {
            return ((year % 4) == 0 && ((year % 25) != 0 || (year % 400) == 0));
        }

        /// <summary>
        /// Calculate the epoch time
        /// </summary>
        /// <returns>Amount of seconds elapsed since Jan 1 1970</returns>
        public static int CalculateEpochTime()
        {
            int daySecs = (Hours * 60 * 60) + (Minutes * 60) + Seconds;
            int days = Day - 1;
            
            // Zero based month number
            // Count all days of the fully passed months
            int cap = Month - 1;
            for (int i = 0; i < cap; i++)
            {
                days += m_daysPerMonth[i];
            }

            // If february has passed and we're in a leap year
            // then: add the leap day
            if (Month > 1 && IsLeapYear(Year))
                days++;

            // Calculate the amount of total days of full years since 1970
            int currentYear = 1970;
            cap = Year;
            while(currentYear < cap)
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
