using Sharpen.Arch;

namespace Sharpen.Task
{
    public unsafe class Task
    {
        public int PID;

        public int GID;
        public int UID;

        public Paging.PageDirectory* PageDir;
        public int* Stack;
        public void* FPUContext;

        public Task Next;

        public int TimeFull;
        public int TimeLeft;
    }
}
