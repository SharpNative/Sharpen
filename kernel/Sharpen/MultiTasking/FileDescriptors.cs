using Sharpen.Exec;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.MultiTasking
{
    public unsafe class FileDescriptors
    {
        public const int DEFAULT_CAPACITY = 16;

        public int Used { get; private set; } = 0;
        public int Capacity { get; private set; } = DEFAULT_CAPACITY;
        private Node[] Nodes;
        private uint[] Offsets;

        /// <summary>
        /// Constructor of the file descriptors
        /// </summary>
        public FileDescriptors()
        {
            Nodes = new Node[Capacity];
            Offsets = new uint[Capacity];
        }

        /// <summary>
        /// Constructor of the file descriptors
        /// </summary>
        /// <param name="capacity">Capacity</param>
        public FileDescriptors(int capacity)
        {
            Capacity = capacity;
            Nodes = new Node[capacity];
            Offsets = new uint[capacity];
        }

        /// <summary>
        /// Closes a node
        /// </summary>
        /// <param name="descriptor">The file descriptor</param>
        /// <returns>The errorcode</returns>
        public int Close(int descriptor)
        {
            Node node = GetNode(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            VFS.Close(node);

            Nodes[descriptor] = null;
            Used--;

            return 0;
        }

        /// <summary>
        /// Duplicates a file descriptor to the lowest unused file descriptor
        /// </summary>
        /// <param name="fd">The file descriptor to clone</param>
        /// <returns>The cloned file descriptor</returns>
        public int Dup(int fd)
        {
            if (fd < 0 || fd >= Capacity)
                return -(int)ErrorCode.EBADF;

            // Clone the new one, to the next available file descriptor
            Node node = GetNode(fd).Clone();
            return AddNode(node);
        }

        /// <summary>
        /// Replaces an old file descriptor with a new one
        /// </summary>
        /// <param name="oldfd">The old file descriptor</param>
        /// <param name="newfd">The new file descriptor</param>
        /// <returns>The new file descriptor, or an errorcode</returns>
        public int Dup2(int oldfd, int newfd)
        {
            if (oldfd < 0 || newfd < 0 || oldfd >= Capacity || newfd >= Capacity)
                return -(int)ErrorCode.EBADF;

            // If there is an old file descriptor node, close it
            Node old = GetNode(oldfd);
            if (old != null)
                VFS.Close(old);

            // Clone the new one, replacing the old one
            Nodes[oldfd] = Nodes[newfd].Clone();
            return newfd;
        }

        /// <summary>
        /// Gets the node of a file descriptor
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <returns>The node</returns>
        public Node GetNode(int fd)
        {
            if (fd > Capacity || fd < 0)
                return null;

            return Nodes[fd];
        }

        /// <summary>
        /// Gets the offset of a file descriptor
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <returns>The offset</returns>
        public uint GetOffset(int fd)
        {
            if (fd > Capacity || fd < 0)
                return 0;

            return Offsets[fd];
        }

        /// <summary>
        /// Sets the offset of a file descriptor
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <param name="offset">The offset</param>
        public void SetOffset(int fd, uint offset)
        {
            if (fd > Capacity || fd < 0)
                return;

            Offsets[fd] = offset;
        }

        /// <summary>
        /// Clones descriptors
        /// </summary>
        /// <param name="destination">The destination</param>
        public FileDescriptors Clone()
        {
            FileDescriptors clone = new FileDescriptors(Capacity);
            clone.copyFrom(this);
            return clone;
        }

        /// <summary>
        /// Cleans up the file descriptors
        /// </summary>
        public void Cleanup()
        {
            for (int i = 0; i < Used; i++)
            {
                Node node = GetNode(i);
                if (node == null)
                    continue;

                VFS.Close(node);
                Heap.Free(node);
            }
        }

        /// <summary>
        /// Copies file descriptors from another one to this one
        /// </summary>
        /// <param name="source">The source file descriptors</param>
        private void copyFrom(FileDescriptors source)
        {
            Used = source.Used;

            // Copy file descriptors
            int cap = source.Capacity;
            for (int i = 0; i < cap; i++)
            {
                Node sourceNode = source.GetNode(i);
                if (sourceNode != null)
                {
                    Nodes[i] = sourceNode.Clone();
                    Offsets[i] = source.GetOffset(i);
                }
            }
        }

        /// <summary>
        /// Adds a node to the file descriptor
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The file descriptor ID</returns>
        public unsafe int AddNode(Node node)
        {
            if (Used == Capacity - 1)
            {
                // Expand if needed
                int oldCap = Capacity;
                Capacity += 8;

                Node[] newNodeArray = new Node[Capacity];
                uint[] newOffsetArray = new uint[Capacity];

                Memory.Memcpy(Util.ObjectToVoidPtr(newNodeArray), Util.ObjectToVoidPtr(Nodes), oldCap * sizeof(void*));
                Memory.Memcpy(Util.ObjectToVoidPtr(newOffsetArray), Util.ObjectToVoidPtr(Offsets), oldCap * sizeof(uint));

                Heap.Free(Nodes);
                Heap.Free(Offsets);

                Nodes = newNodeArray;
                Offsets = newOffsetArray;
            }

            // Find a free descriptor
            int i = 0;
            for (; i < Capacity; i++)
            {
                if (Nodes[i] == null)
                {
                    Nodes[i] = node;
                    Offsets[i] = 0;
                    break;
                }
            }

            Used++;
            return i;
        }
    }
}
