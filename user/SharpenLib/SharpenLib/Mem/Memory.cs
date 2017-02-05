using LibCS2C.Attributes;

namespace Sharpen.Mem
{
    public sealed class Memory
    {
        /// <summary>
        /// Copies the value of num bytes from the source to the destination
        /// </summary>
        /// <param name="destination">The destination</param>
        /// <param name="source">The source</param>
        /// <param name="num">The number of bytes</param>
        [Extern("memcpy")]
        public static extern unsafe void Memcpy(void* destination, void* source, int num);

        /// <summary>
        /// Compares two chunks of memory
        /// </summary>
        /// <param name="s1">The first chunk</param>
        /// <param name="s2">The second chunk</param>
        /// <param name="n">The amount of memory to compare</param>
        /// <returns>Which chunk was greater or less, or zero if equal</returns>
        [Extern("memcmp")]
        public static extern unsafe int Memcmp(void* s1, void* s2, int n);

        /// <summary>
        /// Compares two chunks of memory
        /// </summary>
        /// <param name="s1">The first chunk</param>
        /// <param name="s2">The second chunk</param>
        /// <param name="n">The amount of memory to compare</param>
        /// <returns>If the two chunks are equal</returns>
        public static unsafe bool Compare(void* s1, void* s2, int n)
        {
            return (Memcmp(s1, s2, n) == 0);
        }

        /// <summary>
        /// Sets the num bytes of the block of memory pointed by ptr to the specified value
        /// </summary>
        /// <param name="ptr">The destination pointer</param>
        /// <param name="value">The value</param>
        /// <param name="num">The number of bytes</param>
        [Extern("memset")]
        public static extern unsafe void Memset(void* ptr, int value, int num);
    }
}
