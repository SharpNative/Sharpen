using Sharpen.Arch;

namespace Sharpen
{
    class Program
    {
        /// <summary>
        /// Kernel entrypoint
        /// </summary>
        static void KernelMain()
        {
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

            // Panic.DoPanic("hallo");
        }
    }
}
