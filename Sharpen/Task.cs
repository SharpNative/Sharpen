using Sharpen.Arch;

namespace Sharpen
{
    public unsafe class Task
    {
        public int PID;

        public int GID;
        public int UID;

        public Paging.PageDirectory* PageDir;
        public int* Stack;

        public Task Next;

        public int TimeFull;
        public int TimeLeft;
    }
}
