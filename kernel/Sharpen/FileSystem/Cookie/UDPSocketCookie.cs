using Sharpen.Mem;
using Sharpen.Net;

namespace Sharpen.FileSystem.Cookie
{
    public class UDPSocketCookie : ICookie
    {
        public UDPSocket Socket;

        /// <summary>
        /// Creates a new UDP cookie
        /// </summary>
        /// <param name="socket">The socket</param>
        public UDPSocketCookie(UDPSocket socket)
        {
            Socket = socket;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
            Socket.Close();
            Heap.Free(Socket);
        }
    }
}
