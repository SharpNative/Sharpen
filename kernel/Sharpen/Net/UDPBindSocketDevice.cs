using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPBindSocketDevice
    {
        private struct UDPSocketItem
        {
            public UDPSocket Socket;
            public bool InUse;
        }

        private static uint i = 0;
        private static UDPSocketItem[] m_list;

        public static void Init()
        {
            m_list = new UDPSocketItem[30];
        }

        public static unsafe Node Open(string name)
        {
            int port = Int.Parse(name);
            if (port == -1)
                return null;

            UDPSocket sock = new UDPSocket();
            bool found = sock.Bind((ushort)port);

            if(!found)
                return null;

            uint index = i++;

            m_list[index].InUse = true;
            m_list[index].Socket = sock;

            Node node = new Node();
            node.Flags = NodeFlags.FILE;
            node.Cookie = index;
            node.Read = readImpl;
            node.Write = writeImpl;
            node.GetSize = getSize;
            node.Close = close;

            return node;
        }

        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only header is 6 bytes
            if (size < 6)
                return 0;
            
            UDPSocket sock = m_list[node.Cookie].Socket;
            uint ret = 0;
            if (sock != null)
                ret = sock.ReadPacket((byte*)Util.ObjectToVoidPtr(buffer), size);
            
            return ret;
        }

        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only header is 6 bytes
            if (size < 6)
                return 0;

            UDPSocket sock = m_list[node.Cookie].Socket;
            if (sock != null)
                sock.SendByPacket((byte *)Util.ObjectToVoidPtr(buffer), size);

            return size;
        }

        private static uint getSize(Node node)
        {
            UDPSocket sock = m_list[node.Cookie].Socket;
            if (sock != null)
                return m_list[node.Cookie].Socket.GetSize();

            return 0;
        }

        private static void close(Node node)
        {
            UDPSocket sock = m_list[node.Cookie].Socket;
            sock.Close();
            m_list[node.Cookie].InUse = false;
            // TODO: free socket?
        }
    }
}
