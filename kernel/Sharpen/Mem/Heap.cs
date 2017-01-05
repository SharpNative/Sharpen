// These two flags are used for debugging
// #define HEAP_DEBUG
// #define HEAP_USE_MAGIC

using Sharpen.Arch;
using Sharpen.Utilities;

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
#if HEAP_USE_MAGIC
            public uint Magic;
#endif
        }

        // Descriptor for space
        private unsafe struct BlockDescriptor
        {
            public int FreeSpace;
            public BlockDescriptor* Next;
            public Block* First;
            public Block* FirstFree;
#if HEAP_USE_MAGIC
            public uint Magic;
#endif
        }

        // Current end address of the heap
        public static void* CurrentEnd { get; private set; }

        // If we use the real heap or not
        public static bool useRealHeap = false;

        // First block descriptor
        private static unsafe BlockDescriptor* firstDescriptor;

        // Mutex
        private static Mutex mutex;

        // Minimal amount of pages in a descriptor
        private const int MINIMALPAGES = 128;

        // Heap magic (DEBUG)
        private const uint HEAP_MAGIC = 0xDEADBEEF;

        /// <summary>
        /// Initializes the temporary heap at the given start address
        /// </summary>
        /// <param name="start">The start address</param>
        public static unsafe void InitTempHeap(void* start)
        {
            Console.Write("[HEAP] Temporary start at ");
            Console.WriteHex((int)start);
            Console.Write('\n');

            CurrentEnd = start;
            mutex = new Mutex();
        }

        /// <summary>
        /// Sets up the real heap
        /// </summary>
        public static void InitRealHeap()
        {
            firstDescriptor = createBlockDescriptor(MINIMALPAGES * 0x1000);
            useRealHeap = true;
            Console.WriteLine("[HEAP] Initialized");
        }

        /// <summary>
        /// Calculates the required amount of pages
        /// </summary>
        /// <param name="size">The requested size</param>
        /// <returns>The required amount of pages</returns>
        private static int getRequiredPageCount(int size)
        {
            // Calculate the required amount of pages (round up to nearest page)
            int required = (int)Paging.Align((uint)size) / 0x1000;

            // Round up to the next minimal pages count (based on power of twos)
            int power = 1;
            while (power < required)
                power *= 2;

            required = power;

#if HEAP_DEBUG
            Console.Write("[HEAP] Required page count: ");
            Console.WriteNum(required);
            Console.Write('\n');
#endif

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
            Console.WriteLine("[HEAP] Creating a new block descriptor");
#endif

            // Add size of Block and BlockDescriptor
            size += sizeof(Block) + sizeof(BlockDescriptor);

            // Allocate descriptor
            size = getRequiredPageCount(size) * 0x1000;
            BlockDescriptor* descriptor = (BlockDescriptor*)Paging.AllocateVirtual(size);

#if HEAP_DEBUG
            Console.Write("[HEAP] New descriptor is at 0x");
            Console.WriteHex((long)descriptor);
            Console.Write('\n');
#endif

            if (descriptor == null)
                Panic.DoPanic("descriptor == null");

            // Setup block
            Block* first = (Block*)((int)descriptor + sizeof(BlockDescriptor));
            first->Prev = null;
            first->Next = null;
            first->Size = size - sizeof(BlockDescriptor);
            first->Used = false;
            first->Descriptor = descriptor;
#if HEAP_USE_MAGIC
            first->Magic = HEAP_MAGIC;
#endif

            // Setup descriptor
            descriptor->FreeSpace = size - sizeof(BlockDescriptor);
            descriptor->First = first;
            descriptor->FirstFree = first;
            descriptor->Next = null;
#if HEAP_USE_MAGIC
            descriptor->Magic = HEAP_MAGIC;
#endif

            return descriptor;
        }

        /// <summary>
        /// Dumps a table of the descriptors
        /// </summary>
        private static unsafe void dumpDescriptors()
        {
            BlockDescriptor* descriptor = firstDescriptor;

            Console.WriteLine("---");

            // Search for a big enough descriptor
            while (true)
            {
                if (descriptor->Next == null)
                    break;

                descriptor = descriptor->Next;

                Console.Write("[");
                Console.WriteHex((int)descriptor);
                Console.WriteLine("]");
            }

            Console.WriteLine("---");
        }

        /// <summary>
        /// Gets a sufficient block descriptor for the required size
        /// </summary>
        /// <param name="size">The required size</param>
        /// <returns>The block descriptor</returns>
        private static unsafe BlockDescriptor* getSufficientDescriptor(BlockDescriptor* first, int size)
        {
            BlockDescriptor* descriptor = first;
            if (descriptor == null)
                descriptor = firstDescriptor;

            // Search for a big enough descriptor
            while (true)
            {
#if HEAP_USE_MAGIC
                if (descriptor->Magic != HEAP_MAGIC)
                    Panic.DoPanic("descriptor->magic != HEAP_MAGIC");
#endif

                if (descriptor != first && descriptor->FreeSpace >= size)
                    return descriptor;

                if (descriptor->Next == null)
                    break;

                descriptor = descriptor->Next;
            }

            // Create next descriptor because there is no descriptor that is big enough
            BlockDescriptor* newDescriptor = createBlockDescriptor(size);
            descriptor->Next = newDescriptor;
            return newDescriptor;
        }

        /// <summary>
        /// Allocates an aligned piece of memory
        /// </summary>
        /// <param name="alignment">The alignment</param>
        /// <param name="size">The size</param>
        public static unsafe void* AlignedAlloc(int alignment, int size)
        {
            if (useRealHeap)
            {
                mutex.Lock();

                // Find a descriptor that is big enough to hold the block header and its data
                // We need to look for something that can hold an aligned size if alignment is requested
                size += sizeof(Block);

                // Safe size
                if (size % alignment != 0)
                {
                    size = size - (size % alignment);
                    size += alignment;
                }

                Block* currentBlock = null;
                Block* previousBlock = null;
                BlockDescriptor* descriptor = getSufficientDescriptor(null, size);

                if (descriptor == null)
                    Panic.DoPanic("descriptor == null");
                
                retry:

                currentBlock = descriptor->FirstFree;
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
#if HEAP_USE_MAGIC
                            first->Magic = HEAP_MAGIC;
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
#if HEAP_USE_MAGIC
                    currentBlock->Magic = HEAP_MAGIC;
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

#if HEAP_USE_MAGIC
                        afterBlock->Magic = HEAP_MAGIC;
#endif

                        if (currentBlock->Next != null)
                            currentBlock->Next->Prev = afterBlock;

                        currentBlock->Next = afterBlock;
                        currentBlock->Size = size;
                    }

                    // Return block (skip header)
                    mutex.Unlock();
                    return (void*)((int)currentBlock + sizeof(Block));

                // Next block
                nextBlock:
                    {
                        previousBlock = currentBlock;
                        currentBlock = currentBlock->Next;
                        if (currentBlock == null)
                        {
                            // This was the last block in the descriptor
                            // Due to alignment issues we haven't found a good place
                            // Get another descriptor that has enough free space
                            descriptor = getSufficientDescriptor(descriptor, size);
                            goto retry;
                        }
                    }
                }
            }
            else
            {
                if (alignment == 0x1000)
                {
                    return KAlloc(size, true);
                }
                else if (alignment == 4)
                {
                    return KAlloc(size, false);
                }
                else
                {
                    Panic.DoPanic("Unsupported alignment in early allocation");
                    return null;
                }
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
            if (ptr == null)
                return;

            // Grab block (header is just before the data)
            Block* block = getBlockFromPtr(ptr);

#if HEAP_USE_MAGIC
            if (block->Magic != HEAP_MAGIC)
            {
                Panic.DoPanic("block->Magic != HEAP_MAGIC");
                return;
            }
#endif

            mutex.Lock();

            // Not used anymore
            block->Used = false;
            block->Descriptor->FreeSpace += block->Size;
            if ((int)block->Descriptor->FirstFree > (int)block)
                block->Descriptor->FirstFree = block;

            // Merge forward
            if (block->Next != null && !block->Next->Used)
            {
                Block* next = block->Next;
                block->Size += next->Size;
                block->Next = next->Next;

                if ((int)block->Descriptor->FirstFree > (int)next)
                    block->Descriptor->FirstFree = next;

                if (next->Next != null)
                    next->Next->Prev = block;
            }

            // Merge backwards
            if (block->Prev != null && !block->Prev->Used)
            {
                Block* prev = block->Prev;
                prev->Size += block->Size;
                prev->Next = block->Next;

                if ((int)block->Descriptor->FirstFree > (int)prev)
                    block->Descriptor->FirstFree = prev;

                if (block->Next != null)
                    block->Next->Prev = prev;
            }

            mutex.Unlock();
        }

        /// <summary>
        /// Frees a piece of memory
        /// </summary>
        /// <param name="obj">The object</param>
        public static unsafe void Free(object ptr)
        {
            Free(Util.ObjectToVoidPtr(ptr));
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

#if HEAP_USE_MAGIC
            Console.Write(" magic=");
            Console.WriteHex(currentBlock->Magic);
#endif
            Console.WriteLine("");
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
            if (useRealHeap)
                Panic.DoPanic("KAlloc has been called after real heap started!");
#endif

            if (PhysicalMemoryManager.isInitialized)
            {
                return PhysicalMemoryManager.AllocRange(size);
            }
            else
            {
                uint address = (uint)CurrentEnd;
                if (align)
                    address = Paging.Align(address);

                // At least 4byte align
                if ((address & 3) != 0)
                {
                    address &= 3;
                    address += 4;
                }

                CurrentEnd = (void*)(address + size);

                return (void*)address;
            }
        }
    }
}