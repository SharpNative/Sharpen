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
    }
}
