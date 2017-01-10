namespace Sharpen.MultiTasking
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
        /// <param name="kernelContext">If this is a kernel context</param>
        void CreateNewContext(bool kernelContext);

        /// <summary>
        /// Clones a context from another context
        /// </summary>
        /// <param name="context">The source context</param>
        void CloneFrom(IContext context);
        
        /// <summary>
        /// Increases the virtual address data space
        /// </summary>
        /// <param name="size">The size to increase with</param>
        /// <returns>The old end</returns>
        unsafe void* Sbrk(int size);

        /// <summary>
        /// Prepares this context
        /// </summary>
        void PrepareContext();
    }
}
