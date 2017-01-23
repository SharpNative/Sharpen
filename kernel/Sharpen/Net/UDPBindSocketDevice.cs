using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPBindSocketDevice
    {
        /// <summary>
        /// Binds a UDP port
        /// </summary>
        /// <param name="name">The UDP port</param>
        /// <returns>The node</returns>
        public static unsafe Node Open(string name)
        {
            int port = Int.Parse(name);
            if (port == -1)
                return null;

            UDPSocket sock = new UDPSocket();
            bool found = sock.Bind((ushort)port);

            if(!found)
                return null;
            
            Node node = new Node();
            node.Flags = NodeFlags.FILE;
            node.Cookie = (uint)Util.ObjectToVoidPtr(sock);
            node.Read = readImpl;
            node.Write = writeImpl;
            node.GetSize = getSizeImpl;
            node.Close = closeImpl;

            return node;
        }

        /// <summary>
        /// Gets the UDP socket from a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The socket</returns>
        private static unsafe UDPSocket getSocketFromNode(Node node)
        {
            return (UDPSocket)Util.VoidPtrToObject((void*)node.Cookie);
        }

        /// <summary>
        /// Reads data from the UDP socket
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The destination buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only header is 6 bytes
            if (size < 6)
                return 0;

            UDPSocket sock = getSocketFromNode(node);
            if (sock == null)
                return 0;

            return sock.ReadPacket((byte*)Util.ObjectToVoidPtr(buffer), size);
        }

        /// <summary>
        /// Writes data to the UDP socket
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The destination buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only header is 6 bytes
            if (size < 6)
                return 0;

            UDPSocket sock = getSocketFromNode(node);
            if (sock == null)
                return 0;

            sock.SendByPacket((byte *)Util.ObjectToVoidPtr(buffer), size);

            return size;
        }

        /// <summary>
        /// Gets the size of a UDP socket
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The size</returns>
        private static unsafe uint getSizeImpl(Node node)
        {
            UDPSocket sock = getSocketFromNode(node);
            if (sock != null)
                return sock.GetSize();

            return 0;
        }

        /// <summary>
        /// Closes a UDP socket
        /// </summary>
        /// <param name="node">The node</param>
        private static unsafe void closeImpl(Node node)
        {
            UDPSocket sock = getSocketFromNode(node);
            sock.Close();
            node.Cookie = 0;
            Heap.Free(sock);
        }
    }
}
