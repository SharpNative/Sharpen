using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.FileSystem.Cookie
{
    public class AudioDataCookie : ICookie
    {
        public Fifo Buffer { get; private set; }

        /// <summary>
        /// Creates a new AudioDataCookie
        /// </summary>
        /// <param name="bufferSize">Buffer size</param>
        public AudioDataCookie(int bufferSize)
        {
            Buffer = new Fifo(bufferSize, false);
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
            Buffer.Dispose();
            Heap.Free(Buffer);
        }
    }
}
