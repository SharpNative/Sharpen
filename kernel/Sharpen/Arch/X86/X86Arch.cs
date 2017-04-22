using Sharpen.Drivers.Power;
using Sharpen.Mem;

namespace Sharpen.Arch
{
    sealed class X86Arch
    {
        /// <summary>
        /// Early init for x86
        /// </summary>
        public static void EarlyInit()
        {
            FPU.Init();
        }

        /// <summary>
        /// Initializes the specific x86 stuff
        /// </summary>
        public static void Init()
        {
            GDT.Init();
            PIC.Remap();
            IDT.Init();
            IRQ.Init();

            PhysicalMemoryManager.Init();
            Paging.Init();
            Heap.InitRealHeap();
            
            IOApicManager.Init();
            Acpi.Init();
            LocalApic.InitLocalAPIC();
            IOApicManager.InitIOApics();
            MPTable.Init();

            CMOS.UpdateTime();
            Time.FullTicks = Time.CalculateEpochTime();
        }
    }
}
