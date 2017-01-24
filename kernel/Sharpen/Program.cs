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
using Sharpen.Utilities;
using Sharpen.Drivers.USB;

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
            heapStart = (void*)end;
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

            initUSB();

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
            ProcFS.Init();
        }

        /// <summary>
        /// Initializes storage components
        /// </summary>
        private static void initStorage()
        {
            AHCI.Init();
            ATA.Init();

            Node hddNode = VFS.GetByAbsolutePath("devices://HDD0");
            Fat16.Init(hddNode, "C");
            Tasking.KernelTask.CurrentDirectory = "C://";
        }

        /// <summary>
        /// Initializes USB
        /// </summary>
        private static void initUSB()
        {
            UHCI.Init();
        }

        /// <summary>
        /// Initializes networking
        /// </summary>
        private static unsafe void initNetworking()
        {
            // Networking
            Network.Init();
            Route.Init();

            // Networking protocols
            IPV4.Init();
            ICMP.Init();

            // Transport protocols
            UDP.Init();
            TCP.Init();
            ARP.Init();
            DHCP.Init();

            // Network drivers
            E1000.Init();
            PCNet2.Init();
            rtl8139.Init();
            
            DHCP.Discover();
            
            
            TCPConnection con = TCP.Bind(80);

            string message = "<!doctype html><html><title>Van Sharpen</title><body>Wij serveren dit van Sharpen naar Dossche</body></html>";
            string httpResp = "HTTP/1.1 200 OK\r\nDate: Fri, 13 May 2005 05:51:12 GMT\r\nServer: Sharpen :)\r\nLast-Modified: Fri, 13 May 2005 05:25:02 GMT\r\nAccept-Ranges: bytes\r\nContent-Length: ";

            string count = Int.ToString(String.Length(message));

            httpResp = String.Merge(httpResp, count);
            httpResp = String.Merge(httpResp, "\r\nConnection: close\r\nContent-Type: text/html\r\n\r\n");

            string finalResp = String.Merge(httpResp, message);

            TCPPacketDescriptor* ptr;
            while (true)
            {
                ptr = TCP.Read(con);
                if (ptr == null)
                {
                    continue;
                }

                if (ptr->Type == TCPPacketDescriptorTypes.ACCEPT)
                {
                    Console.Write("New connection from: ");
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteNum(ptr->Data[i]);
                        Console.Write('.');
                    }
                    Console.WriteNum(ptr->Data[3]);
                    Console.Write(" with XID: ");
                    Console.WriteHex(ptr->xid);
                    Console.WriteLine("");
                }
                else if (ptr->Type == TCPPacketDescriptorTypes.RECEIVE)
                {
                    Console.Write("New data from XID: ");
                    Console.WriteHex(ptr->xid);
                    Console.WriteLine("");
                    
                    TCP.Send(con, ptr->xid, (byte*)Util.ObjectToVoidPtr(finalResp), (uint)String.Length(finalResp));

                    TCP.Close(con, ptr->xid);
                }
                else if (ptr->Type == TCPPacketDescriptorTypes.RESET)
                {
                    Console.Write("RESET from XID: ");
                    Console.WriteHex(ptr->xid);
                    Console.WriteLine("");
                }
                else if (ptr->Type == TCPPacketDescriptorTypes.CLOSE)
                {
                    Console.Write("CLOSE from XID: ");
                    Console.WriteHex(ptr->xid);
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Invalid ptr->Type!");
                }

                Heap.Free(ptr);
            }

            Console.WriteLine("EXIT");
            for (;;) ;

            TCP.Free(con);
        }

        /// <summary>
        /// Initializes memory-related components
        /// </summary>
        private static void initMemory()
        {
            PhysicalMemoryManager.Init();
            Paging.Init();
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
                Panic.DoPanic("Not booted by a multiboot bootloader");
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

                Console.Write("[Multiboot] Modules: ");
                Console.WriteNum((int)modsCount);
                Console.Write('\n');

                Multiboot.Module** mods = (Multiboot.Module**)m_mbootHeader.ModsAddr;
                for (int i = 0; i < modsCount; i++)
                {
                    Multiboot.Module* module = mods[i];

                    // Move the heap end
                    if ((int)module->End > (int)heapStart)
                    {
                        heapStart = module->End;
                    }
                }
            }
            else
            {
                Console.WriteLine("[Multiboot] No modules");
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

            Heap.Free(argv);
        }
    }
}
