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
            DESCHEDULED = 1,
            SLEEPING = 2
        }

        public int PID;
        public int GID;
        public int UID;

        public FileDescriptors FileDescriptors { get; private set; }
        public string CurrentDirectory;

        public TaskFlags Flags { get; set; }

        public Paging.PageDirectory* PageDir;
        public int* Stack;
        public int* StackStart;
        public int* KernelStack;
        public int* KernelStackStart;
        public void* FPUContext;
        public void* DataEnd;
        public Regs* SysRegs;

        public Task Next;

        // Priority times
        public TaskPriority Priority { get; private set; }
        public int TimeLeft { get; set; }

        // Sleeping
        private uint m_sleepUntilFullTicks;
        private uint m_sleepUntilSubTicks;

        // PID counter
        public static int NextPid = 0;

        /// <summary>
        /// Constructor of task
        /// </summary>
        /// <param name="priority">The priority of the task</param>
        public Task(TaskPriority priority)
        {
            PID = NextPid++;
            FileDescriptors = new FileDescriptors();

            Priority = priority;
            TimeLeft = (int)priority;
        }

        /// <summary>
        /// Cleans up this task
        /// </summary>
        public void Cleanup()
        {
            // TODO: more cleaning required
            //FileDescriptors.Cleanup();
            //Heap.Free(FPUContext);
            //Heap.Free(KernelStackStart);
            //Paging.FreeDirectory(PageDir);
        }

        /// <summary>
        /// Replaces the current file descriptors
        /// </summary>
        /// <param name="fileDescriptors">The new file descriptors</param>
        public void SetFileDescriptors(FileDescriptors fileDescriptors)
        {
            FileDescriptors.Cleanup();
            FileDescriptors = fileDescriptors;
        }

        /// <summary>
        /// Sets the task sleeping until a given time
        /// </summary>
        /// <param name="fullTicks">Full ticks</param>
        /// <param name="subTicks">Sub ticks</param>
        /// <returns>The amount of time the task still needs to sleep (only if interrupted)</returns>
        public uint SleepUntil(uint fullTicks, uint subTicks)
        {
            Flags |= TaskFlags.SLEEPING;
            m_sleepUntilFullTicks = fullTicks;
            m_sleepUntilSubTicks = subTicks;
            Tasking.AddToSleepingList(Tasking.CurrentTask);
            Tasking.ManualSchedule();
            return 0;
        }

        /// <summary>
        /// Returns if the task is sleeping
        /// </summary>
        /// <returns>True if the task is sleeping, False if it's not sleeping</returns>
        public bool IsSleeping()
        {
            // If the flag is not set, we know it's not sleeping
            if ((Flags & TaskFlags.SLEEPING) != TaskFlags.SLEEPING)
                return false;

            // If the full ticks are greater than the fullticks we needed to sleep until, we know we're done sleeping
            if (PIT.FullTicks > m_sleepUntilFullTicks)
            {
                Flags &= ~TaskFlags.SLEEPING;
                return false;
            }

            // If the full ticks are the same, and the subticks are greater, we know we're done sleeping
            if (PIT.FullTicks == m_sleepUntilFullTicks && PIT.SubTicks > m_sleepUntilSubTicks)
            {
                Flags &= ~TaskFlags.SLEEPING;
                return false;
            }

            return true;
        }
    }
}
