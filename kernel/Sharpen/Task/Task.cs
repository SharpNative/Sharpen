using Sharpen.Arch;
using Sharpen.Mem;

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

        // PID counter
        public static int NextPid = 0;

        /// <summary>
        /// Constructor of task
        /// </summary>
        public Task()
        {
            PID = NextPid++;
            FileDescriptors = new FileDescriptors();
        }

        /// <summary>
        /// Cleans up this task
        /// </summary>
        public unsafe void Cleanup()
        {
            // TODO: more cleaning required
            return;
            FileDescriptors.Cleanup();
            Heap.Free(FPUContext);
            Heap.Free(KernelStackStart);
            Paging.FreeDirectory(PageDir);
        }
    }
}
