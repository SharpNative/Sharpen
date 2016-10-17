using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class NetworkInfoFS
    {

        public static unsafe void Init()
        {

            Device dev = new Device();
            dev.Name = "info";
            dev.node = new Node();
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
            if (String.Equals(name, "ip"))
                return byID(0);
            else if (String.Equals(name, "subnet"))
                return byID(1);
            else if (String.Equals(name, "gateway"))
                return byID(2);
            else if (String.Equals(name, "ns1"))
                return byID(3);
            else if (String.Equals(name, "ns2"))
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
            uint read = 0;

            // ip
            if(node.Cookie == 0)
            {
                for(int i = 0; i < size && i < 4; i++)
                {
                    buffer[i] = Network.Settings->IP[i];
                    read++;
                }
            }
            // subnet
            else if (node.Cookie == 1)
            {
                for (int i = 0; i < size && i < 4; i++)
                {
                    buffer[i] = Network.Settings->Subnet[i];
                    read++;
                }
            }
            // gateway
            else if (node.Cookie == 2)
            {
                for (int i = 0; i < size && i < 4; i++)
                {
                    buffer[i] = Network.Settings->Gateway[i];
                    read++;
                }
            }
            // ns1
            else if (node.Cookie == 3)
            {
                for (int i = 0; i < size && i < 4; i++)
                {
                    buffer[i] = Network.Settings->DNS1[i];
                    read++;
                }
            }
            // ns2
            else if (node.Cookie == 4)
            {
                for (int i = 0; i < size && i < 4; i++)
                {
                    buffer[i] = Network.Settings->DNS2[i];
                    read++;
                }
            }
            
            return read;
        }

        private static unsafe Node byID(uint id)
        {
            Node node = new Node();
            node.Cookie = id;
            node.Read = readImpl;

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
