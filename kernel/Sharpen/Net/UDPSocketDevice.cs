using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPSocketDevice
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
            int foundIndex = String.IndexOf(name, ":");

            if (foundIndex == -1)
                return null;

            string ip = String.SubString(name, 0, foundIndex);
            string portText = String.SubString(name, foundIndex + 1, String.Length(name) - foundIndex - 1);
            
            int port = Int.Parse(portText);
            if (port == -1)
                return null;

            UDPSocket sock = new UDPSocket();
            bool found = sock.Connect(ip, (ushort)port);

            if(!found)
            {
                Heap.Free((void*)Util.ObjectToVoidPtr(portText));
                Heap.Free((void*)Util.ObjectToVoidPtr(ip));

                return null;
            }

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

            //Heap.Free((void *)Util.ObjectToVoidPtr(portText));
            //Heap.Free((void*)Util.ObjectToVoidPtr(ip));

            return node;
        }

        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            UDPSocket sock = m_list[node.Cookie].Socket;
            uint ret = 0;
            if(sock != null)
                ret = sock.Read((byte*)Util.ObjectToVoidPtr(buffer), size);
            
            return ret;
        }

        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            UDPSocket sock = m_list[node.Cookie].Socket;
            if (sock != null)
                sock.Send((byte *)Util.ObjectToVoidPtr(buffer), size);

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
