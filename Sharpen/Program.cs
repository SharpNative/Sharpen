using Sharpen.Arch;
using Sharpen.Drivers.Block;

namespace Sharpen
{
    class Program
    {
        /// <summary>
        /// Kernel entrypoint
        /// </summary>
        public static unsafe void KernelMain()
        {
            Heap.Init((void*)0x911000);
            Console.Clear();
            GDT.Init();

            Console.WriteLine("test test");
            Console.WriteLine("1234");

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

                Console.WriteStr(device.Name);
                Console.PutChar('\n');
            }

            for (int i = 0; i < 8000; i++)
            {
                Console.WriteNum(i);
                Console.PutChar(' ');
            }

            // Panic.DoPanic("hallo");
        }
    }
}
