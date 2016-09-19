namespace Sharpen.Memory
{
    unsafe class Heap
    {
        /// <summary>
        /// Allocates a block of memory
        /// </summary>
        /// <param name="size">The size of the memory block</param>
        /// <returns>The pointer to the allocated memory</returns>
        public static extern void* Alloc(int size);

        /// <summary>
        /// Frees a block of memory
        /// </summary>
        /// <param name="ptr">The pointer of the memory block</param>
        public static extern void Free(void* ptr);
    }
}
