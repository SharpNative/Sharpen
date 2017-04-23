using Sharpen.Collections;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    public class PipeFS
    {
        public const int DefaultPipeSize = 4096;
        
        /// <summary>
        /// Initialize PipeFS
        /// </summary>
        /// <param name="nodes">The nodes array for read and write end</param>
        /// <param name="size">The size of the Fifo</param>
        public static unsafe ErrorCode Create(Node[] nodes, int size)
        {
            // Two sided pipe
            Node readEnd = new Node();
            Node writeEnd = new Node();

            Fifo fifo = new Fifo(DefaultPipeSize, true);
            PipeFSCookie cookie = new PipeFSCookie();
            cookie.Fifo = fifo;

            // Configure
            readEnd.Cookie = cookie;
            readEnd.GetSize = getSizeImpl;
            readEnd.Read = readImpl;
            readEnd.Close = closeImpl;
            writeEnd.Cookie = cookie;
            writeEnd.GetSize = getSizeImpl;
            writeEnd.Write = writeImpl;
            writeEnd.Close = closeImpl;

            // Should be already opened before entering the program again
            VFS.Open(readEnd, (int)FileMode.O_RDONLY);
            VFS.Open(writeEnd, (int)FileMode.O_WRONLY);

            // 0 is read end, 1 is write end
            nodes[0] = readEnd;
            nodes[1] = writeEnd;

            return ErrorCode.SUCCESS;
        }
        
        /// <summary>
        /// Closes the node
        /// </summary>
        /// <param name="node">The node</param>
        private static void closeImpl(Node node)
        {
            PipeFSCookie cookie = (PipeFSCookie)node.Cookie;
            node.Cookie = null;

            // Last reference gone?
            cookie.ReferenceCount--;
            if (cookie.ReferenceCount == 0)
            {
                cookie.Fifo.Dispose();
                Heap.Free(cookie.Fifo);
                Heap.Free(cookie);
            }
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            PipeFSCookie cookie = (PipeFSCookie)node.Cookie;
            if (cookie == null)
                return 0;
            
            return cookie.Fifo.Write((byte*)Util.ObjectToVoidPtr(buffer), size);
        }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            PipeFSCookie cookie = (PipeFSCookie)node.Cookie;
            if (cookie == null)
                return 0;

            return cookie.Fifo.Read(buffer, size, 0);
        }

        /// <summary>
        /// Gets the size of a pipe
        /// </summary>
        /// <param name="node">The pipe node</param>
        /// <returns>The size</returns>
        private static uint getSizeImpl(Node node)
        {
            PipeFSCookie cookie = (PipeFSCookie)node.Cookie;
            if (cookie == null)
                return 0;

            Fifo fifo = cookie.Fifo;
            return fifo.AvailableBytes;
        }
    }
}
