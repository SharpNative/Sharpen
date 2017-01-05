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

        public enum TaskFlag
        {
            NOFLAGS = 0,
            DESCHEDULED = 1,
            SLEEPING = 2
        }

        public int PID { get; set; }
        public int GID { get; set; }
        public int UID { get; set; }

        public FileDescriptors FileDescriptors { get; private set; }
        public string CurrentDirectory;

        private TaskFlag m_flags;

        // It's not always the case we can access the PhysicalAddress field of a page directory
        // because it may be unmapped, so we have a reference here
        public Paging.PageDirectory* PageDirVirtual;
        public Paging.PageDirectory* PageDirPhysical;

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
        public static int NextPID = 0;

        /// <summary>
        /// Constructor of task
        /// </summary>
        /// <param name="priority">The priority of the task</param>
        public Task(TaskPriority priority)
        {
            PID = NextPID++;
            FileDescriptors = new FileDescriptors();

            Priority = priority;
            TimeLeft = (int)priority;
        }

        /// <summary>
        /// Checks if the task has a flag
        /// </summary>
        /// <param name="flag">The flag</param>
        /// <returns>If it has the flag</returns>
        public bool HasFlag(TaskFlag flag)
        {
            return ((m_flags & flag) == flag);
        }

        /// <summary>
        /// Adds a flag to the task
        /// </summary>
        /// <param name="flag">The flag</param>
        public void AddFlag(TaskFlag flag)
        {
            m_flags |= flag;
        }

        /// <summary>
        /// Removes a flag from the task
        /// </summary>
        /// <param name="flag">The flag</param>
        public void RemoveFlag(TaskFlag flag)
        {
            m_flags &= ~flag;
        }

        /// <summary>
        /// Cleans up this task
        /// </summary>
        public void Cleanup()
        {
            // TODO: more cleaning required

            FileDescriptors.Cleanup();
            Heap.Free(FileDescriptors);

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
            Heap.Free(FileDescriptors);
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
            m_flags |= TaskFlag.SLEEPING;
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
            if ((m_flags & TaskFlag.SLEEPING) != TaskFlag.SLEEPING)
                return false;

            // If the full ticks are greater than the fullticks we needed to sleep until, we know we're done sleeping
            if (PIT.FullTicks > m_sleepUntilFullTicks)
            {
                m_flags &= ~TaskFlag.SLEEPING;
                return false;
            }

            // If the full ticks are the same, and the subticks are greater, we know we're done sleeping
            if (PIT.FullTicks == m_sleepUntilFullTicks && PIT.SubTicks > m_sleepUntilSubTicks)
            {
                m_flags &= ~TaskFlag.SLEEPING;
                return false;
            }

            return true;
        }
    }
}
