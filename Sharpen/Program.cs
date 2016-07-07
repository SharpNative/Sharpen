using Sharpen.Arch;
using Sharpen.Drivers.Block;

namespace Sharpen
{
    class Program
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
                Console.WriteLine("Booted by a multiboot bootloader");

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

                    Console.Write("Modules: ");
                    Console.WriteNum((int)modsCount);
                    Console.PutChar('\n');

                    for (uint i = 0; i < modsCount; i++)
                    {
                        Multiboot.Module** mods = (Multiboot.Module**)m_mbootHeader.ModsAddr;
                        Multiboot.Module module = *mods[i];

                        // Check if the end is bigger
                        // If it's bigger, set the new end
                        if((int)module.End > (int)heapStart)
                        {
                            heapStart = module.End;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No modules");
                }
            }

            #endregion

            Heap.Init(heapStart);
            GDT.Init();
            IDT.Init();
            Console.PutChar('\n');

            CMOS.UpdateTime();
            Console.Write("It is ");
            Console.WriteNum(Time.Hours);
            Console.Write(":");
            Console.WriteNum(Time.Minutes);
            Console.Write(":");
            Console.WriteNum(Time.Seconds);
            Console.WriteLine("");

            ATA.Probe();
            ATA.Test();
            ATA.WriteTest();

            Console.WriteLine("\nList of ATA devices:");
            for (int i = 0; i < 4; i++)
            {
                IDE_Device device = ATA.Devices[i];
                if (!device.Exists)
                {
                    continue;
                }

                Console.Write(device.Name);
                Console.PutChar('\n');
            }
        }
    }
}
