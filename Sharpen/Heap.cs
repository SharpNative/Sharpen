namespace Sharpen
{
    public sealed class Heap
    {
        /// <summary>
        /// Initializes the heap at the given start address
        /// </summary>
        /// <param name="start">The start address</param>
        public static unsafe extern void Init(void* start);

        /// <summary>
        /// Allocates an aligned piece of memory
        /// </summary>
        /// <param name="alignment">The alignment</param>
        /// <param name="size">The size</param>
        public static unsafe extern void* AlignedAlloc(int alignment, int size);

        /// <summary>
        /// Allocates a piece of memory
        /// </summary>
        /// <param name="size">The size</param>
        public static unsafe extern void* Alloc(int size);

        /// <summary>
        /// Frees a piece of memory
        /// </summary>
        /// <param name="ptr">The pointer</param>
        public static unsafe extern void Free(void* ptr);
    }
}
