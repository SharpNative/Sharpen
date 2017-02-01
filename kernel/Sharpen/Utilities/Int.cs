using LibCS2C.Attributes;
using Sharpen.Mem;

namespace Sharpen.Utilities
{
    sealed class Int
    {
        public const int MinValue = -2147483648;
        public const int MaxValue = 2147483647;

        /// <summary>
        /// Parse string to in
        /// </summary>
        /// <param name="value"></param>
        /// <returns>-1 when failed</returns>
        [Plug("System_Int32_Parse_1string_t_")]
        private static int parseImpl(string value)
        {
            int res = 0;
            int sign = 1;
            int index = 0;

            if (value[0] == '-')
            {
                sign = -1;
                index++;
            }

            char c;
            while ((c = value[index++]) != '\0')
            {
                if (c >= '0' && c <= '9')
                {
                    res = res * 10 + c - '0';
                }
                else
                    return -1;
            }

            return sign * res;
        }

        /// <summary>
        /// Converts an integer to a string
        /// </summary>
        /// <param name="val">The integer to convert</param>
        /// <returns>The string</returns>
        [Plug("System_Int32_ToString_1class_")]
        private static unsafe string toStringImpl(int val)
        {
            // This method fails for MinValue because of the sign inversion (overflow)
            if (val == MinValue)
            {
                return String.Clone("-2147483648");
            }

            char* buffer = (char*)Heap.Alloc(12);
            int bufferOffset = 0;

            // Sign
            if (val < 0)
            {
                bufferOffset = 1;
                buffer[0] = '-';
                val = -val;
            }

            // First calculate how long
            int tempVal = val;
            int offset = 10;
            do
            {
                tempVal /= 10;
                offset--;
            } while (offset > 0 && tempVal > 0);

            // End of string mark
            buffer[10 - offset + bufferOffset] = '\0';

            // Put the number in the buffer
            int i = 10;
            do
            {
                buffer[i - offset - 1 + bufferOffset] = "0123456789"[val % 10];

                val /= 10;
                i--;
            } while (i > 0 && val > 0);

            return Util.CharPtrToString(buffer);
        }
    }
}
