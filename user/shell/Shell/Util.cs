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

        /// <summary>
        /// Converts an object to a void pointer
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* ObjectToVoidPtr(object obj);
    }
}
