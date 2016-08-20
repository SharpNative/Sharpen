//#define HEAP_DEBUG

using Sharpen.Arch;
using Sharpen.Drivers.Char;

namespace Sharpen.Mem
{
    public unsafe sealed class Heap
    {
        // Block of memory
        private unsafe struct Block
        {
            public int Size;
            public bool Used;
            public Block* Prev;
            public Block* Next;
            public BlockDescriptor* Descriptor;
#if HEAP_DEBUG
            public uint Magic;
#endif
        }

        // Descriptor for space
        private unsafe struct BlockDescriptor
        {
            public int FreeSpace;
            public BlockDescriptor* Next;
            public Block* First;
        }

        // Current end address of the heap
        public static void* CurrentEnd { get; private set; }
        
        // If we use the real heap or not
        private static bool m_realHeap = false;

        // First block descriptor
        private static unsafe BlockDescriptor* firstDescriptor;

        // Minimal amount of pages in a descriptor
        private const int MINIMALPAGES = 32;

        // Heap magic (DEBUG)
        private const uint HEAPMAGIC = 0xDEADBEEF;

        /// <summary>
        /// Initializes the heap at the given start address
        /// </summary>
        /// <param name="start">The start address</param>
        public static unsafe void Init(void* start)
        {
            Console.Write("[HEAP] Temporary start at ");
            Console.WriteHex((int)start);
            Console.PutChar('\n');

            CurrentEnd = start;
        }

        /// <summary>
        /// Calculates the required amount of pages
        /// </summary>
        /// <param name="size">The requested size</param>
        /// <returns>The required amount of pages</returns>
        private static int getRequiredPageCount(int size)
        {
            // Calculate the required amount of pages
            int required = size / 0x1000;
            return (required < MINIMALPAGES) ? MINIMALPAGES : required;
        }

        /// <summary>
        /// Creates a block descriptor with given minimal size
        /// </summary>
        /// <param name="size">The minimal size</param>
        /// <returns>The block descriptor</returns>
        private static unsafe BlockDescriptor* createBlockDescriptor(int size)
        {
#if HEAP_DEBUG
            Console.WriteLine("CREATING NEW BLOCK DESCRIPTOR");
#endif

            // Allocate descriptor
            size = getRequiredPageCount(size) * 0x1000;
            BlockDescriptor* descriptor = (BlockDescriptor*)Paging.AllocateVirtual(size);
            
            if (descriptor == null)
            {
                Panic.DoPanic("descriptor == null");
                return null;
            }

            // Setup block
            Block* first = (Block*)((int)descriptor + sizeof(BlockDescriptor));
            first->Next = null;
            first->Size = size - sizeof(BlockDescriptor);
            first->Used = false;
            first->Descriptor = descriptor;
#if HEAP_DEBUG
            first->Magic = HEAPMAGIC;
#endif
            
            // Setup descriptor
            descriptor->FreeSpace = size;
            descriptor->First = first;
            descriptor->Next = null;
            
            return descriptor;
        }

        /// <summary>
        /// Gets a sufficient block descriptor for the required size
        /// </summary>
        /// <param name="size">The required size</param>
        /// <returns>The block descriptor</returns>
        private static unsafe BlockDescriptor* getSufficientDescriptor(int size)
        {
            BlockDescriptor* descriptor = firstDescriptor;

            // Search for a big enough descriptor
            while (true)
            {
                if (descriptor->FreeSpace >= size)
                    return descriptor;

                if (descriptor->Next == null)
                    break;

                descriptor = descriptor->Next;
            }

            // Create next descriptor because there is no descriptor that is big enough
            BlockDescriptor* newDescriptor = createBlockDescriptor(size * 2);
            descriptor->Next = newDescriptor;
            return newDescriptor;
        }

        /// <summary>
        /// Sets up the real heap
        /// </summary>
        public static void SetupRealHeap()
        {
            // Page align the heap
            uint address = Paging.Align((uint)CurrentEnd);
            CurrentEnd = (void*)address;
            
            // First block descriptor and real heap on
            firstDescriptor = createBlockDescriptor(MINIMALPAGES * 0x1000);
            m_realHeap = true;

            Console.Write("[HEAP] Currently at ");
            Console.WriteHex(address);
            Console.PutChar('\n');
        }

        /// <summary>
        /// Allocates an aligned piece of memory
        /// </summary>
        /// <param name="alignment">The alignment</param>
        /// <param name="size">The size</param>
        public static unsafe void* AlignedAlloc(int alignment, int size)
        {
            if (m_realHeap)
            {
                // Find a descriptor that is big enough to hold the block header and its data
                // We need to look for something that can hold an aligned size if alignment is requested
                size += sizeof(Block);

                // Safe size
                if (size % alignment != 0)
                {
                    size = size - (size % alignment);
                    size += alignment;
                }

                Block* currentBlock;
                Block* previousBlock;
                int safeSize = size * 2;
                BlockDescriptor* descriptor = getSufficientDescriptor(safeSize);

            retry:

                currentBlock = descriptor->First;
                previousBlock = null;

                // Search in the descriptor
                while (true)
                {
                    // Can fit in here
                    if (currentBlock->Used || currentBlock->Size < size)
                        goto nextBlock;
                    
                    // Check if this block data would be aligned
                    int currentData = (int)currentBlock + sizeof(Block);
                    int remainder = currentData % alignment;

                    // Not aligned
                    if (remainder != 0)
                    {
                        // Split the current block into two
                        // The first part is a padding
                        // The second part is the block we want for allocation
                        // This only happens if there is enough space left

                        // Size of gap
                        int gapSize = alignment - remainder;
                        int newSize = currentBlock->Size - gapSize;

                        // Would the new block be too small to fit our data into?
                        if (newSize < size)
                            goto nextBlock;

                        // Store old data
                        Block* newNext = currentBlock->Next;
                        bool newUsed = currentBlock->Used;

                        // Move block forward
                        currentBlock = (Block*)((int)currentBlock + gapSize);
                        currentBlock->Used = newUsed;
                        currentBlock->Prev = previousBlock;
                        currentBlock->Next = newNext;
                        currentBlock->Size = newSize;
                        currentBlock->Descriptor = descriptor;

                        // Increase size of previous block if needed
                        if (previousBlock != null)
                        {
                            previousBlock->Next = currentBlock;
                            previousBlock->Size += gapSize;

                            // If the block is used and the gap is merged
                            // that means that the total free space in this descriptor decreases
                            if (currentBlock->Used)
                                descriptor->FreeSpace -= gapSize;
                        }
                        // This is the first block that was moved
                        else if (gapSize >= sizeof(Block))
                        {
                            // Update header
                            Block* first = descriptor->First;
                            descriptor->FreeSpace -= gapSize;

                            first->Used = false;
                            first->Prev = null;
                            first->Next = currentBlock;
                            first->Size = gapSize;
                            first->Descriptor = descriptor;
#if HEAP_DEBUG
                            first->Magic = HEAPMAGIC;
#endif
                            currentBlock->Prev = first;
                        }
                    }

                    // Calculate leftover part for the next block
                    int leftover = currentBlock->Size - size;

                    // Update header
                    currentBlock->Used = true;
                    currentBlock->Descriptor = descriptor;
                    currentBlock->Prev = previousBlock;
#if HEAP_DEBUG
                    currentBlock->Magic = HEAPMAGIC;
#endif
                    descriptor->FreeSpace -= size;

                    // If we have something left over, create a new block
                    if (leftover > sizeof(Block) + 4)
                    {
                        // Update header
                        Block* afterBlock = (Block*)((int)currentBlock + size);
                        afterBlock->Size = leftover;
                        afterBlock->Used = false;
                        afterBlock->Next = currentBlock->Next;
                        afterBlock->Prev = currentBlock;
                        afterBlock->Descriptor = descriptor;

#if HEAP_DEBUG
                        afterBlock->Magic = HEAPMAGIC;
#endif

                        if (currentBlock->Next != null)
                            currentBlock->Next->Prev = afterBlock;

                        currentBlock->Next = afterBlock;
                        currentBlock->Size = size;
                    }

                    // Return block (skip header)
                    return (void*)((int)currentBlock + sizeof(Block));

                // Next block
                nextBlock:
                    {
                        previousBlock = currentBlock;
                        currentBlock = currentBlock->Next;
                        if (currentBlock == null)
                        {
                            descriptor = createBlockDescriptor(safeSize);
                            goto retry;
                        }
                    }
                }
            }
            else
            {
                if (alignment == 0x1000)
                    return KAlloc(size, true);
                else
                    return KAlloc(size, false);
            }
        }

        /// <summary>
        /// Allocates a piece of memory
        /// </summary>
        /// <param name="size">The size</param>
        public static unsafe void* Alloc(int size)
        {
            return AlignedAlloc(4, size);
        }

        /// <summary>
        /// Gets the block from the pointer
        /// </summary>
        /// <param name="ptr">The pointer</param>
        /// <returns>The block</returns>
        private static unsafe Block* getBlockFromPtr(void* ptr)
        {
            Block* block = (Block*)((int)ptr - sizeof(Block));
            return block;
        }

        /// <summary>
        /// Expands the memory block of ptr to newSize
        /// </summary>
        /// <param name="ptr">The pointer to the memory block</param>
        /// <param name="newSize">The new size of the block</param>
        /// <returns>The memory block</returns>
        public static unsafe void* Expand(void* ptr, int newSize)
        {
            Block* block = getBlockFromPtr(ptr);

            void* newPtr = AlignedAlloc(4, newSize);
            Memory.Memcpy(newPtr, ptr, block->Size);
            Free(ptr);

            return newPtr;
        }

        /// <summary>
        /// Frees a piece of memory
        /// </summary>
        /// <param name="ptr">The pointer</param>
        public static unsafe void Free(void* ptr)
        {
            // Grab block (header is just before the data)
            Block* block = getBlockFromPtr(ptr);

#if HEAP_DEBUG
            if (block->Magic != HEAPMAGIC)
            {
                Panic.DoPanic("block->Magic != HEAPMAGIC");
                return;
            }
#endif

            // Not used anymore
            block->Used = false;
            block->Descriptor->FreeSpace += block->Size;

            // Merge forward
            if (block->Next != null && !block->Next->Used)
            {
                Block* next = block->Next;
                block->Size += next->Size;
                block->Next = next->Next;

                if (next->Next != null)
                    next->Next->Prev = block;
            }

            // Merge backwards
            if (block->Prev != null && !block->Prev->Used)
            {
                Block* prev = block->Prev;
                prev->Size += block->Size;
                prev->Next = block->Next;

                if (block->Next != null)
                    block->Next->Prev = prev;
            }
        }

        /// <summary>
        /// Dumps a block
        /// </summary>
        /// <param name="currentBlock">The block</param>
        private static unsafe void dumpBlock(Block* currentBlock)
        {
            Console.Write("Block: size=");
            Console.WriteHex(currentBlock->Size);
            Console.Write(" used:");
            Console.Write((currentBlock->Used ? "yes" : "no"));
            Console.Write(" prev=");
            Console.WriteHex((int)currentBlock->Prev);
            Console.Write(" next=");
            Console.WriteHex((int)currentBlock->Next);
            Console.Write(" i am=");
            Console.WriteHex((int)currentBlock);
            
#if HEAP_DEBUG
            Console.Write(" magic=");
            Console.WriteHex(currentBlock->Magic);
#endif
            Console.WriteLine("");
        }

        /// <summary>
        /// Dumps the data
        /// </summary>
        public static void Dump()
        {
            BlockDescriptor* descriptor = firstDescriptor;
            Block* currentBlock = descriptor->First;

            // Search in the descriptor
            while (true)
            {
                dumpBlock(currentBlock);
                // Keyboard.Getch();

                currentBlock = currentBlock->Next;
                if (currentBlock == null)
                    return;
            }
        }

        /// <summary>
        /// Temporary kernel memory allocation before a real heap is set up
        /// </summary>
        /// <param name="size">The size</param>
        /// <param name="align">If the block should be aligned</param>
        /// <returns>A block of memory</returns>
        public static unsafe void* KAlloc(int size, bool align)
        {
#if HEAP_DEBUG
            if (m_realHeap)
            {
                Panic.DoPanic("KAlloc has been called after real heap started!");
                return null;
            }
#endif
            
            uint address = (uint)CurrentEnd;
            if (align)
                address = Paging.Align(address);
            
            // Update physical memory manager
            PhysicalMemoryManager.Set((int)address, (uint)size);

            CurrentEnd = (void*)(address + size);

            return (void*)address;
        }
    }
}
