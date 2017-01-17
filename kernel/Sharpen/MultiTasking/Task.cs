using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.MultiTasking
{
    public class Task
    {
        // Flags that a task can hold
        public enum TaskFlag
        {
            DESCHEDULED = 1
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

        // Used addresses that need cleanup when ending the task
        private List m_usedAddresses;

        // Next task in the linked list
        public Task NextTask;

        // Priority times
        public TaskPriority Priority { get; private set; }
        public int TimeLeft { get; set; }

        // PID counter
        public static int NextPID = 0;

        // Threading
        public Thread FirstThread { get; private set; }
        public Thread CurrentThread { get; private set; }
        public int ThreadCount { get; private set; }
        public int SleepingThreadCount { get; set; }

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

            // Filesystem related initialization if we're not a kernel task
            if ((flags & SpawnFlags.KERNEL_TASK) != SpawnFlags.KERNEL_TASK)
            {
                SetFileDescriptors(Tasking.CurrentTask.FileDescriptors.Clone());
                CurrentDirectory = String.Clone(Tasking.CurrentTask.CurrentDirectory);
            }

            // List of addresses that need to be freed
            m_usedAddresses = new List();
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
            // Cleanup child threads
            Thread current = FirstThread;
            while (current.NextThread != FirstThread)
            {
                Thread next = current.NextThread;
                current.Cleanup();
                Heap.Free(current);
                current = next;
            }

            // Cleanup virtual addresses claimed by this task
            int count = m_usedAddresses.Count;
            for (int i = 0; i < count; i++)
            {
                Heap.Free(m_usedAddresses.Item[i]);
            }
            m_usedAddresses.Dispose();
            Heap.Free(m_usedAddresses);

            // Filesystem stuff
            FileDescriptors.Cleanup();
            Heap.Free(FileDescriptors);
            Heap.Free(m_currentDirectory);

            // Context
            Context.Cleanup();
            Heap.Free(Context);
        }

        /// <summary>
        /// Returns if the task is sleeping (which means that all the threads are sleeping)
        /// </summary>
        /// <returns>If the task is sleeping</returns>
        public bool IsSleeping()
        {
            return (SleepingThreadCount == ThreadCount);
        }
        
        /// <summary>
        /// Adds a used address to be freed when the task cleans up
        /// </summary>
        /// <param name="address">The address</param>
        public unsafe void AddUsedAddress(void* address)
        {
            m_usedAddresses.Add(Util.VoidPtrToObject(address));
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
        /// Awakes sleeping threads
        /// </summary>
        public void AwakeThreads()
        {
            Thread next = CurrentThread.NextThread;
            do
            {
                if (next.IsSleeping())
                    next.Awake();
            }
            while (next.NextThread != FirstThread);
        }

        /// <summary>
        /// Switches to the next thread
        /// </summary>
        public void SwitchToNextThread()
        {
            Thread next = CurrentThread.NextThread;
            while (next.IsSleeping() && next.NextThread != FirstThread)
            {
                next = next.NextThread;
            }

            CurrentThread = next;
        }

        /// <summary>
        /// Stores the current thread context
        /// </summary>
        /// <param name="currentStack">The current stack</param>
        public unsafe void StoreThreadContext(void* currentStack)
        {
            CurrentThread.Context.StoreContext(currentStack);
        }

        /// <summary>
        /// Restores a previous thread context
        /// </summary>
        /// <returns>The previous stack</returns>
        public unsafe void* RestoreThreadContext()
        {
            return CurrentThread.Context.RestoreContext();
        }

        /// <summary>
        /// Prepares this task context
        /// </summary>
        public void PrepareContext()
        {
            Context.PrepareContext();
        }

        /// <summary>
        /// Forks this current task (actually takes the current thread)
        /// </summary>
        /// <returns>The new PID (zero for child, >0 for childs PID)</returns>
        public int Fork()
        {
            return Tasking.SetForkingThread(CurrentThread);
        }

        /// <summary>
        /// Adds a thread to the task
        /// </summary>
        /// <param name="thread">The thread</param>
        public void AddThread(Thread thread)
        {
            ThreadCount++;
            thread.OwningTask = this;

            if (FirstThread == null)
            {
                thread.NextThread = thread;
                FirstThread = thread;
                CurrentThread = thread;
            }
            else
            {
                Thread current = CurrentThread;
                while (current.NextThread != FirstThread)
                {
                    current = current.NextThread;
                }
                thread.NextThread = FirstThread;
                current.NextThread = thread;
            }
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
