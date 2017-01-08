using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.MultiTasking;

namespace Sharpen.Arch
{
    public unsafe class X86Context : IContext
    {
        // These can be found in the GDT
        private const int KernelCS = 0x08;
        private const int KernelDS = 0x10;
        private const int UserspaceCS = 0x1B;
        private const int UserspaceDS = 0x23;

        private const int KernelStackSize = 4 * 1024;
        private const int UserStackSize = 16 * 1024;

        // It's not always the case we can access the PhysicalAddress field of a page directory
        // because it may be unmapped, so we have a reference here
        public Paging.PageDirectory* PageDirVirtual { get; private set; }
        private Paging.PageDirectory* PageDirPhysical;

        private int* m_stack;
        private int* m_stackStart;
        private int* m_kernelStack;
        private int* m_kernelStackStart;
        private void* m_FPUContext;
        private Regs* m_sysRegs;

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
            Heap.Free(m_FPUContext);
            Heap.Free(m_kernelStackStart);

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
        /// Writes a scheduler stack
        /// </summary>
        /// <param name="ptr">The pointer to the stack</param>
        /// <param name="cs">The Code Segment</param>
        /// <param name="ds">The Data Segment</param>
        /// <param name="eip">The return EIP value</param>
        /// <returns>The new pointer</returns>
        private unsafe int* writeSchedulerStack(int* ptr, int cs, int ds, void* eip)
        {
            int esp = (int)ptr;

            // Data pushed by CPU
            *--ptr = ds;        // Data Segment
            *--ptr = esp;       // Old stack
            *--ptr = 0x202;     // EFLAGS
            *--ptr = cs;        // Code Segment
            *--ptr = (int)eip;  // Initial EIP

            // Pushed by pusha
            *--ptr = 0;         // EAX
            *--ptr = 0;         // ECX
            *--ptr = 0;         // EDX
            *--ptr = 0;         // EBX
            --ptr;
            *--ptr = 0;         // EBP
            *--ptr = 0;         // ESI
            *--ptr = 0;         // EDI

            // Data segments
            *--ptr = ds;        // DS
            *--ptr = ds;        // ES
            *--ptr = ds;        // FS
            *--ptr = ds;        // GS

            // New location of stack
            return ptr;
        }

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="eip">The initial instruction pointer</param>
        /// <param name="initialStackSize">Initial stack size</param>
        /// <param name="initialStack">Initial stack data</param>
        /// <param name="kernelContext">If this is a kernel context or not</param>
        public void CreateNewContext(void* eip, int initialStackSize, int[] initialStack, bool kernelContext)
        {
            // Stack
            int* stacks = (int*)Heap.AlignedAlloc(16, KernelStackSize + UserStackSize);
            m_stackStart = (int*)((int)stacks + KernelStackSize);
            m_stack = (int*)((int)m_stackStart + UserStackSize);

            // Copy initial stack
            if (initialStackSize > 0)
            {
                for (int i = 0; i < initialStackSize; i++)
                {
                    *--m_stack = initialStack[i];
                }
            }

            // Descriptors from the GDT
            int cs = UserspaceCS;
            int ds = UserspaceDS;
            if (kernelContext)
            {
                cs = KernelCS;
                ds = KernelDS;
            }

            // Continue with stacks
            m_stack = writeSchedulerStack(m_stack, cs, ds, eip);
            m_kernelStackStart = stacks;
            m_kernelStack = (int*)((int)m_kernelStackStart + KernelStackSize);

            // FPU context
            m_FPUContext = Heap.AlignedAlloc(16, 512);
            FPU.StoreContext(m_FPUContext);

            // Paging
            PageDirVirtual = Paging.CloneDirectory(Paging.CurrentDirectory);
            PageDirPhysical = PageDirVirtual->PhysicalDirectory;
        }

        /// <summary>
        /// Stores the current context state
        /// </summary>
        /// <param name="ptr">The register pointer</param>
        public void StoreContext(void* ptr)
        {
            m_stack = (int*)ptr;
            m_kernelStack = (int*)GDT.TSS_Entry->ESP0;
            FPU.StoreContext(m_FPUContext);
        }

        /// <summary>
        /// Restores a previous state of this context
        /// </summary>
        /// <returns>The pointer of the stack</returns>
        public void* RestoreContext()
        {
            Paging.SetPageDirectory(PageDirVirtual, PageDirPhysical);
            FPU.RestoreContext(m_FPUContext);
            GDT.TSS_Entry->ESP0 = (uint)m_kernelStack;
            return m_stack;
        }

        /// <summary>
        /// Clones a context from another context
        /// </summary>
        /// <param name="context">The source context</param>
        public void CloneFrom(IContext context)
        {
            X86Context source = (X86Context)context;

            // Stack
            int* stacks = (int*)Heap.AlignedAlloc(16, KernelStackSize + UserStackSize);
            m_stackStart = (int*)((int)stacks + KernelStackSize);
            m_kernelStackStart = stacks;

            Memory.Memcpy(m_kernelStackStart, source.m_kernelStackStart, KernelStackSize + UserStackSize);

            int diffStack = (int)source.m_stack - (int)source.m_stackStart;
            int diffKernelStack = (int)source.m_kernelStack - (int)source.m_kernelStackStart;

            m_stack = (int*)((int)m_stackStart + diffStack);
            m_kernelStack = (int*)((int)m_kernelStackStart + diffKernelStack);

            // FPU context
            m_FPUContext = Heap.AlignedAlloc(16, 512);
            Memory.Memcpy(m_FPUContext, source.m_FPUContext, 512);

            // Paging
            PageDirVirtual = Paging.CloneDirectory(source.PageDirVirtual);
            PageDirPhysical = PageDirVirtual->PhysicalDirectory;
        }

        /// <summary>
        /// Creates a kernel context
        /// </summary>
        public void CreateKernelContext()
        {
            PageDirVirtual = Paging.KernelDirectory;
            PageDirPhysical = Paging.KernelDirectory;
            m_FPUContext = Heap.AlignedAlloc(16, 512);
        }

        /// <summary>
        /// Updates syscall registers
        /// </summary>
        /// <param name="ptr">Pointer to syscall registers</param>
        public void UpdateSyscallRegs(void* ptr)
        {
            m_sysRegs = (Regs*)ptr;
        }

        /// <summary>
        /// Sets the return value from a syscall
        /// </summary>
        /// <param name="ret">The return value</param>
        public void SetSysReturnValue(int ret)
        {
            m_sysRegs->EAX = ret;
        }

        /// <summary>
        /// Completes the fork
        /// </summary>
        /// <param name="diffRegs"></param>
        /// <param name="diffESP"></param>
        protected void completeFork(int diffRegs, int diffESP)
        {
            // Update stack references within the stack itself
            m_sysRegs = (Regs*)((int)m_stackStart + diffRegs);
            m_sysRegs->ESP = (int)m_stackStart + diffESP;
        }

        /// <summary>
        /// Forks the context
        /// </summary>
        /// <returns>The new PID or zero</returns>
        public int Fork()
        {
            int diffRegs = (int)m_sysRegs - (int)m_stackStart;
            int diffESP = m_sysRegs->ESP - (int)m_stackStart;

            int pid = Tasking.Fork();

            // Complete fork
            X86Context currentContext = (X86Context)Tasking.CurrentTask.Context;
            currentContext.completeFork(diffRegs, diffESP);
            
            return pid;
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
            Paging.FreeVirtual(Address, Size);
        }
    }
}
