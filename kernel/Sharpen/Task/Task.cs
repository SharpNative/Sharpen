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

    public enum TaskFlags
    {
        NOFLAGS = 0,
        DESCHEDULED = 1
    }

    public unsafe class Task
    {
        public int PID;

        public int GID;
        public int UID;

        public FileDescriptors FileDescriptors;

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
    }
}
