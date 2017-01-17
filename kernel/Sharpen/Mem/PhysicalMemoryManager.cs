using Sharpen.Arch;
using Sharpen.Collections;

namespace Sharpen.Mem
{
    class PhysicalMemoryManager
    {
        public static bool isInitialized = false;
        private static BitArray bitmap;
        private static Mutex mutex;

        /// <summary>
        /// Initializes the physical memory manager
        /// </summary>
        /// <param name="memSize">Memory size</param>
        public static unsafe void Init(uint memSize)
        {
            // Bit array to store which addresses are free
            bitmap = new BitArray((int)(memSize * 1024 / 4));
            mutex = new Mutex();
            uint aligned = Paging.Align((uint)Heap.CurrentEnd);

            Console.Write("[PMM] Bitmap at ");
            Console.WriteHex((int)Utilities.Util.ObjectToVoidPtr(bitmap));
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
        public static unsafe void* FirstFree()
        {
            mutex.Lock();
            int firstFree = bitmap.FindFirstFree(false);
            mutex.Unlock();
            return (void*)(firstFree * 0x1000);
        }

        /// <summary>
        /// Checks if an address is in use or is free
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>If the address is free</returns>
        public static unsafe bool IsFree(void* address)
        {
            return !bitmap.IsBitSet((int)address / 0x1000);
        }

        /// <summary>
        /// Allocates the first free
        /// </summary>
        /// <returns>The address</returns>
        public static unsafe void* Alloc()
        {
            mutex.Lock();
            int firstFree = bitmap.FindFirstFree(true);
            mutex.Unlock();
            void* address = (void*)(firstFree * 0x1000);
            return address;
        }

        /// <summary>
        /// Allocates the first free range
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The start address</returns>
        public static unsafe void* AllocRange(int size)
        {
            size = (int)Paging.Align((uint)size) / 0x1000;
            mutex.Lock();
            int firstFree = bitmap.FindFirstFreeRange(size, true);
            mutex.Unlock();
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
            mutex.Lock();
            for (int i = address; i < size; i += 0x1000)
            {
                bitmap.SetBit(i / 0x1000);
            }
            mutex.Unlock();
        }

        /// <summary>
        /// Sets a single address as used
        /// </summary>
        /// <param name="address">The address</param>
        public static void Set(int address)
        {
            mutex.Lock();
            bitmap.SetBit(address / 0x1000);
            mutex.Unlock();
        }

        /// <summary>
        /// Frees a block of memory
        /// </summary>
        /// <param name="address">The address</param>
        public static unsafe void Free(void* address)
        {
            int addr = (int)address / 0x1000;
            mutex.Lock();
            bitmap.ClearBit(addr);
            mutex.Unlock();
        }
    }
}
