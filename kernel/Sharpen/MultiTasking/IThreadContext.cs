namespace Sharpen.MultiTasking
{
    public interface IThreadContext
    {
        /// <summary>
        /// Cleans the thread context
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Stores the current context state
        /// </summary>
        /// <param name="ptr">The register pointer</param>
        unsafe void StoreContext(void* ptr);

        /// <summary>
        /// Restores a previous state of this context
        /// </summary>
        /// <returns>The pointer of the stack</returns>
        unsafe void* RestoreContext();

        /// <summary>
        /// Sets the return value from a syscall
        /// </summary>
        /// <param name="ret">The return value</param>
        void SetSysReturnValue(int ret);

        /// <summary>
        /// Updates syscall registers
        /// </summary>
        /// <param name="ptr">Pointer to syscall registers</param>
        unsafe void UpdateSyscallRegs(void* regsPtr);

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="eip">The initial instruction pointer</param>
        /// <param name="initialStackSize">Initial stack size</param>
        /// <param name="initialStack">Initial stack data</param>
        /// <param name="kernelContext">If this is a kernel context or not</param>
        unsafe void CreateNewContext(void* eip, int initialStackSize, int[] initialStack, bool kernelContext);

        /// <summary>
        /// Clones the context from another thread
        /// </summary>
        /// <param name="context">The other thread context</param>
        void CloneFrom(IThreadContext context);
    }
}
