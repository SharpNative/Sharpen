using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.Arch
{
    public sealed class Paging
    {
        // TODO: mutexes?

        // Last page entry in last table is kept free (see mapTemporarily)
        private const uint TEMP_MAP_ADDRESS = 0xFFFFF000;

        // Flags on a page
        public enum PageFlags
        {
            Present = (1 << 0x0),
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            NoCache = (1 << 0x4)
        }

        // Flags on a table
        public enum TableFlags
        {
            Present = (1 << 0x0),
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            WriteThrough = (1 << 0x3),
            NoCache = (1 << 0x4),
            SizeMegs = (1 << 0x7)
        }

        public unsafe struct PageTable
        {
            public fixed int Pages[1024];
        }

        public unsafe struct PageDirectory
        {
            public fixed int PhysicalTables[1024];
            public fixed int VirtualTables[1024];
            public PageDirectory* PhysicalDirectory;
        }

        // Kernel directory
        public static unsafe PageDirectory* KernelDirectory { get; private set; }
        
        // The current page directory (virtual address)
        public static unsafe PageDirectory* CurrentDirectory { get; private set; }

        // The current page directory (physical address)
        public static unsafe PageDirectory* CurrentDirectoryPhysical { get; private set; }

        private static BitArray bitmap;

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
            // Flags
            PageFlags kernelFlags = PageFlags.Present | PageFlags.Writable;
            PageFlags userFlags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;

            // Bit array to store which frames are free
            bitmap = new BitArray((int)(memSize * 1024 / 4 / 32));
            unchecked
            {
                SetFrame((int)TEMP_MAP_ADDRESS);
            }

            // Create a new page directory for the kernel
            // Note: At this point, virtual address == physical address due to identity mapping
            KernelDirectory = CreateNewDirectoryPhysically(userFlags);
            SetPageDirectory(KernelDirectory, KernelDirectory);

            // Mark the used memory as used
            int address = 0;
            while (address < (int)PhysicalMemoryManager.FirstFree())
            {
                MapPage(KernelDirectory, address, address, kernelFlags);
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
            PageDirectory* directory = (PageDirectory*)Heap.AlignedAlloc(0x1000, sizeof(PageDirectory));
            directory->PhysicalDirectory = directory;
            if (directory == null)
                Panic.DoPanic("directory == null");

            // Allocate the tables
            for (int i = 0; i < 1024; i++)
            {
                PageTable* table = (PageTable*)PhysicalMemoryManager.Alloc();
                if (table == null)
                    Panic.DoPanic("table == null");

                Memory.Memset(table, 0, sizeof(PageTable));

                // Note: At this point, virtual address == physical address due to identity mapping
                directory->PhysicalTables[i] = (int)table | (int)flags;
                directory->VirtualTables[i] = (int)table;
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

            // Set page using its virtual address
            PageTable* table = (PageTable*)directory->VirtualTables[tableIndex];
            table->Pages[pageIndex & (1024 - 1)] = ToFrameAddress(phys) | (int)flags;
        }

        /// <summary>
        /// Maps a physical address (range) to a free virtual address (range)
        /// </summary>
        /// <param name="directory">The page directory</param>
        /// <param name="phys">The physical address</param>
        /// <param name="size">The size of the range</param>
        /// <param name="flags">The flags</param>
        /// <returns>The virtual address</returns>
        public static unsafe void* MapToVirtual(PageDirectory* directory, int phys, int size, PageFlags flags)
        {
            int sizeAligned = (int)Align((uint)size) / 0x1000;
            int free = bitmap.FindFirstFreeRange(size, true);
            int virt = free * 0x1000;

            for (int i = 0; i < sizeAligned; i++)
            {
                int offset = i * 0x1000;
                MapPage(directory, phys + offset, virt + offset, flags);
            }

            return (void*)virt;
        }

        /// <summary>
        /// Gets the physical address from a virtual address
        /// </summary>
        /// <param name="pageDir">The page directory to look into</param>
        /// <param name="virt">The virtual address</param>
        /// <returns>The physical address</returns>
        public static unsafe void* GetPhysicalFromVirtual(PageDirectory* pageDir, void* virt)
        {
            // Get indices
            int address = (int)virt;
            int remaining = address % 0x1000;
            int frame = address / 0x1000;

            // Find corresponding table to the virtual address
            PageTable* table = (PageTable*)pageDir->VirtualTables[frame / 1024];
            if (table == null)
                Panic.DoPanic("table == null");

            // Calculate page
            int page = table->Pages[frame & (1024 - 1)];
            return (void*)(GetFrameAddress(page) + remaining);
        }

        /// <summary>
        /// Gets the physical address from a virtual address
        /// </summary>
        /// <param name="virt">The virtual address</param>
        /// <returns>The physical address</returns>
        public static unsafe void* GetPhysicalFromVirtual(void* virt)
        {
            return GetPhysicalFromVirtual(CurrentDirectory, virt);
        }

        /// <summary>
        /// Sets the frame
        /// </summary>
        /// <param name="frame">Frame address</param>
        public static void SetFrame(int frame)
        {
            bitmap.SetBit(frame / 0x1000);
        }

        /// <summary>
        /// Clears the frame
        /// </summary>
        /// <param name="frame">Frame address</param>
        public static void ClearFrame(int frame)
        {
            bitmap.ClearBit(frame / 0x1000);
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
            int free = bitmap.FindFirstFreeRange((int)(sizeAligned / 0x1000), true);
            int start = free * 0x1000;
            int address = start;
            int end = (int)(address + sizeAligned);
            PageFlags flags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;
            while (address < end)
            {
                int phys = (int)PhysicalMemoryManager.Alloc();
                MapPage(CurrentDirectory, phys, address, flags);
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
            // Note: sizeof(PageDirectory) is not neccesarily a page
            int pageDirSizeAligned = (int)Align((uint)sizeof(PageDirectory));

            // One block for the page directory and the page tables
            int allocated = (int)AllocateVirtual(pageDirSizeAligned + 1024 * sizeof(PageTable));
            if (allocated == 0)
                Panic.DoPanic("Couldn't clone page directory because there is no memory left");

            int allocatedPhysical = (int)GetPhysicalFromVirtual((void*)allocated);

            PageDirectory* destination = (PageDirectory*)allocated;
            destination->PhysicalDirectory = (PageDirectory*)allocatedPhysical;
            for (int i = 0; i < 1024; i++)
            {
                int sourceTable = source->VirtualTables[i];
                if (sourceTable == 0)
                    Panic.DoPanic("sourceTable == 0?!");

                // Get the pointer without the flags and the flags seperately
                PageTable* sourceTablePtr = (PageTable*)sourceTable;
                int flags = source->PhysicalTables[i] & 0xFFF;

                // Calculate addresses
                int addressOffset = pageDirSizeAligned + i * sizeof(PageTable);
                PageTable* newTable = (PageTable*)(allocated + addressOffset);
                int newTablePhysical = allocatedPhysical + addressOffset;

                // Copy table data and set pointers
                Memory.Memcpy(newTable, sourceTablePtr, sizeof(PageTable));
                destination->PhysicalTables[i] = newTablePhysical | flags;
                destination->VirtualTables[i] = (int)newTable;
            }

            return destination;
        }

        /// <summary>
        /// Frees a page directory
        /// </summary>
        /// <param name="directory">The directory</param>
        public static unsafe void FreeDirectory(PageDirectory* directory)
        {
            if (directory == CurrentDirectory)
                Panic.DoPanic("Tried to free the current page directory");

            if (directory == KernelDirectory)
                Panic.DoPanic("Tried to free the kernel page directory");

            // The directory was cloned, so it was allocated in one huge block
            Heap.Free(directory);
        }

        /// <summary>
        /// Sets the page directory
        /// </summary>
        /// <param name="directoryVirtual">The virtual address of the page directory</param>
        /// <param name="directoryPhysical">The physical address of the page directory</param>
        public static unsafe void SetPageDirectory(PageDirectory* directoryVirtual, PageDirectory* directoryPhysical)
        {
            // Note: This is a way to solve the issue that a page directory is not available
            //       in every other page directory (due to security).
            //       So: upon a task switch, setting a directory physically works for the CPU
            //       but we also need its virtual address.
            //       Setting the virtual address with a getter and updating the CR3 with the physical address
            //       also wont work, because it's not guaranteed you can read the page directory
            //       as it may not be available in the current task.
            CurrentDirectory = directoryVirtual;
            CurrentDirectoryPhysical = directoryPhysical;
            setDirectoryInternal(directoryPhysical);
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
