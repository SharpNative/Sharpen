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
        /// Converts an object to a void pointer
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* ObjectToVoidPtr(object obj);

        /// <summary>
        /// Converts a method to a pointer
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* MethodToPtr(Action method);
    }
}
