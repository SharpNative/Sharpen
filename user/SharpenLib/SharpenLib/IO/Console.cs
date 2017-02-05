using LibCS2C.Attributes;
using Sharpen.Utilities;

namespace Sharpen.IO
{
    public class Console
    {
        /// <summary>
        /// Writes a string to the console without a newline
        /// </summary>
        /// <param name="str">The string</param>
        public static extern void Write(string str);

        /// <summary>
        /// Writes a character to the console
        /// </summary>
        /// <param name="c">The character</param>
        [Extern("putchar")]
        public static extern void Write(char c);

        /// <summary>
        /// Flushes the output stream
        /// </summary>
        public static extern void Flush();

        /// <summary>
        /// Writes a line to the console
        /// </summary>
        /// <param name="str">The line</param>
        public static void WriteLine(string str)
        {
            Write(str);
            Write("\n");
        }

        /// <summary>
        /// Writes a line to the console
        /// </summary>
        /// <param name="num">The number</param>
        public static void WriteLine(int num)
        {
            Write(num);
            Write("\n");
        }

        /// <summary>
        /// Reads a character from the console
        /// </summary>
        /// <returns>The character</returns>
        public static extern char ReadChar();

        /// <summary>
        /// Reads a line from the console
        /// </summary>
        /// <returns>The line</returns>
        public static string ReadLine()
        {
            char[] buffer = new char[1024];

            char c;
            int i = 0;
            while ((c = ReadChar()) != '\n')
            {
                buffer[i++] = c;
                if (i >= 1023)
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

        /// <summary>
        /// Writes an integer to the screen
        /// </summary>
        /// <param name="num">The number</param>
        public static void Write(int num)
        {
            if (num == 0)
            {
                Write('0');
                return;
            }

            if (num < 0)
            {
                Write('-');
                num = -num;
            }

            int a = num % 10;
            if (num >= 10)
                Write(num / 10);

            Write((char)('0' + a));
        }
    }
}
