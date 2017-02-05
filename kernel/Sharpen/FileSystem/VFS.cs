using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.MultiTasking;
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
            path = CreateAbsolutePath(path);
            Node node = GetByAbsolutePath(path);
            Heap.Free(path);
            return node;
        }

        /// <summary>
        /// Get node by absolute path
        /// </summary>
        /// <param name="path">The absolute path</param>
        /// <returns>The node</returns>
        public static unsafe Node GetByAbsolutePath(string path)
        {
            int index = String.IndexOf(path, "://");
            int pathLength = path.Length;

            // Get the device name
            string deviceName = path.Substring(0, index);

            // Find first mount
            MountPoint mp = FindMountByName(deviceName);
            Heap.Free(deviceName);
            if (mp == null)
                return null;

            // Possibly add a slash
            bool addedSlash = (path[pathLength - 1] != '/');
            if (addedSlash)
                path = String.Merge(path, "/");

            Node lastNode = mp.Node;
            int parts = String.Count(path, '/') - 2;

            // Loop through the slashes
            int offset = index + 3;
            while (parts > 0 && lastNode != null)
            {
                int newOffset = String.IndexOf(path, "/", offset);

                string part = path.Substring(offset, newOffset - offset);
                lastNode = lastNode.FindDir(lastNode, part);
                Heap.Free(part);

                offset = newOffset + 1;
                parts--;
            }

            if (addedSlash)
                Heap.Free(path);

            if (lastNode == mp.Node)
                return lastNode.Clone();
            else
                return lastNode;
        }

        /// <summary>
        /// Resolves a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The resolved version of the path</returns>
        public static unsafe string ResolvePath(string path)
        {
            int index = String.IndexOf(path, "://");
            int pathLength = path.Length;

            char* ptr = (char*)Heap.Alloc(pathLength + 1);
            Memory.Memcpy(ptr, Util.ObjectToVoidPtr(path), index + 3);

            // Possibly add a slash
            bool addedSlash = (path[pathLength - 1] != '/');
            if (addedSlash)
                path = String.Merge(path, "/");

            int parts = String.Count(path, '/') - 2;

            // Loop through the slashes
            int offset = index + 3;
            int destOffset = index + 3;
            int[] partOffsets = new int[parts + 1];
            partOffsets[0] = destOffset;
            int partIndex = 0;
            while (parts > 0)
            {
                int newOffset = String.IndexOf(path, "/", offset);
                string part = path.Substring(offset, newOffset - offset);

                // "../": Remove previous part
                if (part[0] == '.' && part[1] == '.' && part[2] == '\0')
                {
                    if (partIndex > 0)
                    {
                        partIndex--;
                        destOffset = partOffsets[partIndex];
                        Memory.Memclear(&ptr[destOffset], partOffsets[partIndex + 1] - partOffsets[partIndex]);
                    }
                }
                // Everything else and not "./": add the part
                else if(part[0] != '.' || part[1] != '\0')
                {
                    Memory.Memcpy((void*)((int)ptr + destOffset), Util.ObjectToVoidPtr(part), newOffset - offset);
                    destOffset += newOffset - offset + 1;
                    ptr[destOffset - 1] = '/';
                    partIndex++;
                }

                Heap.Free(part);
                offset = newOffset + 1;
                partOffsets[partIndex] = destOffset;
                parts--;
            }
            
            // Add null character
            ptr[destOffset] = '\0';

            if (addedSlash)
                Heap.Free(path);

            Heap.Free(partOffsets);

            return Util.CharPtrToString(ptr);
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
        public static string CreateAbsolutePath(string path)
        {
            if (Tasking.CurrentTask.CurrentDirectory != null && !IsAbsolutePath(path))
            {
                string merged = String.Merge(Tasking.CurrentTask.CurrentDirectory, path);
                string resolved = ResolvePath(merged);
                Heap.Free(merged);
                return resolved;
            }
            else
            {
                return ResolvePath(path);
            }
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

        /// <summary>
        /// Manipulates underlying parameters in files
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="request">The request</param>
        /// <param name="arg">An optional argument</param>
        /// <returns>The errorcode or return value from IOCtl</returns>
        public static unsafe int IOCtl(Node node, int request, void* arg)
        {
            // TODO: handle general ioctl commands: FIOCLEX, FIONCLEX, FIONBIO, FIONREAD

            if (node.IOCtl == null)
                return 0;

            return node.IOCtl(node, request, arg);
        }
    }
}
