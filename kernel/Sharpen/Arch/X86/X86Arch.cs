namespace Sharpen.Arch
{
    sealed class X86Arch
    {
        /// <summary>
        /// Early init for x86
        /// </summary>
        public static void EarlyInit()
        {
            FPU.Init();
        }

        /// <summary>
        /// Initializes the specific x86 stuff
        /// </summary>
        public static void Init()
        {
            GDT.Init();
            PIC.Remap();
            IDT.Init();
            PIT.Init();
        }
    }
}
