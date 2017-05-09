namespace Sharpen.FileSystem.Cookie
{
    public class ContainerCookie : ICookie
    {
        public ContainerFS FS { get; private set; }

        /// <summary>
        /// Creates a new ContainerCookie
        /// </summary>
        /// <param name="container">The container</param>
        public ContainerCookie(ContainerFS container)
        {
            FS = container;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
