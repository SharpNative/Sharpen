using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.MultiTasking;

namespace Sharpen.Arch
{
    public unsafe class X86Context : IContext
    {
        // It's not always the case we can access the PhysicalAddress field of a page directory
        // because it may be unmapped, so we have a reference here
        public Paging.PageDirectory* PageDirVirtual { get; private set; }
        private Paging.PageDirectory* PageDirPhysical;

        private List m_virtualAddresses;

        /// <summary>
        /// Creates a new blank context
        /// </summary>
        public X86Context()
        {
            m_virtualAddresses = new List();
        }

        /// <summary>
        /// Cleans up the context
        /// </summary>
        public void Cleanup()
        {
            int count = m_virtualAddresses.Count;
            for (int i = 0; i < count; i++)
            {
                VirtualAddressRange range = (VirtualAddressRange)m_virtualAddresses.Item[i];
                range.Dispose();
            }

            m_virtualAddresses.Dispose();
            Heap.Free(m_virtualAddresses);

            Paging.FreeDirectory(PageDirVirtual);
        }

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="kernelContext">If this is a kernel context</param>
        public void CreateNewContext(bool kernelContext)
        {
            if (kernelContext)
            {
                PageDirVirtual = Paging.KernelDirectory;
                PageDirPhysical = Paging.KernelDirectory;
            }
            else
            {
                PageDirVirtual = Paging.CloneDirectory(Paging.CurrentDirectory);
                PageDirPhysical = PageDirVirtual->PhysicalDirectory;
            }
        }

        /// <summary>
        /// Clones a context from another context
        /// </summary>
        /// <param name="context">The source context</param>
        public void CloneFrom(IContext context)
        {
            X86Context source = (X86Context)context;
            PageDirVirtual = Paging.CloneDirectory(source.PageDirVirtual);
            PageDirPhysical = PageDirVirtual->PhysicalDirectory;
        }

        /// <summary>
        /// Increases the virtual address data space
        /// </summary>
        /// <param name="size">The size to increase with</param>
        /// <returns>The old end</returns>
        public unsafe void* Sbrk(int size)
        {
            VirtualAddressRange range = new VirtualAddressRange(size);
            m_virtualAddresses.Add(range);
            return range.Address;
        }

        /// <summary>
        /// Prepares this context
        /// </summary>
        public void PrepareContext()
        {
            Paging.SetPageDirectory(PageDirVirtual, PageDirPhysical);
        }
    }

    unsafe class VirtualAddressRange
    {
        public void* Address { get; private set; }
        public int Size { get; private set; }

        /// <summary>
        /// Creates a new range for virtual address
        /// </summary>
        /// <param name="size">The size of the range</param>
        public VirtualAddressRange(int size)
        {
            Address = Paging.AllocateVirtual(size);
            Size = size;
        }

        /// <summary>
        /// Gives the range back to the system
        /// </summary>
        public void Dispose()
        {
            Paging.UnMap(Address, Size);
        }
    }
}
