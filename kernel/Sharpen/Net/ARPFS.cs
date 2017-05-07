using Sharpen.FileSystem;
using Sharpen.Mem;

namespace Sharpen.Net
{
    class ARPFS
    {
        /// <summary>
        /// Initializes the ARP filesystem in NetFS
        /// </summary>
        public static unsafe void Init()
        {
            Node node = new Node();
            node.FindDir = findDirImpl;
            node.ReadDir = readDirImpl;
            node.Flags = NodeFlags.DIRECTORY;

            RootPoint dev = new RootPoint("arp", node);
            VFS.MountPointNetFS.AddEntry(dev);
        }
        
        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            byte[] ip = NetworkTools.StringToIp(name);
            if (ip == null)
                return null;

            byte* dstMac = (byte*)Heap.Alloc(6);
            Memory.Memset(dstMac, 0, 6);

            ARP.Lookup(ip, dstMac);

            if (dstMac[0] == 0x00 && dstMac[1] == 0x00 && dstMac[2] == 0x00 && dstMac[3] == 0x00 && dstMac[4] == 0x00 && dstMac[5] == 0x00)
            {
                byte[] mac = new byte[6];
                for (int i = 0; i < 6; i++)
                    mac[i] = 0xFF;

                ARP.ArpSend(ARP.OP_REQUEST, mac, ip);
                Heap.Free(mac);

                return null;
            }
            Heap.Free(dstMac);

            return new Node();
        }

        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            // We need sprintf for this

            return null;
        }
    }
}
