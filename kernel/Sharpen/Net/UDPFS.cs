using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Net;
using Sharpen.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class UDPFS
    {
        private const uint OPT_LIST = 0;
        private const uint OPT_BIND = 1;
        private const uint OPT_SOCK = 2;

        public static unsafe void Init()
        {

            Device dev = new Device();
            dev.Name = "udp";
            dev.node = new Node();
            dev.node.Cookie = OPT_LIST;
            dev.node.FindDir = findDirImpl;
            dev.node.ReadDir = readDirImpl;
            dev.node.Flags = NodeFlags.DIRECTORY;

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

            return null;
        }

        private static unsafe Node byID(uint id)
        {
            Node node = new Node();
            node.Cookie = id;

            return node;
        }


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
