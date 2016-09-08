using Sharpen.Collections;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    public class PipeFS
    {
        private static List m_fifos;

        /// <summary>
        /// Initializes the PipeFS
        /// </summary>
        public static void Init()
        {
            m_fifos = new List();
        }

        /// <summary>
        /// Initialize PipeFS
        /// </summary>
        /// <param name="nodes">The nodes array for read and write end</param>
        /// <param name="size">The size of the Fifo</param>
        public static unsafe ErrorCode Create(Node[] nodes, int size)
        {
            // 0 is read end, 1 is write end
            // 4096 is the default size
            Node readEnd = new Node();
            Node writeEnd = new Node();
            Fifo fifo = new Fifo(4096, true);

            // Add to list so we can lookup from Cookie
            m_fifos.Add(fifo);
            int index = m_fifos.Count - 1;

            // Configure
            readEnd.Cookie = (uint)index;
            readEnd.GetSize = getSizeImpl;
            readEnd.Read = readImpl;
            writeEnd.Cookie = (uint)index;
            writeEnd.GetSize = getSizeImpl;
            writeEnd.Write = writeImpl;

            // Should be already opened before entering the program again
            VFS.Open(readEnd, FileMode.O_RDONLY);
            VFS.Open(writeEnd, FileMode.O_WRONLY);

            nodes[0] = readEnd;
            nodes[1] = writeEnd;

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Gets a Fifo from a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The fifo</returns>
        private static Fifo getFifoFromNode(Node node)
        {
            Fifo fifo = (Fifo)m_fifos.Item[node.Cookie];
            return fifo;
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
            Fifo fifo = getFifoFromNode(node);
            return fifo.Write((byte*)Util.ObjectToVoidPtr(buffer), size);
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
            Fifo fifo = getFifoFromNode(node);
            return fifo.Read(buffer, size, 0);
        }

        /// <summary>
        /// Gets the size of a pipe
        /// </summary>
        /// <param name="node">The pipe node</param>
        /// <returns>The size</returns>
        private static uint getSizeImpl(Node node)
        {
            return getFifoFromNode(node).AvailableBytes;
        }
    }
}
