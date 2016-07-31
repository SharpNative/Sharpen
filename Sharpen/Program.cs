using Sharpen.Arch;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.Drivers.Power;
using Sharpen.Drivers.Sound;
using Sharpen.FileSystem;

namespace Sharpen
{

    public sealed class Program
    {
        private static Multiboot.Header m_mbootHeader;
        private static bool m_isMultiboot = false;

        /// <summary>
        /// Kernel entrypoint
        /// </summary>
        /// <param name="header">The multiboot header</param>
        /// <param name="magic">The magic</param>
        /// <param name="end">The end address of the kernel</param>
        public static unsafe void KernelMain(Multiboot.Header* header, uint magic, uint end)
        {
            Console.Clear();

            void* heapStart = (void*)end;
            #region Multiboot

            // Booted by a multiboot bootloader
            if (magic == Multiboot.Magic)
            {
                // Bring the header to a safe location
                m_isMultiboot = true;
                fixed (Multiboot.Header* destination = &m_mbootHeader)
                {
                    Memory.Memcpy(destination, header, sizeof(Multiboot.Header));
                }

                // Check if any modules are loaded
                if ((m_mbootHeader.Flags & Multiboot.FlagMods) > 0)
                {
                    uint modsCount = m_mbootHeader.ModsCount;

                    Console.Write("[Multiboot] Detected - Modules: ");
                    Console.WriteNum((int)modsCount);
                    Console.PutChar('\n');

                    for (uint i = 0; i < modsCount; i++)
                    {
                        Multiboot.Module** mods = (Multiboot.Module**)m_mbootHeader.ModsAddr;
                        Multiboot.Module module = *mods[i];

                        // Check if the end is bigger
                        // If it's bigger, set the new end
                        if ((int)module.End > (int)heapStart)
                        {
                            heapStart = module.End;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[Multiboot] Detected - No modules");
                }
            }

            #endregion

            Heap.Init(heapStart);
            GDT.Init();
            PIC.Remap();
            IDT.Init();
            Acpi.Init();

            Paging.Init();
            Heap.SetupRealHeap();
            Console.WriteLine("paging and heap on");

            void* a = Heap.Alloc(800);
            void* b = Heap.Alloc(400);
            Heap.Free(a);
            void* c = Heap.Alloc(800);
            void* d = Heap.AlignedAlloc(0x1000, 600);
            Console.WriteHex((int)a);
            Console.Write(" ");
            Console.WriteHex((int)b);
            Console.Write(" ");
            Console.WriteHex((int)c);
            Console.Write(" ");
            Console.WriteHex((int)d);
            Console.Write(" ");

            PIT.Init();
            CMOS.UpdateTime();
            Keyboard.Init();


            ATA.Probe();
            
            PCI.Probe();
            AC97.Init();

            while (true)
                Console.PutChar(Keyboard.Getch());


            // Idle loop
            while (true)
                CPU.HLT();
        }
    }
}
