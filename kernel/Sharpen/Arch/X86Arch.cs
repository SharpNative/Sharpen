namespace Sharpen.Arch
{
    class X86Arch
    {
        /// <summary>
        /// Initializes the specific x86 stuff
        /// </summary>
        public static void Init()
        {
            GDT.Init();
            PIC.Remap();
            IDT.Init();
            FPU.Init();
            PIT.Init();
        }
    }
}
