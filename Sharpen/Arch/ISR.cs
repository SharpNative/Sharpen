namespace Sharpen.Arch
{
    public sealed class ISR
    {
        // ISR error codes
        private static readonly string[] m_errorCodes =
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
            "Stack fault",
            "General protection fault",
            "Page fault",
            "?",
            "FPU exception",
            "Alignment check",
            "Machine check"
        };
        
        /// <summary>
        /// ISR handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(Regs* regsPtr)
        {
            int isrNum = (*regsPtr).IntNum;
            // Panic.DoPanic(m_errorCodes[isrNum]);
            Console.WriteLine(m_errorCodes[isrNum]);
            CPU.CLI();
            CPU.HLT();
        }
    }
}
