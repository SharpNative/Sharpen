using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Task
{
    public unsafe class Task
    {
        public const int KERNEL_CS = 0x08;
        public const int KERNEL_DS = 0x10;
        public const int USERSPACE_CS = 0x1B;
        public const int USERSPACE_DS = 0x23;

        public enum TaskFlags
        {
            NOFLAGS = 0,
            DESCHEDULED = 1
        }

        public int PID;
        public int GID;
        public int UID;

        public FileDescriptors FileDescriptors;
        public string CurrentDirectory;

        public TaskFlags Flags;

        public Paging.PageDirectory* PageDir;
        public int* Stack;
        public int* StackStart;
        public int* KernelStack;
        public int* KernelStackStart;
        public void* FPUContext;
        public void* DataEnd;
        public Regs* SysRegs;

        public Task Next;

        public int TimeFull;
        public int TimeLeft;

        /// <summary>
        /// Cleans up this task
        /// </summary>
        public unsafe void Cleanup()
        {
            // TODO: more cleaning required
            Heap.Free(FPUContext);
            Heap.Free(KernelStackStart);
            return;
            Paging.FreeDirectory(PageDir);
        }

        /// <summary>
        /// Clones descriptors to another task
        /// </summary>
        /// <param name="destination">The destination</param>
        public void CloneDescriptorsTo(Task destination)
        {
            // Fresh file descriptors
            if (FileDescriptors.Capacity == 0)
            {
                destination.FileDescriptors.Capacity = 16;
                destination.FileDescriptors.Used = 0;
                destination.FileDescriptors.Nodes = new Node[16];
                destination.FileDescriptors.Offsets = new uint[16];
                return;
            }

            // File descriptors
            destination.FileDescriptors.Capacity = FileDescriptors.Capacity;
            destination.FileDescriptors.Used = FileDescriptors.Used;
            destination.FileDescriptors.Nodes = new Node[destination.FileDescriptors.Capacity];
            destination.FileDescriptors.Offsets = new uint[destination.FileDescriptors.Capacity];

            // Copy file descriptors
            int cap = FileDescriptors.Capacity;
            for (int i = 0; i < cap; i++)
            {
                Node sourceNode = FileDescriptors.Nodes[i];
                if (sourceNode != null)
                {
                    destination.FileDescriptors.Nodes[i] = sourceNode.Clone();
                }
                destination.FileDescriptors.Offsets[i] = FileDescriptors.Offsets[i];
            }
        }

        /// <summary>
        /// Gets a node from the descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <returns>The node</returns>
        public Node GetNodeFromDescriptor(int descriptor)
        {
            if (descriptor >= FileDescriptors.Capacity)
                return null;

            return FileDescriptors.Nodes[descriptor];
        }

        /// <summary>
        /// Gets an offset of a node from the descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <returns>The offset</returns>
        public uint GetOffsetFromDescriptor(int descriptor)
        {
            if (descriptor >= FileDescriptors.Capacity)
                return 0;

            return FileDescriptors.Offsets[descriptor];
        }

        /// <summary>
        /// Adds a node to the file descriptor
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The file descriptor ID</returns>
        public unsafe int AddNodeToDescriptor(Node node)
        {
            if (FileDescriptors.Used == FileDescriptors.Capacity - 1)
            {
                // Expand if needed
                int oldCap = FileDescriptors.Capacity;
                FileDescriptors.Capacity += 8;

                Node[] newNodeArray = new Node[FileDescriptors.Capacity];
                uint[] newOffsetArray = new uint[FileDescriptors.Capacity];

                Memory.Memcpy(Util.ObjectToVoidPtr(newNodeArray), Util.ObjectToVoidPtr(FileDescriptors.Nodes), oldCap * sizeof(void*));
                Memory.Memcpy(Util.ObjectToVoidPtr(newOffsetArray), Util.ObjectToVoidPtr(FileDescriptors.Offsets), oldCap * sizeof(uint));

                FileDescriptors.Nodes = newNodeArray;
                FileDescriptors.Offsets = newOffsetArray;
            }

            // Find a free descriptor
            int i = 0;
            for (; i < FileDescriptors.Capacity; i++)
            {
                if (FileDescriptors.Nodes[i] == null)
                {
                    FileDescriptors.Nodes[i] = node;
                    FileDescriptors.Offsets[i] = 0;
                    break;
                }
            }

            FileDescriptors.Used++;
            return i;
        }
    }
}
