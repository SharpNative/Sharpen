using Sharpen.Arch;
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

    public unsafe class Task
    {
        public int PID;

        public int GID;
        public int UID;

        public FileDescriptors FileDescriptors;

        public Paging.PageDirectory* PageDir;
        public int* Stack;
        public int* KernelStack;
        public void* FPUContext;
        public void* DataEnd;

        public Task Next;

        public int TimeFull;
        public int TimeLeft;
    }
}
