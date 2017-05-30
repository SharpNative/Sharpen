using Sharpen.Drivers.Char;
using Sharpen.Lib;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Net;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    public enum FileWhence
    {
        SEEK_SET,
        SEEK_CUR,
        SEEK_END
    }

    public class VFS
    {
        // Commonly accessed mountpoints
        public static ContainerFS RootMountPoint { get; private set; }
        public static ContainerFS MountPointDevFS { get; private set; }
        public static ContainerFS MountPointNetFS { get; private set; }

        /// <summary>
        /// Initializes VFS
        /// </summary>
        public static void Init()
        {
            // Container of all mountpoints
            RootMountPoint = new ContainerFS();

            // DevFS
            MountPointDevFS = new ContainerFS();
            RootPoint devMount = new RootPoint("devices", MountPointDevFS.Node);
            RootMountPoint.AddEntry(devMount);

            // NetFS
            MountPointNetFS = new ContainerFS();
            RootPoint netMount = new RootPoint("net", MountPointNetFS.Node);
            RootMountPoint.AddEntry(netMount);

            // Initialize other filesystems
            initFileSystems();

            Console.WriteLine("[VFS] Initialized");
        }

        /// <summary>
        /// Initialize filesystems
        /// </summary>
        private static void initFileSystems()
        {
            RandomFS.Init();
            STDOUT.Init();
            SerialPort.Init();
            NullFS.Init();
            ProcFS.Init();
            PciFS.Init();

            NetworkInfoFS.Init();
            ARPFS.Init();
            TCPFS.Init();
            UDPFS.Init();
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
        /// Get node by path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The node</returns>
        public static unsafe Node GetOffsetNodeByPath(string path, int offset)
        {
            path = CreateAbsolutePath(path);
            Node node = GetByAbsolutePath(path, offset);
            Heap.Free(path);
            return node;
        }

        public static unsafe Node GetByAbsolutePath(string path)
        {
            return GetByAbsolutePath(path, 0);
        }

        /// <summary>
        /// Get node by absolute path
        /// </summary>
        /// <param name="path">The absolute path</param>
        /// <returns>The node</returns>
        public static unsafe Node GetByAbsolutePath(string path, int offsetEnd)
        {
            int index = path.IndexOf("://");
            int pathLength = path.Length;

            // Get the device name
            string deviceName = path.Substring(0, index);

            // Find first mount
            RootPoint point = RootMountPoint.GetEntry(deviceName);
            Heap.Free(deviceName);
            if (point == null)
                return null;

            Node lastNode = point.Node;

            // Possibly add a slash
            bool addedSlash = (path[pathLength - 1] != '/');
            if (addedSlash)
                path = String.Merge(path, "/");
            
            int parts = String.Count(path, '/') - 2;
            parts -= offsetEnd;

            // Loop through the slashes
            int offset = index + 3;
            while (parts > 0 && lastNode != null)
            {
                int newOffset = path.IndexOf('/', offset);

                string part = path.Substring(offset, newOffset - offset);
                lastNode = lastNode.FindDir(lastNode, part);
                Heap.Free(part);

                offset = newOffset + 1;
                parts--;
            }

            if (addedSlash)
                Heap.Free(path);

            if (lastNode == null)
                return null;

            return lastNode.Clone();
        }

        /// <summary>
        /// Resolves a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The resolved version of the path</returns>
        public static unsafe string ResolvePath(string path)
        {
            int index = path.IndexOf("://");
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
                int newOffset = path.IndexOf('/', offset);
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
                    Memory.Memcpy((byte*)ptr + destOffset, Util.ObjectToVoidPtr(part), newOffset - offset);
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
            int index = path.IndexOf("://");
            if (index == -1)
                return false;

            int firstSlashIndex = path.IndexOf('/');
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
            if (IsAbsolutePath(path))
            {
                return ResolvePath(path);
            }
            else
            {
                string merged = String.Merge(Tasking.CurrentTask.CurrentDirectory, path);
                string resolved = ResolvePath(merged);
                Heap.Free(merged);
                return resolved;
            }
        }

        /// <summary>
        /// Opens a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="flags">The flags</param>
        public static void Open(Node node, int flags)
        {
            if (node.IsOpen)
                return;
            
            node.FileMode = (FileMode)(flags & 0x3);
            node.OpenFlags = flags;
            node.Open?.Invoke(node);
        }

        /// <summary>
        /// Create a node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">Filename to create</param>
        public static Node Create(Node node, string name)
        {
            if (node.Create == null)
                return null;

            return node.Create(node, name);
        }


        /// <summary>
        /// Closes a node
        /// </summary>
        /// <param name="node">The node</param>
        public static void Close(Node node)
        {
            if (!node.IsOpen)
                return;

            node.FileMode = FileMode.O_NONE;
            node.Close?.Invoke(node);
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
