using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Task;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    public enum FileWhence
    {
        SEEK_SET = 0,
        SEEK_CUR = 1,
        SEEK_END = 2
    }

    public class VFS
    {
        private static Dictionary m_dictionary;

        /// <summary>
        /// Initializes the VFS
        /// </summary>
        public static unsafe void Init()
        {
            m_dictionary = new Dictionary();

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

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
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
            string afterDeviceName = String.SubString(path, index + 3, String.Length(path) - (index + 3));
            int parts = String.Count(afterDeviceName, '/');

            // Find first mount
            MountPoint mp = FindMountByName(deviceName);
            if (mp == null)
                return null;

            Node lastNode = mp.Node;

            // TODO: Optimize this process!
            string afterNodeName = afterDeviceName;
            int pathLength = String.Length(path);
            while (parts > 0)
            {
                index = String.IndexOf(afterNodeName, "/");
                string nodeName = String.SubString(afterNodeName, 0, index);

                if (parts > 1)
                    afterNodeName = String.SubString(afterNodeName, index + 1, pathLength - 1);

                lastNode = lastNode.FindDir(lastNode, nodeName);

                if (lastNode == null)
                    return null;

                parts--;
            }

            return lastNode;
        }

        /// <summary>
        /// Resolves a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static unsafe string ResolvePath(string path)
        {
            int index = String.IndexOf(path, "://");
            if (index == -1)
                return null;
            
            if (path[String.Length(path) - 1] != '/')
                path = String.Merge(path, "/");

            string deviceName = String.SubString(path, 0, index + 3);
            string afterDeviceName = String.SubString(path, index + 3, String.Length(path) - (index + 3));
            string afterNodeName = afterDeviceName;

            int pathLength = String.Length(afterDeviceName);
            int parts = String.Count(afterDeviceName, '/');

            // At most <parts>
            string[] partArray = new string[parts];
            int arrayIndex = 0;

            // Get the parts all in the array so we can just merge it with a slash
            while (parts > 0)
            {
                index = String.IndexOf(afterNodeName, "/");
                string nodeName = String.SubString(afterNodeName, 0, index);
                
                if (parts > 1)
                    afterNodeName = String.SubString(afterNodeName, index + 1, pathLength - 1);

                // Special character
                if (nodeName[0] == '.')
                {
                    // Nothing happens if this is only a dot, just the current directory entry
                    // Except if another dot follows
                    if (nodeName[1] == '.' && nodeName[2] == '\0')
                    {
                        if (arrayIndex > 0)
                        {
                            partArray[arrayIndex] = partArray[arrayIndex + 1];
                            arrayIndex--;
                        }
                    }
                }
                // Nothing special
                else
                {
                    partArray[arrayIndex++] = nodeName;
                }

                parts--;
            }

            return String.MergeArray(partArray, arrayIndex, "/", deviceName);
        }
        
        /// <summary>
        /// Checks if a path is an absolute path
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>If it's an absolute path</returns>
        public static bool IsAbsolutePath(string path)
        {
            int index = String.IndexOf(path, "://");
            if (index == -1)
                return false;

            int firstSlashIndex = String.IndexOf(path, "/");
            if (firstSlashIndex > -1 && firstSlashIndex < index - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the absolute path from the relative path
        /// </summary>
        /// <param name="path">The relative path</param>
        /// <returns>The absolute path</returns>
        public static string GetAbsolutePath(string path)
        {
            // Check if it's a relative path
            int deviceNameIndex = String.IndexOf(path, "://");
            if (deviceNameIndex == -1)
            {
                // Current directory
                string currentDir = Tasking.CurrentTask.CurrentDirectory;
                int currentDirLength = String.Length(currentDir);

                // Check if the directory name ends with a slash
                // If not, make sure we put a slash between the current directory and the new directory
                string merged = null;
                if (currentDir[currentDirLength - 1] != '/')
                {
                    merged = String.Merge(currentDir, "/", path);
                }
                else
                {
                    merged = String.Merge(currentDir, path);
                }

                path = ResolvePath(merged);
            }
            // Absolute path
            else
            {
                path = ResolvePath(path);
            }

            return path;
        }

        /// <summary>
        /// Opens a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="flags">The flags</param>
        public static void Open(Node node, int flags)
        {
            node.FileMode = (FileMode)(flags & 0x3);
            node.OpenFlags = flags;

            if (node.Open == null)
                return;

            node.Open(node);
        }

        /// <summary>
        /// Closes a node
        /// </summary>
        /// <param name="node">The node</param>
        public static void Close(Node node)
        {
            node.FileMode = FileMode.O_NONE;

            if (node.Close == null)
                return;

            node.Close(node);
        }

        /// <summary>
        /// Reads data from a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer to put the data into</param>
        /// <returns>The amount of read bytes</returns>
        public static uint Read(Node node, uint offset, uint size, byte[] buffer)
        {
            if (node.Read == null)
                return 0;

            if (node.FileMode != FileMode.O_RDWR && node.FileMode != FileMode.O_RDONLY)
                return 0;

            return node.Read(node, offset, size, buffer);
        }

        /// <summary>
        /// Truncate data to a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer to get the data from</param>
        /// <returns>The amount of written bytes</returns>
        public static uint Truncate(Node node, uint size)
        {
            if (node.Truncate == null)
                return 0;

            if (node.FileMode == FileMode.O_RDONLY)
                return 0;

            return node.Truncate(node, size);
        }


        /// <summary>
        /// Writes data to a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer to get the data from</param>
        /// <returns>The amount of written bytes</returns>
        public static uint Write(Node node, uint offset, uint size, byte[] buffer)
        {
            if (node.Write == null)
                return 0;

            if (node.FileMode != FileMode.O_RDWR && node.FileMode != FileMode.O_WRONLY)
                return 0;

            return node.Write(node, offset, size, buffer);
        }

        /// <summary>
        /// Finds a node in another node (directory node)
        /// </summary>
        /// <param name="node">The directory node</param>
        /// <param name="name">The filename</param>
        /// <returns>The found node</returns>
        public static Node FindDir(Node node, string name)
        {
            if (node.FindDir == null)
                return null;

            return node.FindDir(node, name);
        }

        /// <summary>
        /// Finds a directory entry in another node (directory node)
        /// </summary>
        /// <param name="node">The directory node</param>
        /// <param name="index">The file index</param>
        /// <returns>The found directory entry</returns>
        public static unsafe DirEntry* ReadDir(Node node, uint index)
        {
            if (node.ReadDir == null)
                return null;

            return node.ReadDir(node, index);
        }

        /// <summary>
        /// Gets the size of a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>Its size</returns>
        public static unsafe uint GetSize(Node node)
        {
            if (node.GetSize == null)
                return node.Size;

            return node.GetSize(node);
        }
    }
}
