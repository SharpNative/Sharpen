namespace Sharpen.Arch
{
    public interface IContext
    {
        /// <summary>
        /// Cleans up the context
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="eip">The initial instruction pointer</param>
        /// <param name="initialStackSize">Initial stack size</param>
        /// <param name="initialStack">Initial stack data</param>
        /// <param name="kernelContext">If this is a kernel context or not</param>
        unsafe void CreateNewContext(void* eip, int initialStackSize, int[] initialStack, bool kernelContext);

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
        /// Clones a context from another context
        /// </summary>
        /// <param name="context">The source context</param>
        void CloneFrom(IContext context);

        /// <summary>
        /// Creates a kernel context
        /// </summary>
        void CreateKernelContext();

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
        /// Forks the context
        /// </summary>
        /// <returns>The new PID or zero</returns>
        int Fork();

        /// <summary>
        /// Increases the virtual address data space
        /// </summary>
        /// <param name="size">The size to increase with</param>
        /// <returns>The old end</returns>
        unsafe void* Sbrk(int size);
    }
}
