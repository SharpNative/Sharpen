using Sharpen.Collections;

namespace Sharpen.FileSystem.Cookie
{
    public class PipeFSCookie : ICookie
    {
        public Fifo Fifo;
        public int ReferenceCount = 2;

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
