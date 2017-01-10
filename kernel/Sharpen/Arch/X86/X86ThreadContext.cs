using Sharpen.Mem;
using Sharpen.MultiTasking;

namespace Sharpen.Arch.X86
{
    unsafe class X86ThreadContext : IThreadContext
    {
        // These can be found in the GDT
        private const int KernelCS = 0x08;
        private const int KernelDS = 0x10;
        private const int UserspaceCS = 0x1B;
        private const int UserspaceDS = 0x23;

        // Stacks
        private const int KernelStackSize = 4 * 1024;
        private const int UserStackSize = 16 * 1024;
        
        private int* m_stack;
        private int* m_stackStart;
        private int* m_kernelStack;
        private int* m_kernelStackStart;
        private void* m_FPUContext;
        private Regs* m_sysRegs;

        /// <summary>
        /// Cleans the thread context
        /// </summary>
        public void Cleanup()
        {
            Heap.Free(m_FPUContext);
            Heap.Free(m_kernelStackStart);
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
            FPU.RestoreContext(m_FPUContext);
            GDT.TSS_Entry->ESP0 = (uint)m_kernelStack;
            return m_stack;
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
            int cs = (kernelContext ? KernelCS : UserspaceCS);
            int ds = (kernelContext ? KernelDS : UserspaceDS);

            // Continue with stacks
            m_stack = writeSchedulerStack(m_stack, cs, ds, eip);
            m_kernelStackStart = stacks;
            m_kernelStack = (int*)((int)m_kernelStackStart + KernelStackSize);

            // FPU context
            m_FPUContext = Heap.AlignedAlloc(16, 512);
            FPU.StoreContext(m_FPUContext);
        }

        /// <summary>
        /// Clones the context from another thread
        /// </summary>
        /// <param name="context">The other thread context</param>
        public void CloneFrom(IThreadContext context)
        {
            X86ThreadContext source = (X86ThreadContext)context;

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

            // Update stack references within the system stack itself
            int diffRegs = (int)source.m_sysRegs - (int)source.m_stackStart;
            int diffESP = source.m_sysRegs->ESP - (int)source.m_stackStart;
            m_sysRegs = (Regs*)((int)m_stackStart + diffRegs);
            m_sysRegs->ESP = (int)m_stackStart + diffESP;
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
    }
}
