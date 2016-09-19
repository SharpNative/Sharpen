using Sharpen.FileSystem;

namespace Sharpen.Task
{
    public unsafe struct FileDescriptors
    {
        public int Used;
        public int Capacity;
        public Node[] Nodes;
        public uint[] Offsets;
    }
}
