using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class UDPFS
    {
        private const uint OPT_LIST = 0;
        private const uint OPT_BIND = 1;
        private const uint OPT_SOCK = 2;

        /// <summary>
        /// Initializes UDP filesystem in NetFS
        /// </summary>
        public static unsafe void Init()
        {
            Device dev = new Device();
            dev.Name = "udp";
            dev.Node = new Node();
            dev.Node.Cookie = OPT_LIST;
            dev.Node.FindDir = findDirImpl;
            dev.Node.ReadDir = readDirImpl;
            dev.Node.Flags = NodeFlags.DIRECTORY;

            NetFS.RegisterDevice(dev);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            if (node.Cookie == OPT_LIST)
            {
                if (String.Equals(name, "bind"))
                    return byID(OPT_BIND);
                else if (String.Equals(name, "connect"))
                    return byID(OPT_SOCK);
            }
            else if (node.Cookie == OPT_SOCK)
            {
                return UDPSocketDevice.Open(name);
            }
            else if (node.Cookie == OPT_BIND)
            {
                return UDPBindSocketDevice.Open(name);
            }

            return null;
        }

        /// <summary>
        /// Creates a node
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>The node</returns>
        private static unsafe Node byID(uint id)
        {
            Node node = new Node();
            node.Cookie = id;
            node.Flags = NodeFlags.DIRECTORY;
            node.FindDir = findDirImpl;

            return node;
        }

        /// <summary>
        /// Creates a directory entry with a name
        /// </summary>
        /// <param name="str">The entry name</param>
        /// <returns>The entry</returns>
        private static unsafe DirEntry* makeByName(string str)
        {
            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
            Memory.Memcpy(entry->Name, Util.ObjectToVoidPtr(str), String.Length(str) + 1);
            return entry;
        }
        
        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            // Do list ;)
            if (node.Cookie == OPT_LIST)
            {
                if (index == 0)
                    return makeByName("bind");
                else if (index == 1)
                    return makeByName("connect");
            }

            return null;
        }
    }
}
