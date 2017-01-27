using Sharpen.Mem;
using Sharpen.MultiTasking;

namespace Sharpen.Arch.X86
{
    unsafe sealed class X86SignalContext : ISignalContext
    {
        public int* Stack { get; set; }
        public RegsDirect* Sysregs { get; set; }

        private int* m_stackStart;

        /// <summary>
        /// Creates a new signal context
        /// </summary>
        public X86SignalContext()
        {
            m_stackStart = (int*)Heap.AlignedAlloc(16, X86ThreadContext.UserStackSize);
            Stack = (int*)((int)m_stackStart + X86ThreadContext.UserStackSize);
            Sysregs = (RegsDirect*)Heap.Alloc(sizeof(RegsDirect));
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
            Heap.Free(m_stackStart);
            Heap.Free(Sysregs);
        }
    }
}
