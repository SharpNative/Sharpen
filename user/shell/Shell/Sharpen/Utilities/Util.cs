namespace Sharpen.Utilities
{
    unsafe class Util
    {
        /// <summary>
        /// Converts a char array to a string
        /// </summary>
        /// <param name="array">The char array</param>
        /// <returns>The string</returns>
        internal static extern string CharArrayToString(char[] array);

        /// <summary>
        /// Converts a char pointer to a string
        /// </summary>
        /// <param name="ptr">The char pointer</param>
        /// <returns>The string</returns>
        internal static extern string CharPtrToString(char* ptr);

        /// <summary>
        /// Converts an object to a void pointer
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* ObjectToVoidPtr(object obj);
    }
}
