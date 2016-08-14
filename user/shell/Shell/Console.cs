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
                Write(c);
                if (i > 1022)
                    break;
            }
            buffer[i] = '\0';

            return Util.CharArrayToString(buffer);
        }
    }
}
