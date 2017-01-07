using Sharpen.Arch;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.Drivers.Net;
using Sharpen.Drivers.Other;
using Sharpen.Drivers.Power;
using Sharpen.Drivers.Sound;
using Sharpen.Exec;
using Sharpen.FileSystem;
using Sharpen.Lib;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.MultiTasking;

namespace Sharpen
{
    public sealed class Program
    {
        private static Multiboot.Header m_mbootHeader;
        private static bool m_isMultiboot = false;
        private static uint m_memSize;
        private static unsafe void* heapStart;

        /// <summary>
        /// Kernel entrypoint
        /// </summary>
        /// <param name="header">The multiboot header</param>
        /// <param name="magic">The magic</param>
        /// <param name="end">The end address of the kernel</param>
        public static unsafe void KernelMain(Multiboot.Header* header, uint magic, uint end)
        {
            void* heapStart = (void*)end;
            Console.Clear();

            processMultiboot(header, magic);
            Heap.InitTempHeap(heapStart);
            X86Arch.Init();
            Acpi.Init();
            initMemory();
            Random.Init();

            initFileSystems();

            initPCIDevices();
            Keyboard.Init();

            Tasking.Init();

            initStorage();
            initNetworking();
            runUserspace();

            // Idle loop
            while (true)
                CPU.HLT();
        }

        /// <summary>
        /// Init PCI devices
        /// </summary>
        private static void initPCIDevices()
        {
            PCI.Init();
            PCIFS.Init();

            //AC97.Init();
            VboxDev.Init();
        }

        /// <summary>
        /// Initializes filesystems and VFS
        /// </summary>
        private static void initFileSystems()
        {
            VFS.Init();
            DevFS.Init();
            NullFS.Init();
            RandomFS.Init();
            STDOUT.Init();
            SerialPort.Init();
            PipeFS.Init();
            NetFS.Init();
        }

        /// <summary>
        /// Initializes storage components
        /// </summary>
        private static void initStorage()
        {
            ATA.Init();

            Node hddNode = VFS.GetByAbsolutePath("devices://HDD0/");
            Fat16.Init(hddNode, "C");
            Tasking.KernelTask.CurrentDirectory = "C://";
        }

        /// <summary>
        /// Initializes networking
        /// </summary>
        private static void initNetworking()
        {
            // Networking
            Network.Init();
            Route.Init();

            // Layer 1 - drivers
            E1000.Init();
            PCNet2.Init();
            rtl8139.Init();

            // Layer 2 - Networking protocols
            IPV4.Init();
            ICMP.Init();

            // Layer 3 - Transport protocols
            UDP.Init();
            TCP.Init();
            ARP.Init();
            DHCP.Init();
        }

        /// <summary>
        /// Initializes memory-related components
        /// </summary>
        private static void initMemory()
        {
            PhysicalMemoryManager.Init(m_memSize);
            Paging.Init(m_memSize);
            Heap.InitRealHeap();
        }

        /// <summary>
        /// Processes the multiboot header
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="magic">The magic multiboot number</param>
        private static unsafe void processMultiboot(Multiboot.Header* header, uint magic)
        {
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
            m_memSize = m_mbootHeader.MemHi;

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
        }

        /// <summary>
        /// Runs the userspace
        /// </summary>
        private static void runUserspace()
        {
            // Initial process, usage: init [program]
            string[] argv = new string[3];
            argv[0] = "C://exec/init";
            argv[1] = "C://exec/shell";
            argv[2] = null;
            
            int error = Loader.StartProcess(argv[0], argv, Task.SpawnFlags.NONE);
            if (error < 0)
            {
                Console.Write("Failed to start init process: 0x");
                Console.WriteHex(-error);
                Console.Write('\n');
            }
        }
    }
}
