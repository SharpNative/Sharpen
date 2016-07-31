using Sharpen.Arch;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.Drivers.Net;
using Sharpen.Drivers.Other;
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

                    for (int i = 0; i < modsCount; i++)
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

            PIT.Init();
            CMOS.UpdateTime();
            
            DevFS.Init();
            VFS.Init();

            Keyboard.Init();
            SerialPort.Init();
            ATA.Probe();

            PCI.Probe();
            //AC97.Init();
            VboxDev.Init();
            //I217.Init();

            Tasking.Init();

            Tasking.AddTask(Util.MethodToPtr(Test1), TaskPriority.VERYLOW);
            Tasking.AddTask(Util.MethodToPtr(Test2), TaskPriority.VERYHIGH);

            
            Console.WriteLine("\nReaddir: mounts://");
            Node searchNode = VFS.GetByPath("mounts://");
            uint j = 0;
            DirEntry* entry = searchNode.ReadDir(searchNode, j);
            j++;
            while (entry != null)
            {
                Console.Write("mounts://");
                Console.WriteLineP(entry->Name);

                entry = searchNode.ReadDir(searchNode, j); 
                j++;
            }

            // SET VM on pause
            //Console.WriteLine("Set VM on pause");
            //Node node = VFS.GetByPath("devices://VMMDEV/powerstate");
            //node.Write(node, 0, 4, ByteUtil.toBytes((int)VboxDevPowerState.Pause));
            
            while (true)
                Console.PutChar(Keyboard.Getch());



            // Idle loop
            while (true)
                CPU.HLT();
        }

        public static void Test1()
        {
            while (true)
            {
                Console.PutChar('a');
            }
        }

        public static void Test2()
        {
            while (true)
            {
                Console.PutChar('b');
            }
        }
    }
}
