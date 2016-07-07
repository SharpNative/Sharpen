using System;

namespace Sharpen
{
    public sealed class Util
    {
        /// <summary>
        /// Converts a char pointer to a string
        /// </summary>
        /// <param name="ptr">The char pointer</param>
        /// <returns>The string</returns>
        public static unsafe extern string CharPtrToString(char* ptr);

        /// <summary>
        /// Converts a method to a pointer
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* MethodToPtr(Action method);
    }
}
