namespace Sharpen
{
    public unsafe sealed class Heap
    {
        private static void* m_heapStart;

        /// <summary>
        /// Initializes the heap at the given start address
        /// </summary>
        /// <param name="start">The start address</param>
        public static unsafe void Init(void* start)
        {
            Console.Write("Heap start: ");
            Console.WriteHex((int)start);
            Console.PutChar('\n');

            int address = (int)start;
            if (address % 0x1000 != 0)
            {
                address &= ~0x1000;
                address += 0x1000;
            }

            m_heapStart = (void*)address;
        }

        /// <summary>
        /// Allocates an aligned piece of memory
        /// </summary>
        /// <param name="alignment">The alignment</param>
        /// <param name="size">The size</param>
        public static unsafe void* AlignedAlloc(int alignment, int size)
        {
            // TODO
            return Alloc(size);
        }

        /// <summary>
        /// Allocates a piece of memory
        /// </summary>
        /// <param name="size">The size</param>
        public static unsafe void* Alloc(int size)
        {
            void* ret = m_heapStart;
            if ((size & 3) != 0)
            {
                size &= ~3;
                size += 4;
            }
            m_heapStart = (void*)((int)m_heapStart + size);
            return ret;
        }

        /// <summary>
        /// Frees a piece of memory
        /// </summary>
        /// <param name="ptr">The pointer</param>
        public static unsafe void Free(void* ptr)
        {
            // TODO
        }
    }
}
