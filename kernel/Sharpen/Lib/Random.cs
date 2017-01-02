namespace Sharpen.Lib
{
    unsafe class Random
    {
        private static uint m_seed;

        /// <summary>
        /// Initializes the random number generator
        /// </summary>
        public static void Init()
        {
            m_seed = Time.CalculateEpochTime();
        }

        /// <summary>
        /// Generates a random number
        /// </summary>
        /// <returns>The random number</returns>
        public static int Rand()
        {
            uint next = m_seed;
            int result;

            next *= 1103515245;
            next += 12345;
            result = (int)(next / 65536) % 2048;

            next *= 1103515245;
            next += 12345;
            result <<= 10;
            result ^= (int)(next / 65536) % 1024;

            next *= 1103515245;
            next += 12345;
            result <<= 10;
            result ^= (int)(next / 65536) % 1024;

            m_seed = next;

            return result;
        }
    }
}
