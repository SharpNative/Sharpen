using Sharpen.Collections;
using Sharpen.FileSystem.Filesystems;
using Sharpen.FileSystem.PartitionTables;

namespace Sharpen.FileSystem
{
    enum DiskMountResult
    {
        SUCCESS,
        FS_TYPE_NOT_FOUND,
        MOUNT_POINT_ALREADY_USED,
        INIT_FAIL
    }


    class Disk
    {

        private static StringDictionary mFilesystems;
        private static List mPartitionTables;

        /// <summary>
        /// Initalize init
        /// </summary>
        public static void Init()
        {
            mFilesystems = new StringDictionary(6);
            mPartitionTables = new List();
        }


        #region Parition functions

        /// <summary>
        /// Get partition table by node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Paritiontable type</returns>
        private static IPartitionTable GetParitionTable(Node node)
        {


            return null;
        }

        /// <summary>
        /// Register filesystems
        /// </summary>
        /// <param name="filesystem">Filesystem</param>
        /// <param name="name">Name</param>
        public static void RegisterPatitionTable(IPartitionTable partTable)
        {

            mPartitionTables.Add(partTable);
        }

        #endregion



        #region Filesystem functions

        /// <summary>
        /// Register filesystems
        /// </summary>
        /// <param name="filesystem">Filesystem</param>
        /// <param name="name">Name</param>
        public static void RegisterFilesystem(IFilesystem filesystem, string name)
        {
            if (mFilesystems.Get(name) != null)
                return;

            mFilesystems.Add(name, filesystem);
        }

        #endregion

        #region Static functions

        /// <summary>
        /// Mount root partition to VFS
        /// </summary>
        /// <param name="node">Partition node</param>
        /// <param name="name">Mount name</param>
        /// <param name="fsType">Filesystem type</param>
        /// <returns>Status</returns>
        public static DiskMountResult Mount(Node node, string name, string fsType)
        {

            IFilesystem fs = (IFilesystem) mFilesystems.Get(fsType);

            if (fs == null)
                return DiskMountResult.FS_TYPE_NOT_FOUND;

            Node retNode = fs.Init(node);

            if (retNode == null)
                return DiskMountResult.INIT_FAIL;

            RootPoint point = VFS.RootMountPoint.GetEntry(name);

            if (point != null)
                return DiskMountResult.MOUNT_POINT_ALREADY_USED;

            RootPoint dev = new RootPoint(name, node);
            VFS.RootMountPoint.AddEntry(dev);

            return DiskMountResult.SUCCESS;
        }

        /// <summary>
        /// Read and mount partitions to vfs
        /// </summary>
        /// <param name="node">Disk node</param>
        /// <param name="nodeName">Node name</param>
        public static void InitalizeNode(Node node, string nodeName)
        {
            IPartitionTable table = GetParitionTable(node);
            

                
        }

        #endregion

    }
}
