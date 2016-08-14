namespace Sharpen.Arch
{
    public sealed class CPU
    {
        /// <summary>
        /// Restores interrupts
        /// </summary>
        public static extern void STI();

        /// <summary>
        /// Clears interrupts
        /// </summary>
        public static extern void CLI();

        /// <summary>
        /// Halt processor
        /// </summary>
        public static extern void HLT();
    }
}
