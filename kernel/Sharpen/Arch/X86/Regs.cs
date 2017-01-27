namespace Sharpen.Arch
{
    // Registers from interrupt stack
    public struct Regs
    {
        public int GS, FS, ES, DS;
        public int EDI, ESI, EBP, Unused, EBX, EDX, ECX, EAX;
        public int IntNum, Error;
        public int EIP, CS, EFlags, ESP, SS;
    }

    // Registers from direct iret stack
    public struct RegsDirect
    {
        public int GS, FS, ES, DS;
        public int EDI, ESI, EBP, Unused, EBX, EDX, ECX, EAX;
        public int EIP, CS, EFlags, ESP, SS;
    }
}
