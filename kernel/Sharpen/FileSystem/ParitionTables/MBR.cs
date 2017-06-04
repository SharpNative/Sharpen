using Sharpen.FileSystem.PartitionTables;
using Sharpen.Mem;
using Sharpen.Utilities;
using System.Runtime.InteropServices;
using System;
using Sharpen.FileSystem.Cookie;

namespace Sharpen.FileSystem.ParitionTables
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBR_Partition_entry
    {
        public byte BootIndicator;
        public byte StartingHead;
        public ushort StartingSectorCylinder; // 0-5 Sector, 6-15 Cylinder
        public byte SystemID;
        public byte EndingHead;
        public ushort EndingSectorCylinder; // 0-5 Sector, 6-15 Cylinder
        public uint StartingLBA;
        public uint TotalSectors;
    }

    class MBR: IPartitionTable
    {

        public const int UDiskID = 0x1B4;
        public const int PARTITION_OFFSET = 0x1BE;

        public const int BOOT_ACTIVE = 0x80;
        public const int BOOT_INACTIVE = 0x00;

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

        /// <summary>
        /// Read partition from node
        /// </summary>
        /// <param name="entry">Partition entry</param>
        /// <param name="name">Index name</param>
        /// <param name="i">Index</param>
        public unsafe void ReadPartition(Node disk, MBR_Partition_entry *entry, string name, int i)
        {


            // Is entry used?
            if (entry->StartingHead == 0)
                return;

            IMBRCookie cookie = new IMBRCookie();
            cookie.Disk = disk;
            cookie.MaxLBA = (int)entry->TotalSectors;
            cookie.Offset = (int)entry->StartingLBA;
            cookie.Size = cookie.MaxLBA * 512;
            cookie.Type = entry->SystemID;

            // TODO: Fix this!
            string insertion = "p";
            string index = i.ToString();

            string nodeNames = Sharpen.Lib.String.Merge(name, insertion);
            string nodeName = Sharpen.Lib.String.Merge(nodeNames, index);

            Heap.Free(nodeNames);

            Node node = new Node();
            node.Cookie = cookie;
            node.Read = readImpl;
            node.Write = writeImpl;

            RootPoint rp = new RootPoint(nodeName, node);
            VFS.MountPointDevFS.AddEntry(rp);


            Heap.Free(index);
        }

        #region FS implementations

        /// <summary>
        /// Filesystem read implementation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {

            IMBRCookie cookie = (IMBRCookie)node.Cookie;

            if (offset >= cookie.MaxLBA)
                return 0;

            uint offsetOut = (uint)cookie.Offset + offset;

            //Console.WriteLine("READD");
            //Console.WriteHex(size);
            //Console.WriteLine("");
            return cookie.Disk.Read(cookie.Disk, (uint)cookie.Offset + offset, size, buffer);
        }

        /// <summary>
        /// Filesystem write implementation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {

            IMBRCookie cookie = (IMBRCookie)node.Cookie;

            if (offset >= cookie.MaxLBA)
                return 0;

            return cookie.Disk.Write(cookie.Disk, (uint)cookie.Offset + offset, size, buffer);
        }

        #endregion

        /// <summary>
        /// Read partitions from disk
        /// </summary>
        /// <param name="node">NODE</param>
        /// <param name="name">Name</param>
        public unsafe void ReadPartitions(Node node, string name)
        {

            byte[] buf = new byte[512];

            node.Read(node, 0, 512, buf);

            MBR_Partition_entry* entries = (MBR_Partition_entry*)((byte *)Util.ObjectToVoidPtr(buf) + PARTITION_OFFSET);

            for (int i = 0; i < 4; i++)
                ReadPartition(node, entries + i, name, i);
            
            Heap.Free(buf);
        }
    }
}
