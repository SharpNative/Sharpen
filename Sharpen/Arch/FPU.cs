namespace Sharpen.Arch
{
    public sealed class FPU
    {
        /// <summary>
        /// Initializes the FPU controller
        /// </summary>
        public static extern void Init();

        /// <summary>
        /// Stores the current FPU context
        /// </summary>
        /// <param name="context">The pointer to the context</param>
        public static extern unsafe void StoreContext(void* context);

        /// <summary>
        /// Restores the current FPU context
        /// </summary>
        /// <param name="context">The pointer to the context</param>
        public static extern unsafe void RestoreContext(void* context);
    }
}
