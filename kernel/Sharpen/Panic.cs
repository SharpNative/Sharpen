// #define CLEAR_SCREEN

using Sharpen.Arch;
using Sharpen.MultiTasking;
using Sharpen.Utilities;

namespace Sharpen
{
    public sealed class Panic
    {
        /// <summary>
        /// Does a kernel panic
        /// </summary>
        /// <param name="str">The message</param>
        public static unsafe void DoPanic(string str)
        {
            DoPanic(str, null);
        }

        /// <summary>
        /// Does a kernel panic
        /// </summary>
        /// <param name="str">The message</param>
        /// <param name="regsPtr">Registers</param>
        public static unsafe void DoPanic(string str, Regs* regsPtr)
        {
            // Clear interrupts
            CPU.CLI();
            
            Console.Attribute = 0x4F;

            // Empty screen with logo
#if CLEAR_SCREEN
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("  ____  _                                 ");
            Console.WriteLine(" / ___|| |__   __ _ _ __ _ __   ___ _ __  ");
            Console.WriteLine(" \\___ \\| '_ \\ / _` | '__| '_ \\ / _ \\ '_ \\ ");
            Console.WriteLine("  ___) | | | | (_| | |  | |_) |  __/ | | |");
            Console.WriteLine(" |____/|_| |_|\\__,_|_|  | .__/ \\___|_| |_|");
            Console.WriteLine("                        |_|               ");
            Console.WriteLine("");
#endif
            
            // Message
            Console.WriteLine(str);

            if (Tasking.IsActive)
            {
                Console.Write("\tPID: ");
                Console.WriteNum(Tasking.CurrentTask.PID);
                Console.Write("\tTID: ");
                Console.WriteNum(Tasking.CurrentTask.CurrentThread.TID);
            }

            if (regsPtr != null)
            {
                Console.Write("  Errorcode: ");
                Console.WriteHex(regsPtr->Error);
                Console.Write('\n');
                Console.Write('\n');

                Console.Write("\tEAX ");
                Console.WriteHex(regsPtr->EAX & 0xFFFFFFFF);
                Console.Write("  EBX ");
                Console.WriteHex(regsPtr->EBX & 0xFFFFFFFF);
                Console.Write("  ECX ");
                Console.WriteHex(regsPtr->ECX & 0xFFFFFFFF);
                Console.Write("  EDX ");
                Console.WriteHex(regsPtr->EDX & 0xFFFFFFFF);
                Console.Write("  EDI ");
                Console.WriteHex(regsPtr->EDI & 0xFFFFFFFF);
                Console.Write("  ESI ");
                Console.WriteHex(regsPtr->ESI & 0xFFFFFFFF);
                Console.Write('\n');

                Console.Write("\tCS ");
                Console.WriteHex(regsPtr->CS & 0xFFFF);
                Console.Write("  DS ");
                Console.WriteHex(regsPtr->DS & 0xFFFF);
                Console.Write("  ES ");
                Console.WriteHex(regsPtr->ES & 0xFFFF);
                Console.Write("  FS ");
                Console.WriteHex(regsPtr->FS & 0xFFFF);
                Console.Write("  GS ");
                Console.WriteHex(regsPtr->GS & 0xFFFF);
                Console.Write("  SS ");
                Console.WriteHex(regsPtr->SS & 0xFFFF);
                Console.Write('\n');

                Console.Write("\tEBP ");
                Console.WriteHex(regsPtr->EBP & 0xFFFFFFFF);
                Console.Write("  ESP ");
                Console.WriteHex(regsPtr->ESP & 0xFFFFFFFF);
                Console.Write("  EIP ");
                Console.WriteHex(regsPtr->EIP & 0xFFFFFFFF);
                Console.Write("  CR2 ");
                Console.WriteHex((int)Paging.FaultingAddress & 0xFFFFFFFF);
                Console.Write("  EFlags ");
                Console.WriteHex(regsPtr->EFlags & 0xFFFFFFFF);
                Console.Write('\n');
            }

            // Only able to stacktrace if paging is available
            if (Paging.CurrentDirectory != null)
            {
                Util.PrintStackTrace(10);
            }

            // HALT
            CPU.HLT();
        }
    }
}
