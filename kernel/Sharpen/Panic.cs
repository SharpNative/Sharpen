﻿using Sharpen.Arch;
using Sharpen.Task;

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

            // Empty screen
            Console.Attribute = 0x4F;
            //Console.Clear();

            // Logo
            /*Console.WriteLine("");
            Console.WriteLine("  ____  _                                 ");
            Console.WriteLine(" / ___|| |__   __ _ _ __ _ __   ___ _ __  ");
            Console.WriteLine(" \\___ \\| '_ \\ / _` | '__| '_ \\ / _ \\ '_ \\ ");
            Console.WriteLine("  ___) | | | | (_| | |  | |_) |  __/ | | |");
            Console.WriteLine(" |____/|_| |_|\\__,_|_|  | .__/ \\___|_| |_|");
            Console.WriteLine("                        |_|               ");
            Console.WriteLine("");*/

            // Message
            Console.Write("\tMessage: ");
            Console.WriteLine(str);

            if (Tasking.CurrentTask != null)
            {
                Console.Write("\tPID: ");
                Console.WriteHex(Tasking.CurrentTask.PID);
            }

            if (regsPtr != null)
            {
                Console.Write("  Errorcode: ");
                Console.WriteHex(regsPtr->Error);
                Console.PutChar('\n');
                Console.PutChar('\n');

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
                Console.PutChar('\n');

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
                Console.PutChar('\n');

                Console.Write("\tEBP ");
                Console.WriteHex(regsPtr->EBP & 0xFFFFFFFF);
                Console.Write("  ESP ");
                Console.WriteHex(regsPtr->ESP & 0xFFFFFFFF);
                Console.Write("  EIP ");
                Console.WriteHex(regsPtr->EIP);
                Console.Write("  CR2 ");
                Console.WriteHex(Paging.ReadCR2());
                Console.PutChar('\n');
            }

            // HALT
            CPU.HLT();
        }
    }
}
