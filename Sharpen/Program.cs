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

            Panic.DoPanic("hallo");
        }
    }
}
