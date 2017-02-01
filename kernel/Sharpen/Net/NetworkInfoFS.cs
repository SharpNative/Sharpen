using Sharpen.FileSystem;
using Sharpen.Lib;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class NetworkInfoFS
    {
        /// <summary>
        /// Initializes the networking info filesystem module
        /// </summary>
        public static unsafe void Init()
        {
            Device dev = new Device();
            dev.Name = "info";
            dev.Node = new Node();
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
            if (name.Equals("ip"))
                return byID(0);
            else if (name.Equals("subnet"))
                return byID(1);
            else if (name.Equals("gateway"))
                return byID(2);
            else if (name.Equals("ns1"))
                return byID(3);
            else if (name.Equals("ns2"))
                return byID(4);

            return null;
        }

        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            byte* sourceBuffer = null;

            switch (node.Cookie)
            {
                // IP
                case 0:
                    sourceBuffer = Network.Settings->IP;
                    break;

                // Subnet
                case 1:
                    sourceBuffer = Network.Settings->Subnet;
                    break;

                // Gateway
                case 2:
                    sourceBuffer = Network.Settings->Gateway;
                    break;

                // NS1
                case 3:
                    sourceBuffer = Network.Settings->DNS1;
                    break;

                // NS2
                case 4:
                    sourceBuffer = Network.Settings->DNS2;
                    break;
            }

            int read = Math.Min((int)size, 4);
            Memory.Memcpy(Util.ObjectToVoidPtr(buffer), sourceBuffer, read);

            return (uint)read;
        }

        /// <summary>
        /// Creates a node by its ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>The node</returns>
        private static unsafe Node byID(uint id)
        {
            Node node = new Node();
            node.Cookie = id;
            node.Read = readImpl;
            node.Size = 4;

            return node;
        }

        /// <summary>
        /// Creates a directory entry by its name
        /// </summary>
        /// <param name="str">The name</param>
        /// <returns>The entry</returns>
        private static unsafe DirEntry* makeByName(string str)
        {
            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            Memory.Memcpy(entry->Name, Util.ObjectToVoidPtr(str), str.Length + 1);

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
            if (index == 0)
                return makeByName("ip");
            else if (index == 1)
                return makeByName("subnet");
            else if (index == 2)
                return makeByName("gateway");
            else if (index == 3)
                return makeByName("ns1");
            else if (index == 4)
                return makeByName("ns2");

            return null;
        }
    }
}
