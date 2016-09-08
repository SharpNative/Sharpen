using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.Arch
{
    public sealed class Paging
    {
        // Flags on a page
        public enum PageFlags
        {
            Present = 1,
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            NoCache = (1 << 0x4)
        }

        // Flags on a table
        public enum TableFlags
        {
            Present = 1,
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            WriteThrough = (1 << 0x3),
            NoCache = (1 << 0x4),
            SizeMegs = (1 << 0x7)
        }

        public unsafe struct PageTable
        {
            public fixed int pages[1024];
        }

        public unsafe struct PageDirectory
        {
            public fixed int tables[1024];
        }

        // Kernel directory
        public static unsafe PageDirectory* KernelDirectory { get; private set; }

        /// <summary>
        /// The current paging directory
        /// </summary>
        public static unsafe PageDirectory* CurrentDirectory
        {
            get
            {
                return m_currentDirectory;
            }

            set
            {
                m_currentDirectory = value;
                setDirectoryInternal(value);
            }
        }

        private static BitArray m_bitmap;
        private static unsafe PageDirectory* m_currentDirectory;

        #region Helpers

        /// <summary>
        /// Sets the frame address of the page
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int ToFrameAddress(int a)
        {
            return ((a / 0x1000) << 0xC);
        }

        /// <summary>
        /// Gets the frame address of the page
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetFrameAddress(int a)
        {
            return (a >> 0xC) * 0x1000;
        }

        #endregion

        /// <summary>
        /// Initializes paging
        /// </summary>
        /// <param name="memSize">Memory size</param>
        public static unsafe void Init(uint memSize)
        {
            // Create kernel directory
            KernelDirectory = (PageDirectory*)PhysicalMemoryManager.Alloc();
            CurrentDirectory = KernelDirectory;
            Memory.Memset(KernelDirectory, 0, sizeof(PageDirectory));

            // Bit array to store which frames are free
            // One entry holds 0x1000 * 32 bytes
            m_bitmap = new BitArray((int)(memSize * 1024 / 0x1000 / 32));

            // Identity map
            int address = 0;
            int flags = (int)PageFlags.Present | (int)PageFlags.Writable | (int)PageFlags.UserMode;
            uint end = /*(memSize - 10 * 1024) * 1024*/0x1600000;
            while (address < end)
            {
                MapPage(KernelDirectory, address, address, flags);
                SetFrame(address);
                address += 0x1000;
            }

            // Enable paging
            Enable();
        }

        /// <summary>
        /// Maps a page in a directory
        /// </summary>
        /// <param name="directory">The paging directory</param>
        /// <param name="phys">The physical address</param>
        /// <param name="virt">The virtual address</param>
        /// <param name="flags">The flags</param>
        public static unsafe void MapPage(PageDirectory* directory, int phys, int virt, int flags)
        {
            // Get indices
            int pageIndex = virt / 0x1000;
            int tableIndex = pageIndex / 1024;

            // Create page table if it doesn't exist
            if (directory->tables[tableIndex] == 0)
            {
                // Allocate table
                PageTable* newTable = (PageTable*)PhysicalMemoryManager.Alloc();
                if (newTable == null)
                {
                    Panic.DoPanic("newTable == null");
                    return;
                }

                // Set flags
                int flaggedTable = (int)newTable | flags;
                directory->tables[tableIndex] = flaggedTable;

                MapPage(directory, (int)newTable, (int)newTable, flags);
                MapPage(CurrentDirectory, (int)newTable, (int)newTable, flags);

                // Clear table
                Memory.Memset(newTable, 0, sizeof(PageTable));
            }

            // Set page
            PageTable* table = (PageTable*)(directory->tables[tableIndex] & 0xFFFFF000);
            table->pages[pageIndex & (1024 - 1)] = ToFrameAddress(phys) | flags;
        }

        /// <summary>
        /// Gets the physical address from a virtual address
        /// </summary>
        /// <param name="virt">The virtual address</param>
        /// <returns>The physical address</returns>
        public static unsafe void* GetPhysicalFromVirtual(void* virt)
        {
            // Get indices
            int address = (int)virt;
            int remaining = address % 0x1000;
            int frame = address / 0x1000;

            // Find corresponding table to the virtual address
            PageTable* table = (PageTable*)(CurrentDirectory->tables[frame / 1024] & 0xFFFFF000);
            if (table == null)
            {
                Panic.DoPanic("table == null");
                return null;
            }

            // Calculate page
            int page = table->pages[frame & (1024 - 1)];
            return (void*)(GetFrameAddress(page) + remaining);
        }

        /// <summary>
        /// Gets a page
        /// </summary>
        /// <param name="directory">The page directory</param>
        /// <param name="address">The address</param>
        /// <returns>The page</returns>
        public static unsafe int GetPage(PageDirectory* directory, int address)
        {
            return directory->tables[address & (1024 - 1)];
        }

        /// <summary>
        /// Sets the frame
        /// </summary>
        /// <param name="frame">Frame address</param>
        public static void SetFrame(int frame)
        {
            m_bitmap.SetBit(frame / 0x1000);
        }

        /// <summary>
        /// Clears the frame
        /// </summary>
        /// <param name="frame">Frame address</param>
        public static void ClearFrame(int frame)
        {
            m_bitmap.ClearBit(frame / 0x1000);
        }

        /// <summary>
        /// Allocates a free frame
        /// </summary>
        /// <returns>The free frame</returns>
        public static unsafe int AllocateFrame()
        {
            int free = m_bitmap.FindFirstFree(true);
            int address = free * 0x1000;
            return address;
        }

        /// <summary>
        /// Frees a frame
        /// </summary>
        /// <param name="page">The page</param>
        public static unsafe void FreeFrame(int page)
        {
            ClearFrame(GetFrameAddress(page));
        }

        /// <summary>
        /// Aligns the size or address to make it page aligned
        /// </summary>
        /// <param name="x">The size or address</param>
        /// <returns>The aligned size or address</returns>
        public static uint Align(uint x)
        {
            if ((x & 0x00000FFF) > 0)
            {
                x &= 0xFFFFF000;
                x += 0x1000;
            }

            return x;
        }

        /// <summary>
        /// Allocates a block of virtual memory
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The pointer to the block</returns>
        public static unsafe void* AllocateVirtual(int size)
        {
            // Page align size
            uint sizeAligned = Align((uint)size);

            // Allocate
            int free = m_bitmap.FindFirstFree();
            int start = free * 0x1000;
            int address = start;
            int end = (int)(address + sizeAligned);
            int flags = (int)PageFlags.Present | (int)PageFlags.Writable | (int)PageFlags.UserMode;
            while (address < end)
            {
                int phys = (int)PhysicalMemoryManager.Alloc();
                MapPage(CurrentDirectory, phys, address, flags);

                SetFrame(address);
                address += 0x1000;
            }

            return (void*)start;
        }

        /// <summary>
        /// Clones a page directory and its tables
        /// </summary>
        /// <param name="source">The source page directory</param>
        /// <returns>The cloned page directory</returns>
        public static unsafe PageDirectory* CloneDirectory(PageDirectory* source)
        {
            PageDirectory* destination = (PageDirectory*)PhysicalMemoryManager.Alloc();
            MapPage(CurrentDirectory, (int)destination, (int)destination, (int)PageFlags.Present | (int)PageFlags.Writable | (int)PageFlags.UserMode);
            Memory.Memset(destination, 0, sizeof(PageDirectory));

            if (destination == null)
            {
                Panic.DoPanic("Couldn't clone directory: destination==null");
                return null;
            }

            // Go through every page table
            for (int table = 0; table < 1024; table++)
            {
                // Get source
                int sourceTable = source->tables[table];
                if (sourceTable == 0)
                    continue;

                PageTable* sourceTablePtr = (PageTable*)(sourceTable & 0xFFFFF000);

                // Grab flags and allocate new table
                int flags = sourceTable & 0xFFF;
                PageTable* newTable = (PageTable*)PhysicalMemoryManager.Alloc();

                if (newTable == null)
                    Panic.DoPanic("newTable == null");

                MapPage(CurrentDirectory, (int)newTable, (int)newTable, flags);

                Memory.Memcpy(newTable, sourceTablePtr, sizeof(PageTable));

                destination->tables[table] = (int)newTable | flags;

                MapPage(destination, (int)newTable, (int)newTable, flags);
            }

            MapPage(destination, (int)destination, (int)destination, (int)PageFlags.Present | (int)PageFlags.Writable | (int)PageFlags.UserMode);

            return destination;
        }

        /// <summary>
        /// Frees a page directory
        /// </summary>
        /// <param name="directory">The directory</param>
        public static unsafe void FreeDirectory(PageDirectory* directory)
        {
            // Loop through every table of the directory
            for (int table = 0; table < 1024; table++)
            {
                // Get table
                int pageTable = directory->tables[table];
                if (pageTable == 0)
                    continue;

                // Grab pointer and free
                PageTable* pageTablePtr = (PageTable*)(pageTable & 0xFFFFF000);
                PhysicalMemoryManager.Free(pageTablePtr);
            }

            PhysicalMemoryManager.Free(directory);
        }

        /// <summary>
        /// Enables paging
        /// </summary>
        public static extern void Enable();

        /// <summary>
        /// Disables paging
        /// </summary>
        public static extern void Disable();

        /// <summary>
        /// Sets the current paging directory in the CR3 register
        /// </summary>
        /// <param name="directory">The page directory</param>
        private static unsafe extern void setDirectoryInternal(PageDirectory* directory);

        /// <summary>
        /// Reads the CR2 (pagefault address)
        /// </summary>
        /// <returns>The CR2 (faulting address)</returns>
        public static unsafe extern int ReadCR2();
    }
}
