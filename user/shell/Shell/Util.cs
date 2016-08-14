using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    unsafe class Util
    {
        internal static extern string CharArrayToString(char[] array);
        internal static extern string CharPtrToString(char* ptr);
    }
}
