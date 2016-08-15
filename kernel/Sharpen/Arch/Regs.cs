namespace Sharpen.Arch
{
    public struct Regs
    {
        public int GS, FD, ES, DS;
        public int EDI, ESI, EBP, Unused, EBX, EDX, ECX, EAX;
        public int IntNum, Error;
        public int EIP, CS, EFlags, ESP, SS;
    }
}
