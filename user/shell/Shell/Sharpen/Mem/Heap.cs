using LibCS2C.Attributes;

namespace Sharpen.Mem
{
    unsafe sealed class Heap
    {
        /// <summary>
        /// Allocates a block of memory
        /// </summary>
        /// <param name="size">The size of the memory block</param>
        /// <returns>The pointer to the allocated memory</returns>
        [Extern("malloc")]
        public static extern void* Alloc(int size);

        /// <summary>
        /// Frees a block of memory
        /// </summary>
        /// <param name="ptr">The pointer of the memory block</param>
        [Extern("free")]
        public static extern void Free(void* ptr);

        /// <summary>
        /// Frees an object
        /// </summary>
        /// <param name="obj">The object</param>
        [Extern("free")]
        public static extern void Free(object obj);
    }
}
