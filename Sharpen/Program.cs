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

            PIT.Init();
            CMOS.UpdateTime();
            Keyboard.Init();

            DevFS.Init();
            SerialPort.Init();
            ATA.Probe();

            PCI.Probe();
            //AC97.Init();
            VboxDev.Init();
            //I217.Init();


            Console.WriteLine("\nReaddir: devices://");
            Node searchNode = VFS.GetByPath("devices://");
            uint i = 0;
            DirEntry* entry = searchNode.ReadDir(searchNode, i);
            i++;
            while (entry != null)
            {
                Console.Write("\tdevices://");
                Console.WriteLineP(entry->Name);

                entry = searchNode.ReadDir(searchNode, i); 
                i++;
            }
            
            byte[] t = new byte[3];
            t[0] = (byte)'T';
            t[1] = (byte)'A';
            t[2] = (byte)'\0';
            byte[] a = new byte[3];

            Node node = VFS.GetByPath("devices://COM1");

            Console.WriteLine("Writing TA to serial port");
            node.Write(node, 0, 3, t);
            node.Read(node, 0, 3, a);

            Console.WriteLine("Response");
            Console.PutChar((char)a[0]);
            Console.PutChar((char)a[1]);

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
    }
}
