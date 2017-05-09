using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.Arch
{
    public sealed unsafe class Paging
    {
        // Flags on a page
        public enum PageFlags
        {
            None = 0,
            Present = (1 << 0x0),
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            NoCache = (1 << 0x4)
        }

        // Flags on a table
        public enum TableFlags
        {
            None = 0,
            Present = (1 << 0x0),
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            WriteThrough = (1 << 0x3),
            NoCache = (1 << 0x4),
            SizeMegs = (1 << 0x7)
        }

        public struct PageTable
        {
            public fixed int Pages[1024];
        }

        public struct PageDirectory
        {
            public fixed int PhysicalTables[1024];
            public fixed int VirtualTables[1024];
            public PageDirectory* PhysicalDirectory;
        }

        // Faulting address of last pagefault (CR2)
        public static void* FaultingAddress { get; }

        // Kernel directory
        public static PageDirectory* KernelDirectory { get; private set; }

        // The current page directory (virtual address)
        public static PageDirectory* CurrentDirectory { get; private set; }

        // The current page directory (physical address)
        public static PageDirectory* CurrentDirectoryPhysical { get; private set; }

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
        public static void Init()
        {
            // Flags
            PageFlags kernelFlags = PageFlags.Present | PageFlags.Writable;
            PageFlags usercodeFlags = PageFlags.Present | PageFlags.UserMode;
            PageFlags userFlags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;

            // Bit array to store which frames are free
            bitmap = new BitArray(4096 * 1024 / 4);

            // Create a new page directory for the kernel
            // Note: At this point, virtual address == physical address due to identity mapping
            KernelDirectory = CreateNewDirectoryPhysically(userFlags);
            SetPageDirectory(KernelDirectory, KernelDirectory);

            // Identity map
            int end = (int)PhysicalMemoryManager.FirstFree();
            for (int address = 0; address < end; address += 0x1000)
            {
                MapPage(KernelDirectory, address, address, kernelFlags);
                bitmap.SetBit(address / 0x1000);
            }

            // Usercode is a section that is code in the kernel available to usermode
            int usercode = (int)getUsercodeAddress();
            MapPage(KernelDirectory, usercode, usercode, usercodeFlags);

            // Enable paging
            Enable();
        }

        /// <summary>
        /// Creates a new page directory using only physical memory (used in Init)
        /// </summary>
        /// <param name="flags">The flags</param>
        /// <returns>The page directory</returns>
        public static PageDirectory* CreateNewDirectoryPhysically(PageFlags flags)
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

                Memory.Memclear(table, sizeof(PageTable));

                // Note: At this point, virtual address == physical address due to identity mapping
                directory->PhysicalTables[i] = (int)table | (int)flags;
                directory->VirtualTables[i] = (int)table;
            }

            return directory;
        }

        /// <summary>
        /// Maps a page in a directory
        /// </summary>
        /// <param name="directory">The page directory</param>
        /// <param name="phys">The physical address</param>
        /// <param name="virt">The virtual address</param>
        /// <param name="flags">The flags</param>
        public static void MapPage(PageDirectory* directory, int phys, int virt, PageFlags flags)
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
        public static void* MapToVirtual(PageDirectory* directory, int phys, int size, PageFlags flags)
        {
            int sizeAligned = (int)AlignUp((uint)size) / 0x1000;
            int free = bitmap.FindFirstFreeRange(sizeAligned, true);
            int virt = free * 0x1000;
            
            for (int i = 0; i < sizeAligned; i++)
            {
                int offset = i * 0x1000;
                MapPage(directory, phys + offset, virt + offset, flags);
                PhysicalMemoryManager.Set(phys + offset);
            }
            Paging.setDirectoryInternal(Paging.CurrentDirectoryPhysical);
            return (void*)virt;
        }

        /// <summary>
        /// Gets the physical address from a virtual address
        /// </summary>
        /// <param name="pageDir">The page directory to look into</param>
        /// <param name="virt">The virtual address</param>
        /// <returns>The physical address</returns>
        public static void* GetPhysicalFromVirtual(PageDirectory* pageDir, void* virt)
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
        public static void* GetPhysicalFromVirtual(void* virt)
        {
            return GetPhysicalFromVirtual(CurrentDirectory, virt);
        }

        /// <summary>
        /// Aligns the size or address up to make it page aligned
        /// </summary>
        /// <param name="x">The size or address</param>
        /// <returns>The aligned size or address</returns>
        public static uint AlignUp(uint x)
        {
            if ((x & 0x00000FFF) > 0)
            {
                x &= 0xFFFFF000;
                x += 0x1000;
            }

            return x;
        }

        /// <summary>
        /// Aligns the size or address down to make it page aligned
        /// </summary>
        /// <param name="x">The size or address</param>
        /// <returns>The aligned size or address</returns>
        public static uint AlignDown(uint x)
        {
            return (x & 0xFFFFF000);
        }

        /// <summary>
        /// Allocates a virtual address range
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The pointer to the block</returns>
        public static void* AllocateVirtual(int size)
        {
            // Page align size
            uint sizeAligned = AlignUp((uint)size);

            // Allocate
            int free = bitmap.FindFirstFreeRange((int)(sizeAligned / 0x1000), true);
            int start = free * 0x1000;
            int end = (int)(start + sizeAligned);
            PageFlags flags = PageFlags.Present | PageFlags.Writable | PageFlags.UserMode;
            for (int address = start; address < end; address += 0x1000)
            {
                int phys = (int)PhysicalMemoryManager.Alloc();
                MapPage(KernelDirectory, phys, address, flags);
                MapPage(CurrentDirectory, phys, address, flags);
            }

            // Clear the data before returning it for safety
            Memory.Memclear((void*)start, size);

            return (void*)start;
        }
        
        /// <summary>
        /// Frees an allocate virtual address range and marks the corresponding physical addresses as free
        /// </summary>
        /// <param name="address">The starting address</param>
        /// <param name="size">The size</param>
        public static void UnMap(void* address, int size)
        {
            // Page align size
            uint sizeAligned = AlignUp((uint)size);
            int start = (int)AlignDown((uint)address) / 0x1000;
            int addressStart = (int)AlignDown((uint)address);
            int addressEnd = addressStart + (int)sizeAligned;
            
            // Free physical memory
            for (int i = addressStart; i < addressEnd; i += 0x1000)
            {
                void* phys = GetPhysicalFromVirtual((void*)i);
                PhysicalMemoryManager.Free(phys);
            }
            
            // Free virtual memory
            bitmap.ClearRange(start, (int)(sizeAligned / 0x1000));
        }

        /// <summary>
        /// Frees an allocate virtual address range
        /// </summary>
        /// <param name="address">The starting address</param>
        /// <param name="size">The size</param>
        public static void UnMapKeepPhysical(void* address, int size)
        {
            // Page align size
            uint sizeAligned = AlignUp((uint)size);
            int start = (int)AlignDown((uint)address) / 0x1000;
            
            // Free virtual memory
            bitmap.ClearRange(start, (int)(sizeAligned / 0x1000));
        }
        
        /// <summary>
        /// Clones a page directory and its tables
        /// </summary>
        /// <param name="source">The source page directory</param>
        /// <returns>The cloned page directory</returns>
        public static PageDirectory* CloneDirectory(PageDirectory* source)
        {
            // Note: sizeof(PageDirectory) is not neccesarily a page
            int pageDirSizeAligned = (int)AlignUp((uint)sizeof(PageDirectory));
            
            // One block for the page directory and the page tables
            int allocated = (int)Heap.AlignedAlloc(0x1000, pageDirSizeAligned + 1024 * sizeof(PageTable));
            if (allocated == 0)
                Panic.DoPanic("Couldn't clone page directory because there is no memory left");
            
            PageDirectory* destination = (PageDirectory*)allocated;
            destination->PhysicalDirectory = (PageDirectory*)GetPhysicalFromVirtual((void*)allocated);
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
                int newTablePhysical = (int)GetPhysicalFromVirtual(newTable);

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
        public static void FreeDirectory(PageDirectory* directory)
        {
            if (directory == CurrentDirectory)
                Panic.DoPanic("Tried to free the current page directory");

            if (directory == KernelDirectory)
                Panic.DoPanic("Tried to free the kernel page directory");
            
            // The directory was cloned, so it was allocated in one huge block
            PageDirectory* virt = CurrentDirectory;
            PageDirectory* phys = CurrentDirectoryPhysical;
            SetPageDirectory(KernelDirectory, KernelDirectory);
            Heap.Free(directory);
            SetPageDirectory(virt, phys);
        }

        /// <summary>
        /// Sets the page directory
        /// </summary>
        /// <param name="directoryVirtual">The virtual address of the page directory</param>
        /// <param name="directoryPhysical">The physical address of the page directory</param>
        public static void SetPageDirectory(PageDirectory* directoryVirtual, PageDirectory* directoryPhysical)
        {
            /** 
             * Note: This is a way to solve the issue that a page directory is not available
             * in every other page directory (due to security).
             * So: upon a task switch, setting a directory physically works for the CPU
             * but we also need its virtual address.
             * Setting the virtual address with a getter and updating the CR3 with the physical address
             * also wont work, because it's not guaranteed you can read the page directory
             * as it may not be available in the current task.
             */

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
        private static extern void setDirectoryInternal(PageDirectory* directory);

        /// <summary>
        /// Gets the usercode section address
        /// </summary>
        /// <returns>Usercode section address</returns>
        public static extern void* getUsercodeAddress();
    }
}
