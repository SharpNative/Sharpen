using Sharpen.Arch;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.FileSystem;

namespace Sharpen
{
    class Test
    {
        public int a = 3;

        public Test()
        {

        }
    }

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
                        if ((int)module.End > (int)heapStart)
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
            PIC.Remap();
            PIT.Init();
            CMOS.UpdateTime();
            IDT.Init();
            Keyboard.Init();
            Console.PutChar('\n');

            ATA.Probe();

            Console.WriteLine("VFS test:");
            VFS vfs = new VFS();
            MountPoint test = new MountPoint();
            test.Name = "henk";
            MountPoint test2 = new MountPoint();
            test2.Name = "de pony";
            vfs.AddMountPoint(test);
            vfs.AddMountPoint(test2);

            MountPoint mp = vfs.FindMountByName("de pony");
            if (mp == null)
                Console.WriteLine("TEST FAIL!");
            else
                Console.WriteLine(mp.Name);
            
            // Idle loop
            while (true)
                CPU.HLT();
        }
    }
}
