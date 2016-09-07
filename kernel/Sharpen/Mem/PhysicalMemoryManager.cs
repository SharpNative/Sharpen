using Sharpen.Arch;
using Sharpen.Collections;

namespace Sharpen.Mem
{
    class PhysicalMemoryManager
    {
        public static bool isInitialized = false;
        private static BitArray m_bitmap;

        /// <summary>
        /// Initializes the physical memory manager
        /// </summary>
        /// <param name="memSize">Memory size</param>
        public static unsafe void Init(uint memSize)
        {
            // Bit array to store which addresses are free
            // The memory size is given in MB, everything is 4KB in this memory manager

            m_bitmap = new BitArray((int)(memSize / 32));
            uint aligned = Paging.Align((uint)Heap.CurrentEnd);

            Console.Write("[PMM] Bitmap at ");
            Console.WriteHex((int)Utilities.Util.ObjectToVoidPtr(m_bitmap));
            Console.Write(", heap at ");
            Console.WriteHex((int)Heap.CurrentEnd);
            Console.Write(", marking 0 - ");
            Console.WriteHex(aligned);
            Console.WriteLine("");

            Set(0, aligned);
            isInitialized = true;
        }

        /// <summary>
        /// Gets the first free
        /// </summary>
        /// <returns>The address</returns>
        public static unsafe void* First()
        {
            int firstFree = m_bitmap.FindFirstFree(false);
            return (void*)(firstFree * 0x1000);
        }

        /// <summary>
        /// Allocates the first free
        /// </summary>
        /// <returns>The address</returns>
        public static unsafe void* Alloc()
        {
            int firstFree = m_bitmap.FindFirstFree(true);
            void* address = (void*)(firstFree * 0x1000);
            return address;
        }

        /// <summary>
        /// Sets the range as used
        /// </summary>
        /// <param name="address">The starting address</param>
        /// <param name="size">The size of the range</param>
        public static void Set(int address, uint size)
        {
            address = (int)Paging.Align((uint)address);
            size = Paging.Align(size);
            for (int i = address; i < size; i += 0x1000)
                Set(i);
        }

        /// <summary>
        /// Sets a single address as used
        /// </summary>
        /// <param name="address">The address</param>
        public static void Set(int address)
        {
            m_bitmap.SetBit(address / 0x1000);
        }

        /// <summary>
        /// Frees a chunk of memory
        /// </summary>
        /// <param name="address">The address</param>
        public static unsafe void Free(void* address)
        {
            int addr = (int)address / 0x1000;
            m_bitmap.ClearBit(addr);
        }
    }
}
