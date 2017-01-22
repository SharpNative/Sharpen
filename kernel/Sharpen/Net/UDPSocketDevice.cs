using Sharpen.Collections;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPSocketDevice
    {

        private static uint i = 0;

        public static void Init()
        {
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
            {
                Heap.Free(portText);
                Heap.Free(ip);

                return null;
            }

            UDPSocket sock = new UDPSocket();
            bool found = sock.Connect(ip, (ushort)port);

            if(!found)
            {
                Heap.Free(portText);
                Heap.Free(ip);

                return null;
            }
            

            Node node = new Node();
            node.Flags = NodeFlags.FILE;
            node.Cookie = (uint)Util.ObjectToVoidPtr(sock);
            node.Read = readImpl;
            node.Write = writeImpl;
            node.GetSize = getSize;
            node.Close = close;

            Heap.Free(portText);
            Heap.Free(ip);

            return node;
        }

        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            UDPSocket sock = (UDPSocket)Util.VoidPtrToObject((void *)node.Cookie);
            
            uint ret = 0;
            if(sock != null)
                ret = sock.Read((byte*)Util.ObjectToVoidPtr(buffer), size);
            
            return ret;
        }

        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            UDPSocket sock = (UDPSocket)Util.VoidPtrToObject((void*)node.Cookie);

            if (sock != null)
                sock.Send((byte *)Util.ObjectToVoidPtr(buffer), size);

            return size;
        }

        private static unsafe uint getSize(Node node)
        {
            UDPSocket sock = (UDPSocket)Util.VoidPtrToObject((void*)node.Cookie);

            if (sock != null)
                return sock.GetSize();

            return 0;
        }

        private static unsafe void close(Node node)
        {
            UDPSocket sock = (UDPSocket)Util.VoidPtrToObject((void*)node.Cookie);

            sock.Close();

            Heap.Free(sock);
        }
    }
}
