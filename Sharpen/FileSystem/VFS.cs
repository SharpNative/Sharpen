using Sharpen.Collections;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    class VFS
    {
        private static Dictionary m_dictionary = new Dictionary();

        /// <summary>
        /// Initializes the VFS
        /// </summary>
        public static unsafe void Init()
        {
            MountPoint mountPoint = new MountPoint();
            mountPoint.Name = "mounts";
            mountPoint.Node = new Node();
            mountPoint.Node.ReadDir = readDirImpl;

            AddMountPoint(mountPoint);
        }

        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= m_dictionary.Count())
                return null;
            
            MountPoint dev = (MountPoint)m_dictionary.GetAt((int)index);
            if (dev == null)
                return null;

            DirEntry *entry = (DirEntry *)Heap.Alloc(sizeof(DirEntry));
            int i = 0;
            for (; dev.Name[i] != '\0'; i++)
                entry->Name[i] = dev.Name[i];
            entry->Name[i] = '\0';

            return entry;
        }

        /// <summary>
        /// Generate hash from string
        /// </summary>
        /// <param name="inVal">Name</param>
        /// <returns></returns>
        private static long generateHash(string inVal)
        {
            long hash = 0;

            // There can be 8 chars before the NULL-character
            for (int i = 0; i <= 8; i++)
            {
                char c = inVal[i];
                if (c == '\0')
                    break;

                hash <<= 3;
                hash |= c;
            }

            return hash;
        }

        /// <summary>
        /// Add mount to VFS
        /// </summary>
        /// <param name="mountPoint"></param>
        public static void AddMountPoint(MountPoint mountPoint)
        {
            long key = generateHash(mountPoint.Name);
            m_dictionary.Add(key, mountPoint);
        }

        /// <summary>
        /// Find mount in VFS
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MountPoint FindMountByName(string name)
        {
            long key = generateHash(name);
            return (MountPoint)m_dictionary.GetByKey(key);
        }

        /// <summary>
        /// Get node by path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The node</returns>
        public static unsafe Node GetByPath(string path)
        {
            int index = String.IndexOf(path, "://");
            if (index == -1)
                return null;

            if (path[String.Length(path) - 1] != '/')
                path = String.Merge(path, "/");
            
            string deviceName = String.SubString(path, 0, index);
            string AfterDeviceName = String.SubString(path, index + 3, String.Length(path) - (index + 3));
            int parts = String.Count(AfterDeviceName, '/');
            
            // Find first mount
            MountPoint mp = FindMountByName(deviceName);
            if (mp == null)
                return null;
            
            Node lastNode = mp.Node;

            // TODO: Optimize this process!
            string nodeName = AfterDeviceName;
            string afterNodeName = AfterDeviceName;
            while (parts > 0)
            {
                index = String.IndexOf(afterNodeName, "/");
                
                nodeName = String.SubString(afterNodeName, 0, index);
                afterNodeName = String.SubString(afterNodeName, index + 1, String.Length(path) - 1);
                lastNode = lastNode.FindDir(lastNode, nodeName);
                
                if (lastNode == null)
                    return null;

                parts--;
            }
            
            return lastNode;
        }
    }
}
