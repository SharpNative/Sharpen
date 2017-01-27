using Sharpen.Exec;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Utilities;

namespace Sharpen.Arch.X86
{
    unsafe class X86ThreadContext : IThreadContext
    {
        // These can be found in the GDT
        public const int KernelCS = 0x08;
        public const int KernelDS = 0x10;
        public const int UserspaceCS = 0x1B;
        public const int UserspaceDS = 0x23;

        // Stack sizes
        public const int KernelStackSize = 4 * 1024;
        public const int UserStackSize = 16 * 1024;
        
        private int* m_stack;
        private int* m_stackStart;
        private int* m_kernelStack;
        private int* m_kernelStackStart;
        private void* m_FPUContext;
        private RegsDirect* m_sysRegs;

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
            m_sysRegs = (RegsDirect*)ptr;
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
            createStacks();

            // Copy initial stack
            for (int i = 0; i < initialStackSize; i++)
            {
                *--m_stack = initialStack[i];
            }

            // Descriptors from the GDT
            int cs = (kernelContext ? KernelCS : UserspaceCS);
            int ds = (kernelContext ? KernelDS : UserspaceDS);

            // Continue with stacks
            m_stack = writeSchedulerStack(m_stack, m_stack, cs, ds, eip);
            
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
            createStacks();

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
            m_sysRegs = (RegsDirect*)((int)m_stackStart + diffRegs);
            m_sysRegs->ESP = (int)m_stackStart + diffESP;

            // Write stack
            m_stack = writeSchedulerStack(m_stack, (void*)m_sysRegs->ESP, UserspaceCS, UserspaceDS, (void*)m_sysRegs->EIP);
            RegsDirect* ptr = (RegsDirect*)m_stack;
            ptr->EBX = m_sysRegs->EBX;
            ptr->ECX = m_sysRegs->ECX;
            ptr->EDX = m_sysRegs->EDX;
            ptr->EBP = m_sysRegs->EBP;
            ptr->ESI = m_sysRegs->ESI;
            ptr->EDI = m_sysRegs->EDI;
        }

        /// <summary>
        /// Processes a signal
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The signal context</returns>
        public ISignalContext ProcessSignal(SignalAction action)
        {
            X86SignalContext context = new X86SignalContext();

            // Push arguments
            *--context.Stack = 0; // Context (unused)
            *--context.Stack = 0x7331; // TODO: siginfo
            *--context.Stack = action.SignalNumber;

            // Return address of signal handling method
            *--context.Stack = (int)Util.MethodToPtr(SignalAction.ReturnFromSignal);

            // Backup regs so we can restore later
            Memory.Memcpy(context.Sysregs, m_sysRegs, sizeof(RegsDirect));

            // Modify regs to return to
            m_sysRegs->ESP = (int)context.Stack;
            m_sysRegs->EIP = (int)action.Sigaction.Handler;
            m_sysRegs->EAX = 0;
            m_sysRegs->EBX = 0;
            m_sysRegs->ECX = 0;
            m_sysRegs->EDX = 0;
            m_sysRegs->ESI = 0;
            m_sysRegs->EDI = 0;

            return (ISignalContext)context;
        }

        /// <summary>
        /// Returns from signal (restores original context)
        /// </summary>
        /// <param name="oldContext">Old context</param>
        public void ReturnFromSignal(ISignalContext oldContext)
        {
            X86SignalContext old = (X86SignalContext)oldContext;
            Memory.Memcpy(m_sysRegs, old.Sysregs, sizeof(RegsDirect));
        }

        /// <summary>
        /// Writes a scheduler stack
        /// </summary>
        /// <param name="ptr">The pointer to the stack to write to</param>
        /// <param name="esp">The pointer to the stack to restore to</param>
        /// <param name="cs">The Code Segment</param>
        /// <param name="ds">The Data Segment</param>
        /// <param name="eip">The return EIP value</param>
        /// <returns>The new pointer</returns>
        private unsafe int* writeSchedulerStack(int* ptr, void* esp, int cs, int ds, void* eip)
        {
            // Data pushed by CPU
            *--ptr = ds;        // Data Segment
            *--ptr = (int)esp;  // Stack to restore to
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
        /// Sets up the stacks
        /// </summary>
        private void createStacks()
        {
            int* stacks = (int*)Heap.AlignedAlloc(16, KernelStackSize + UserStackSize);
            m_stackStart = (int*)((int)stacks + KernelStackSize);
            m_stack = (int*)((int)m_stackStart + UserStackSize);

            m_kernelStackStart = stacks;
            m_kernelStack = (int*)((int)m_kernelStackStart + KernelStackSize);
        }
    }
}
