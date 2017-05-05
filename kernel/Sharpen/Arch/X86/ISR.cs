 #define ALWAYS_DO_PANIC

using Sharpen.MultiTasking;
using Sharpen.Exec;

namespace Sharpen.Arch
{
    public sealed class ISR
    {
        // ISR error codes
        private static readonly string[] errorCodes =
        {
            "Divide by zero",
            "Debug",
            "Non-maskable interrupt",
            "Breakpoint",
            "Detected overflow",
            "Out-of-bounds",
            "Invalid opcode",
            "No FPU",
            "Double fault",
            "FPU segment overrun",
            "Bad TSS",
            "Segment not present",
            "Stack segment fault",
            "General protection fault",
            "Page fault",
            "?",
            "FPU exception",
            "Alignment check",
            "Machine check"
        };

        // ISR -> Signal number
        private static readonly Signal[] isrToSignal =
        {
            Signal.SIGFPE,
            Signal.SIGTRAP,
            Signal.SIGKILL,
            Signal.SIGTRAP,
            Signal.SIGTRAP,
            Signal.SIGKILL,
            Signal.SIGILL,
            Signal.SIGFPE,
            Signal.SIGKILL,
            Signal.SIGFPE,
            Signal.SIGKILL,
            Signal.SIGSEGV,
            Signal.SIGSEGV,
            Signal.SIGKILL,
            Signal.SIGSEGV,
            Signal.SIGKILL,
            Signal.SIGFPE,
            Signal.SIGKILL,
            Signal.SIGABRT
        };

        /// <summary>
        /// ISR handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(Regs* regsPtr)
        {
            int isrNum = regsPtr->IntNum;
            
            // If the kernel caused this, do a panic
            if (!Tasking.IsActive || Tasking.CurrentTask.PID == 0)
            {
                Panic.DoPanic(errorCodes[isrNum], regsPtr);
            }
            // Otherwise send a signal
            else
            {
#if ALWAYS_DO_PANIC
                Panic.DoPanic(errorCodes[isrNum], regsPtr);
#else
                Tasking.CurrentTask.ProcessSignal(isrToSignal[isrNum]);
#endif
            }
        }
    }
}
