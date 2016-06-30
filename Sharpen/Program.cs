namespace Sharpen
{
    class Program
    {
        /// <summary>
        /// Kernel entrypoint
        /// </summary>
        static void KernelMain()
        {
            GDT.Init();

            Console.WriteLine("test test");
            Console.WriteLine("1234");
        }
    }
}
