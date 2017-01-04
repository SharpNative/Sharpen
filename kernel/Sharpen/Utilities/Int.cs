using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Utilities
{
    class Int
    {
        /// <summary>
        /// Parse string to in
        /// </summary>
        /// <param name="value"></param>
        /// <returns>-1 when failed</returns>
        public static unsafe int Parse(string value)
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
        /// Int to string
        /// 
        /// NOTE: See todos
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static unsafe string ToString(int val)
        {
            if(val == 0)
            {
                return String.Clone("0");
            }

            char[] characters = new char[10];
            for (int j = 0; j < 10; j++)
                characters[j] = (char)('0' + j);

            char *buffer = (char *)Heap.Alloc(12);
            buffer[11] = (char)0x00;

            int i = 10;

            while(i > 0 && val > 0)
            {
                // TODO: "0123456789"[val % 10]
                buffer[i] = characters[val % 10];

                val /= 10;
                i--;
            }

            Heap.Free(characters);

            return Util.CharPtrToString(buffer + i + 1);
        }
    }
}
