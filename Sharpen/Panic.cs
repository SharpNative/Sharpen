using Sharpen.Arch;

namespace Sharpen
{
    public class Panic
    {
        /// <summary>
        /// Does a kernel panic
        /// </summary>
        /// <param name="str">The message</param>
        public static void DoPanic(string str)
        {
            // Clear interrupts
            CPU.CLI();

            // Empty screen
            Console.Attribute = 0x04;
            Console.Clear();

            // Logo
            Console.WriteLine("");
            Console.WriteLine("  ____  _                                 ");
            Console.WriteLine(" / ___|| |__   __ _ _ __ _ __   ___ _ __  ");
            Console.WriteLine(" \\___ \\| '_ \\ / _` | '__| '_ \\ / _ \\ '_ \\ ");
            Console.WriteLine("  ___) | | | | (_| | |  | |_) |  __/ | | |");
            Console.WriteLine(" |____/|_| |_|\\__,_|_|  | .__/ \\___|_| |_|");
            Console.WriteLine("                        |_|               ");
            Console.WriteLine("");

            // Message
            Console.Write("Message: ");
            Console.Write(str);

            // HALT
            CPU.HLT();
        }
    }
}
