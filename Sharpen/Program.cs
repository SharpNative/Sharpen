﻿using Sharpen.Arch;
using Sharpen.Drivers.Block;
using Sharpen.Drivers.Char;
using Sharpen.Drivers.Net;
using Sharpen.Drivers.Other;
using Sharpen.Drivers.Power;
using Sharpen.Drivers.Sound;
using Sharpen.Exec;
using Sharpen.FileSystem;
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
            uint memSize = 32;
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

                // Memory size
                memSize = m_mbootHeader.MemHi;

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
            FPU.Init();
            
            Paging.Init(memSize);
            Heap.SetupRealHeap();
            
            PIT.Init();
            CMOS.UpdateTime();
            Keyboard.Init();
            
            DevFS.Init();
            VFS.Init();
            SerialPort.Init();
            
            PCI.Probe();
            //AC97.Init();
            VboxDev.Init();
            //I217.Init();
            ATA.Init();
            Tasking.Init();

            Node hddNode = VFS.GetByPath("devices://HDD0");
            Fat16.Init(hddNode, "C");
            
            Console.WriteLine("\nReaddir: C://a/");
            Node searchNode = VFS.GetByPath("C://a/");
            uint j = 0;
            DirEntry* entry = null;
            do
            {
                entry = VFS.ReadDir(searchNode, j);
                if (entry == null)
                    break;

                Console.Write("C://a/");
                Console.WriteLine(Util.CharPtrToString(entry->Name));

                j++;
            }
            while (entry != null);
            
            Node node = VFS.GetByPath("C://a/test.txt");
            byte[] buf = new byte[node.Size];
            VFS.Open(node, FileMode.O_RDONLY);
            uint bytes = VFS.Read(node, 0, 10, buf);
            VFS.Close(node);

            for (int i = 0; i < bytes; i++)
                Console.PutChar((char)buf[i]);
            
            //node = VFS.GetByPath("C://testt");
            //SubDirectory a = Fat16.readDirectory(7);

            // Node nd =  Fat16.FindFileInDirectory(a, (char *)Util.ObjectToVoidPtr("TEST    TXT"));
            //Console.WriteHex(nd.Cookie);


            

            // Idle loop
            while (true)
                CPU.HLT();
        }

        public static void Test1()
        {
            while (true)
                Console.PutChar('a');
        }

        public static void Test2()
        {
            while (true)
                Console.PutChar('b');
        }
    }
}
