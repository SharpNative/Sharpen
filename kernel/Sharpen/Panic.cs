using Sharpen.Arch;

namespace Sharpen
{
    public sealed class Panic
    {
        /// <summary>
        /// Does a kernel panic
        /// </summary>
        /// <param name="str">The message</param>
        public static void DoPanic(string str)
        {
            DoPanic(str, 0);
        }

        /// <summary>
        /// Does a kernel panic
        /// </summary>
        /// <param name="str">The message</param>
        /// <param name="extraData">Extra data</param>
        public static void DoPanic(string str, int extraData)
        {
            // Clear interrupts
            CPU.CLI();

            // Empty screen
            Console.Attribute = 0x4F;
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
            Console.Write("\tMessage: ");
            Console.WriteLine(str);
            Console.Write("\tExtra Data: ");
            Console.WriteHex(extraData);

            // HALT
            CPU.HLT();
        }
    }
}
