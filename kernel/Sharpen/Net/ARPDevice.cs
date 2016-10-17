using Sharpen.FileSystem;
using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class ARPDevice
    {
        public static unsafe void Init()
        {

            Device dev = new Device();
            dev.Name = "arp";
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
