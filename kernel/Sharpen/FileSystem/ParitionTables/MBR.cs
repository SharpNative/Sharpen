using System;
using Sharpen.FileSystem.PartitionTables;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem.ParitionTables
{
    class MBR: IPartitionTable
    {

        /// <summary>
        /// Register MBR
        /// </summary>
        public static void Register()
        {
            Disk.RegisterPatitionTable(new MBR());
        }

        /// <summary>
        /// Detect if type is mbr
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public unsafe bool isType(Node node)
        {
            byte[] buf = new byte[512];

            node.Read(node, 0, 512, buf);

            bool isMbr = (buf[510] == 0x55 && buf[511] == 0xAA);

            Heap.Free(buf);

            return isMbr;
        }
    }
}
