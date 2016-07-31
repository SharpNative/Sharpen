using Sharpen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    public sealed class Paging
    {
        // Flags on a page
        enum PageFlags
        {
            Present = 1,
            Writable = (1 << 0x1),
            UserMode = (1 << 0x2),
            NoCache = (1 << 0x4)
        }

        // Flags on a table
        enum TableFlags
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
                SetDirectoryInternal(value);
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
        public static int FrameAddress(int a)
        {
            return (a << 0xC);
        }

        /// <summary>
        /// Gets the frame address of the page
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetFrameAddress(int a)
        {
            return (a >> 0xC);
        }

        #endregion

        /// <summary>
        /// Initializes paging
        /// </summary>
        public static unsafe void Init()
        {
            // Create kernel directory
            KernelDirectory = (PageDirectory*)Heap.AlignedAlloc(0x1000, sizeof(PageDirectory));
            Memory.Memset(KernelDirectory, 0, sizeof(PageDirectory));

            // Bit array to store which frames are free
            int memsize = 0x1000000; // Assume 16MB for now
            m_bitmap = new BitArray((memsize / 0x1000) / 32);

            // Map from [ 0x00 - Heap end ] as kernelspace
            int address = 0;
            int end = (int)Heap.CurrentEnd + (1024 * 1024);
            while (address < end)
            {
                int flags = (int)PageFlags.Present | (int)PageFlags.Writable;
                MapPage(KernelDirectory, address, address, flags);
                AllocateFrame(GetPage(KernelDirectory, address), flags);
                address += 0x1000;
            }

            // Enable paging
            CurrentDirectory = KernelDirectory;
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
                PageTable* newTable = (PageTable*)Heap.AlignedAlloc(0x1000, sizeof(PageTable));
                if (newTable == null)
                    return;

                Memory.Memset(newTable, 0, sizeof(PageTable));

                // Set flags
                int flaggedTable = (int)newTable | (int)TableFlags.Present | (int)TableFlags.Writable;
                directory->tables[tableIndex] = flaggedTable;
            }

            // Set page
            PageTable* table = (PageTable*)(directory->tables[tableIndex] & 0xFFFFF000);
            table->pages[pageIndex & (1024 - 1)] = FrameAddress(phys / 0x1000) | flags;
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
                return null;

            // Calculate page
            int page = table->pages[frame & (1024 - 1)];
            Console.WriteNum(page);
            Console.Write(" << page ");
            Console.WriteHex((int)table);
            return (void*)(GetFrameAddress(page) * 0x1000 + remaining);
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
        /// Allocate a frame
        /// </summary>
        /// <param name="page">The page (physical)</param>
        /// <param name="flags">The flags</param>
        public static unsafe void AllocateFrame(int page, int flags)
        {
            int free = m_bitmap.FindFirstFree();
            int address = free * 0x1000;
            SetFrame(address);
            MapPage(CurrentDirectory, address, (int)(page & 0xFFFFF000), flags);
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
        /// Allocates a block of physical memory
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The pointer to the physical memory</returns>
        public static unsafe void* AllocatePhysical(int size)
        {
            // Page align size
            uint sizeAligned = (uint)size;
            if (sizeAligned % 0x1000 != 0)
            {
                sizeAligned &= 0xFFFFF000;
                sizeAligned += 0x1000;
            }

            // Allocate
            int free = m_bitmap.FindFirstFree();
            int start = free * 0x1000;
            int address = start;
            int end = (int)(address + sizeAligned);
            while (address < end)
            {
                int flags = (int)PageFlags.Present | (int)PageFlags.Writable;
                MapPage(KernelDirectory, address, address, flags);
                AllocateFrame(GetPage(KernelDirectory, address), flags);
                address += 0x1000;
            }

            return (void*)start;
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
        private static unsafe extern void SetDirectoryInternal(PageDirectory* directory);
    }
}
