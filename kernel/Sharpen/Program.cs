using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.Drivers.Net;
using Sharpen.Drivers.Other;
using Sharpen.Drivers.Power;
using Sharpen.Drivers.Sound;
using Sharpen.Exec;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Task;
using Sharpen.Utilities;

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
            uint memSize;
            #region Multiboot

            // We require to be booted by a multiboot compliant bootloader
            if (magic != Multiboot.Magic)
            {
                Panic.DoPanic("Not booted by a multiboot compliant bootloader");
            }

            // Bring the header to a safe location
            m_isMultiboot = true;
            fixed (Multiboot.Header* destination = &m_mbootHeader)
            {
                Memory.Memcpy(destination, header, sizeof(Multiboot.Header));
            }

            // Memory size
            memSize = m_mbootHeader.MemHi;

            // Check if any modules are loaded
            if ((m_mbootHeader.Flags & Multiboot.FlagMods) > 0)
            {
                uint modsCount = m_mbootHeader.ModsCount;

                Console.Write("[Multiboot] Detected - Modules: ");
                Console.WriteNum((int)modsCount);
                Console.Write('\n');

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

            #endregion

            Heap.TempInit(heapStart);

            GDT.Init();
            PIC.Remap();
            IDT.Init();
            Acpi.Init();
            FPU.Init();

            PhysicalMemoryManager.Init(memSize);
            Paging.Init(memSize);
            Heap.SetupRealHeap();

            PIT.Init();
            VFS.Init();
            DevFS.Init();
            Keyboard.Init();
            STDOUT.Init();
            SerialPort.Init();
            PipeFS.Init();


            PCI.Init();
            //AC97.Init();
            VboxDev.Init();


            Tasking.Init();

            // Networking proto's 
            Network.Init();
            IPV4.Init();
            UDP.Init();
            ARP.Init();

            // Networking drivers
            //E1000.Init();
            PCNet2.Init();
            //rtl8139.Init();

            ATA.Init();
            
            Node hddNode = VFS.GetByPath("devices://HDD0");
            Fat16.Init(hddNode, "C");
            Tasking.KernelTask.CurrentDirectory = "C://";
            
            byte[] bac = new byte[6];
            Network.GetMac((byte *)Util.ObjectToVoidPtr(bac));

            //NetworkTools.WakeOnLan(bac);
            //DHCP.Init();
            
            // Initial process, usage: init [program]
            string[] argv = new string[3];
            argv[0] = "C://init";
            argv[1] = "C://shell";
            argv[2] = null;

            int error = Loader.StartProcess(argv[0], argv, Tasking.SpawnFlags.NONE);
            if (error < 0)
            {
                Console.Write("Failed to start init process: 0x");
                Console.WriteHex(-error);
                Console.Write('\n');
            }

            // Idle loop
            while (true)
                CPU.HLT();
        }
    }
}
