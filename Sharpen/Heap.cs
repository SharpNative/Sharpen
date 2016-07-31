using Sharpen.Arch;
using Sharpen.Drivers.Char;

namespace Sharpen
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
        private static int GetRequiredPageCount(int size)
        {
            // Calculate the required amount of pages
            // There is a minimal of 16
            int required = size / 0x1000;
            return (required < 16) ? 16 : required;
        }

        /// <summary>
        /// Creates a block descriptor with given minimal size
        /// </summary>
        /// <param name="size">The minimal size</param>
        /// <returns>The block descriptor</returns>
        private static unsafe BlockDescriptor* CreateBlockDescriptor(int size)
        {
            // Allocate descriptor
            size = GetRequiredPageCount(size) * 0x1000;
            BlockDescriptor* descriptor = (BlockDescriptor*)Paging.AllocatePhysical(size);
            if (descriptor == null)
                return null;

            // Setup block
            Block* first = (Block*)((int)descriptor + sizeof(BlockDescriptor));
            first->Next = null;
            first->Size = size - sizeof(BlockDescriptor);
            first->Used = false;
            first->Descriptor = descriptor;

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
        private static unsafe BlockDescriptor* GetSufficientDescriptor(int size)
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
            BlockDescriptor* newDescriptor = CreateBlockDescriptor(size);
            descriptor->Next = newDescriptor;
            return newDescriptor;
        }

        /// <summary>
        /// Sets up the real heap
        /// </summary>
        public static void SetupRealHeap()
        {
            // Page align the heap
            uint address = (uint)CurrentEnd;
            if (address % 0x1000 != 0)
            {
                address &= 0xFFFFF000;
                address += 0x1000;
            }
            CurrentEnd = (void*)address;

            // First block descriptor and real heap on
            firstDescriptor = CreateBlockDescriptor(16 * 0x1000);
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
                int alignedSize = size * 2;
                if (alignedSize % alignment != 0)
                {
                    alignedSize = size - (size % alignment);
                    alignedSize += alignment;
                }

                BlockDescriptor* descriptor = GetSufficientDescriptor(alignedSize);
                Block* currentBlock = descriptor->First;

                // Search in the descriptor
                while (true)
                {
                    // Can fit in here
                    if (!currentBlock->Used && currentBlock->Size >= size)
                    {
                        // Check if this block data would be aligned
                        {
                            int currentData = (int)currentBlock + sizeof(Block);

                            // Not aligned
                            if (currentData % alignment != 0)
                            {
                                // Split the current block into two
                                // The first part is a padding
                                // The second part is the block we want for allocation
                                Block* padding = currentBlock;

                                // Align
                                int address = currentData;
                                address = address - (address % alignment);
                                address += alignment;
                                address -= sizeof(Block);

                                // Padding size is the difference between the new block address and the old block address
                                int paddingSize = address - (int)currentBlock;
                                int remainingAfterSplit = currentBlock->Size - paddingSize;

                                Block* afterPadding = (Block*)((int)padding + paddingSize);
                                if (paddingSize <= sizeof(Block))
                                {
                                    // Waste of space to include padding and also impossible
                                    Block* next = currentBlock->Next;
                                    Block* prev = currentBlock->Prev;

                                    prev->Next = afterPadding;
                                    afterPadding->Next = next;
                                    afterPadding->Prev = prev;
                                    afterPadding->Size = remainingAfterSplit;
                                    afterPadding->Descriptor = descriptor;
                                }
                                else
                                {
                                    padding->Used = false;
                                    padding->Size = paddingSize;

                                    // Remaining block after padding
                                    afterPadding->Next = padding->Next;
                                    padding->Next = afterPadding;
                                    afterPadding->Prev = padding;
                                    afterPadding->Size = remainingAfterSplit;
                                    afterPadding->Descriptor = descriptor;
                                }

                                currentBlock = afterPadding;
                            }
                        }

                        // Calculate remaining size when splitting this block into two
                        int remaining = currentBlock->Size - size;

                        // When needed, create a block that follows this block
                        Block* newNext = null;
                        if (remaining > sizeof(Block))
                        {
                            // Create block
                            newNext = (Block*)((int)currentBlock + size);
                            newNext->Used = false;
                            newNext->Prev = currentBlock;
                            newNext->Next = currentBlock->Next;
                            newNext->Size = remaining;
                            newNext->Descriptor = descriptor;
                        }

                        // Update current block
                        currentBlock->Used = true;
                        currentBlock->Next = newNext;
                        currentBlock->Size = size;
                        descriptor->FreeSpace -= size;

                        // Return block (skip header)
                        return (void*)((int)currentBlock + sizeof(Block));
                    }

                    // Next block
                    currentBlock = currentBlock->Next;
                    if (currentBlock == null)
                        return null;
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
        /// Frees a piece of memory
        /// </summary>
        /// <param name="ptr">The pointer</param>
        public static unsafe void Free(void* ptr)
        {
            // Grab block (header is just before the data)
            Block* block = (Block*)((int)ptr - sizeof(Block));

            // Not used anymore
            block->Used = false;
            block->Descriptor->FreeSpace += block->Size;

            // Merge forward
            if (block->Next != null && !block->Next->Used)
            {
                Block* next = block->Next;
                block->Size += next->Size;
                block->Next = next->Next;
                next->Next->Prev = block;
            }

            // Merge backwards
            if (block->Prev != null && !block->Prev->Used)
            {
                Block* prev = block->Prev;
                prev->Size += block->Size;
                prev->Next = block->Next;
                block->Next->Prev = prev;
            }
        }

        /// <summary>
        /// Dumps a block
        /// </summary>
        /// <param name="currentBlock">The block</param>
        private static unsafe void DumpBlock(Block* currentBlock)
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
                DumpBlock(currentBlock);
                Keyboard.Getch();

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
            uint address = (uint)CurrentEnd;
            if (align && (address & 0xFFFFF000) > 0)
            {
                address &= 0xFFFFF000;
                address += 0x1000;
            }
            CurrentEnd = (void*)(address + size);
            return (void*)address;
        }
    }
}
