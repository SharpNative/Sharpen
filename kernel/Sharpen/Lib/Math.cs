namespace Sharpen.Lib
{
    class Math
    {
        /// <summary>
        /// Gets the absolute value of an integer
        /// </summary>
        /// <param name="num">The integer</param>
        /// <returns>The absolute value of the integer</returns>
        public static int Abs(int num)
        {
            if (num > 0)
                return num;

            return num * -1;
        }

        /// <summary>
        /// Ceils a number
        /// </summary>
        /// <param name="num">The number</param>
        /// <returns>The ceiled number</returns>
        public static int Ceil(double num)
        {
            int inum = (int)num;
            if (num == inum)
                return inum;

            if (num < 0.0)
                return inum;
            else
                return inum + 1;
        }

        /// <summary>
        /// Floors a number
        /// </summary>
        /// <param name="num">The number</param>
        /// <returns>The floored number</returns>
        public static int Floor(double num)
        {
            int inum = (int)num;
            if (num == inum)
                return inum;

            if (num < 0.0)
                return (int)num - 1;
            else
                return (int)num;
        }

        /// <summary>
        /// Returns the largest number of the two
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>The largest number</returns>
        public static int Max(int a, int b)
        {
            if (a > b)
                return a;

            return b;
        }

        /// <summary>
        /// Returns the smallest number of the two
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>The smallest number</returns>
        public static int Min(int a, int b)
        {
            if (a < b)
                return a;

            return b;
        }
    }
}
