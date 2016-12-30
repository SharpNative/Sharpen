using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Arch
{
    public sealed class Paging
    {
        // TODO: (VERIFY THIS) set temp address to used in frames
        // TODO: use one big block of memory in the page cloning routine for extra speed and security

        // Last page entry in last table
        // TODO: update this, the real value we want doesn't work because of overflowing numbers, great!
        private const uint TEMP_MAP_ADDRESS = 0x7FFFF000;//0xFFFFF000;

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
            // Flags for kernel pages
            PageFlags flags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;

            // Bit array to store which frames are free
            // One entry holds 0x1000 * 32 bytes
            m_bitmap = new BitArray((int)(memSize * 1024 / 0x1000 / 32));

            // Create a new page directory for the kernel
            KernelDirectory = CreateNewDirectoryPhysically(flags);
            CurrentDirectory = KernelDirectory;

            // Make the directory available
            MapPage(KernelDirectory, (int)KernelDirectory, (int)KernelDirectory, flags);
            for (int i = 0; i < 1024; i++)
            {
                MapPage(KernelDirectory, (int)(KernelDirectory->tables[i] & 0xFFFFF000), (int)(KernelDirectory->tables[i] & 0xFFFFF000), flags);
            }

            // Mark the used memory as used
            int address = 0;
            while (address < (uint)PhysicalMemoryManager.First())
            {
                MapPage(KernelDirectory, address, address, flags);
                SetFrame(address);
                address += 0x1000;
            }

            // Enable paging
            Enable();
        }

        /// <summary>
        /// Creates a new page directory using only physical memory (used in Init)
        /// </summary>
        /// <param name="flags">The flags</param>
        /// <returns>The page directory</returns>
        public static unsafe PageDirectory* CreateNewDirectoryPhysically(PageFlags flags)
        {
            // Allocate a new block of physical memory to store our physical page in
            PageDirectory* directory = (PageDirectory*)PhysicalMemoryManager.Alloc();
            if (directory == null)
                Panic.DoPanic("directory == null");

            // Allocate the tables
            for (int i = 0; i < 1024; i++)
            {
                PageTable* table = (PageTable*)PhysicalMemoryManager.Alloc();
                if (table == null)
                    Panic.DoPanic("table == null");

                Memory.Memset(table, 0, sizeof(PageTable));

                directory->tables[i] = (int)table | (int)flags;
            }

            return directory;
        }

        /// <summary>
        /// Maps a page in a directory
        /// </summary>
        /// <param name="directory">The paging directory</param>
        /// <param name="phys">The physical address</param>
        /// <param name="virt">The virtual address</param>
        /// <param name="flags">The flags</param>
        public static unsafe void MapPage(PageDirectory* directory, int phys, int virt, PageFlags flags)
        {
            // Get indices
            int pageIndex = virt / 0x1000;
            int tableIndex = pageIndex / 1024;

            // Set page
            PageTable* table = (PageTable*)(directory->tables[tableIndex] & 0xFFFFF000);
            table->pages[pageIndex & (1024 - 1)] = ToFrameAddress(phys) | (int)flags;
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
                Panic.DoPanic("table == null");

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
            PageFlags flags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;
            while (address < end)
            {
                int phys = (int)PhysicalMemoryManager.Alloc();

                MapPage(CurrentDirectory, phys, address, flags);
                //MapPage(KernelDirectory, phys, address, flags);

                SetFrame(address);
                address += 0x1000;
            }

            // Clear the data before returning it for safety
            Memory.Memset((void*)start, 0, size);

            return (void*)start;
        }

        /// <summary>
        /// Temporarily map an address to make it available (mapped to TEMP_MAP_ADDRESS)
        /// </summary>
        /// <param name="address">The address to make available</param>
        private static unsafe void mapTemporarily(void* address)
        {
            // To access memory that has not been mapped already
            // (because of situations like cloning page directories)
            // we can keep one entry free to temporarily map an address in kernelspace
            // See https://github.com/littleosbook/littleosbook/blob/master/page_frame_allocation.md for a more detailed explanation

            PageFlags flags = PageFlags.Present | PageFlags.Writable;
            MapPage(KernelDirectory, (int)address, (int)(void*)TEMP_MAP_ADDRESS, flags);
        }

        /// <summary>
        /// Clones a page directory and its tables
        /// </summary>
        /// <param name="source">The source page directory</param>
        /// <returns>The cloned page directory</returns>
        public static unsafe PageDirectory* CloneDirectory(PageDirectory* source)
        {
            // One block for the directory and its tables
            int allocated = (int)Heap.AlignedAlloc(0x1000, sizeof(PageDirectory) + 1024 * sizeof(PageTable));
            if (allocated == 0)
                Panic.DoPanic("Couldn't clone a new directory because there is no memory");

            PageDirectory* destination = (PageDirectory*)allocated;
            for (int i = 0; i < 1024; i++)
            {
                int sourceTable = source->tables[i];
                if (sourceTable == 0)
                    Panic.DoPanic("sourceTable == 0?!");

                // Get the pointer without the flags
                PageTable* sourceTablePtr = (PageTable*)(sourceTable & 0xFFFFF000);
                // Grab flags
                int flags = sourceTable & 0xFFF;

                PageTable* newTable = (PageTable*)(allocated + sizeof(PageDirectory) + i * sizeof(PageTable));
                Memory.Memcpy(newTable, sourceTablePtr, sizeof(PageTable));
                destination->tables[i] = (int)newTable | flags;
            }

            return destination;
        }

        /// <summary>
        /// Frees a page directory
        /// </summary>
        /// <param name="directory">The directory</param>
        public static unsafe void FreeDirectory(PageDirectory* directory)
        {
            // The directory was cloned, so it was allocated in one huge block
            // TODO
            //Heap.Free(directory);
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
