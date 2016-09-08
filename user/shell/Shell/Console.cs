using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Console
    {
        public static extern void Write(string str);

        public static extern void Write(char c);

        public static void WriteLine(string str)
        {
            Write(str);
            Write("\n");
        }

        public static extern char ReadChar();

        internal static string ReadLine()
        {
            char[] buffer = new char[1024];

            char c;
            int i = 0;
            while((c = ReadChar()) != '\n')
            {
                buffer[i++] = c;
                if (i > 1022)
                    break;
            }
            buffer[i] = '\0';

            return Util.CharArrayToString(buffer);
        }


        /// <summary>
        /// Writes a hexadecimal integer to the screen
        /// </summary>
        /// <param name="num">The number</param>
        public static void WriteHex(long num)
        {
            if (num == 0)
            {
                Write('0');
                return;
            }

            // Don't print zeroes at beginning of number
            bool noZeroes = true;
            for (int j = 60; j >= 0; j -= 4)
            {
                long tmp = (num >> j) & 0x0F;
                if (tmp == 0 && noZeroes)
                    continue;

                noZeroes = false;
                if (tmp >= 0x0A)
                {
                    Write((char)(tmp - 0x0A + 'A'));
                }
                else
                {
                    Write((char)(tmp + '0'));
                }
            }
        }
    }
}
