using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.MultiTasking
{
    public unsafe class Task
    {
        // Flags that a task can hold
        public enum TaskFlag
        {
            DESCHEDULED = 1,
            SLEEPING = 2
        }

        // Flags that can be used when a task spawns
        public enum SpawnFlags
        {
            NONE = 0,
            SWAP_PID = 1,
            KERNEL_TASK = 2
        }

        public int PID { get; private set; }
        public int GID { get; private set; }
        public int UID { get; private set; }

        public FileDescriptors FileDescriptors { get; private set; }

        // Current working directory
        public string CurrentDirectory
        {
            get
            {
                return m_currentDirectory;
            }

            set
            {
                string old = m_currentDirectory;
                m_currentDirectory = value;
                Heap.Free(old);
            }
        }

        private TaskFlag m_flags;
        private string m_currentDirectory;

        // Context
        public IContext Context { get; private set; }

        // Next task in the linked list
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
        /// <param name="flags">The spawn flags</param>
        public Task(TaskPriority priority, SpawnFlags flags)
        {
            PID = NextPID++;
            FileDescriptors = new FileDescriptors();

            Priority = priority;
            TimeLeft = (int)priority;

            // Check spawn flags
            if ((flags & SpawnFlags.SWAP_PID) == SpawnFlags.SWAP_PID)
            {
                int old = PID;
                PID = Tasking.CurrentTask.PID;
                Tasking.CurrentTask.PID = old;
            }

            // UID & GID
            GID = Tasking.CurrentTask.GID;
            UID = Tasking.CurrentTask.UID;

            // Context
            Context = new X86Context();

            // FS related stuff
            if ((flags & SpawnFlags.KERNEL_TASK) != SpawnFlags.KERNEL_TASK)
            {
                SetFileDescriptors(Tasking.CurrentTask.FileDescriptors.Clone());
                CurrentDirectory = String.Clone(Tasking.CurrentTask.CurrentDirectory);
            }
        }
        
        /// <summary>
        /// Stores the context of a task
        /// </summary>
        /// <param name="regsPtr">The pointer to registers</param>
        public void StoreContext(Regs* regsPtr)
        {
            Context.StoreContext(regsPtr);
        }

        /// <summary>
        /// Restores the context of a task
        /// </summary>
        /// <returns>The stack pointer</returns>
        public void* RestoreContext()
        {
            return Context.RestoreContext();
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

            //FileDescriptors.Cleanup();
            //Heap.Free(FileDescriptors);

            //Context.Cleanup();
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

        /// <summary>
        /// Clones this task
        /// </summary>
        /// <returns>The clone</returns>
        public Task Clone()
        {
            Task newTask = new Task(Priority, SpawnFlags.NONE);
            newTask.Context.CloneFrom(Context);
            return newTask;
        }
    }
}
