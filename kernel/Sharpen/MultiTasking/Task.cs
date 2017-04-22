using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Exec;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.MultiTasking
{
    public class Task
    {
        // Flags that a task can hold
        public enum TaskFlag
        {
            NONE = 0,
            DESCHEDULED = 1,
            STOPPED = 2
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
        public string Name { get; set; }
        public string CMDLine { get; set; }

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
                if (old != null)
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
        public Task NextTask { get; set; }

        // Priority times
        public TaskPriority Priority { get; private set; }
        public int TimeLeft { get; set; }

        // Uptime
        public uint Uptime { get { return Time.FullTicks - m_launchTime; } }
        private uint m_launchTime;

        // Signals
        private SignalAction[] m_signalActions;

        // Threading
        public Thread FirstThread { get; private set; }
        public Thread CurrentThread { get; private set; }
        public int ThreadCount { get; private set; }
        public int SleepingThreadCount { get; set; }

        // PID counter
        private static int NextPID = 0;

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

            // Other data
            GID = Tasking.CurrentTask.GID;
            UID = Tasking.CurrentTask.UID;
            m_launchTime = Time.FullTicks;
            Name = "Nameless";
            CMDLine = "";

            // Signals
            m_signalActions = new SignalAction[Signals.NSIG];

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
        /// Sets the signal handler of a signal
        /// </summary>
        /// <param name="signal">The signal</param>
        /// <param name="newact">The new action</param>
        /// <param name="oldact">The old action</param>
        /// <returns>The errorcode</returns>
        public unsafe ErrorCode SetSignalHandler(Signal signal, SignalAction.SigAction* newact, SignalAction.SigAction* oldact)
        {
            if (signal <= 0 || (int)signal >= Signals.NSIG)
                return ErrorCode.EINVAL;

            SignalAction action = m_signalActions[(int)signal];

            // If the handler is NULL, remove this action
            if (newact->Handler == null)
            {
                m_signalActions[(int)signal] = null;
                if (action != null)
                    Heap.Free(action);

                return ErrorCode.SUCCESS;
            }

            // If the action is NULL, allocate new signal action
            if (action == null)
            {
                m_signalActions[(int)signal] = action = new SignalAction((int)signal);
            }

            // Copy to old action
            if (oldact != null)
            {
                fixed (SignalAction.SigAction* ptr = &action.Sigaction)
                    Memory.Memcpy(oldact, ptr, sizeof(SignalAction.SigAction));
            }

            // Set new action
            fixed (SignalAction.SigAction* ptr = &action.Sigaction)
                Memory.Memcpy(ptr, newact, sizeof(SignalAction.SigAction));

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Processes a signal
        /// </summary>
        /// <param name="signal">The signal</param>
        /// <returns>The errorcode</returns>
        public unsafe ErrorCode ProcessSignal(Signal signal)
        {
            if (signal <= 0 || (int)signal >= Signals.NSIG)
                return ErrorCode.EINVAL;
            
            // Get handler, if no handler is set, use default handler
            SignalAction action = m_signalActions[(int)signal];
            if (action == null)
            {
                Signals.DefaultAction defaultAction = Signals.DefaultActions[(int)signal];
                switch (defaultAction)
                {
                    case Signals.DefaultAction.Continue:
                        RemoveFlag(TaskFlag.STOPPED);
                        break;

                    case Signals.DefaultAction.Stop:
                        AddFlag(TaskFlag.STOPPED);
                        // Do a task switch because the task may not run until a continue is received
                        Tasking.Yield();
                        break;

                    case Signals.DefaultAction.Core:
                    case Signals.DefaultAction.Terminate:
                        Console.WriteLine(Signals.SignalNames[(int)signal]);
                        Tasking.RemoveTaskByPID(PID);
                        break;
                }
            }
            else
            {
                if (action.Sigaction.Handler != (void*)Signals.SIG_IGN)
                    CurrentThread.ProcessSignal(action);
            }

            return ErrorCode.SUCCESS;
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
                current.Cleanup();
                Heap.Free(current);
                current = current.NextThread;
            }

            // Cleanup signals
            for (int i = 0; i < Signals.NSIG; i++)
            {
                SignalAction action = m_signalActions[i];
                if (action != null)
                    Heap.Free(action);
            }
            Heap.Free(m_signalActions);

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
        /// Adds a used address to be freed when the task cleans up
        /// </summary>
        /// <param name="address">The address</param>
        public void AddUsedAddress(object address)
        {
            m_usedAddresses.Add(address);
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
                {
                    next.Awake();
                }
                next = next.NextThread;
            }
            while (next != FirstThread);
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

            // Clone signal handlers
            for (int i = 1; i < Signals.NSIG; i++)
            {
                SignalAction original = m_signalActions[i];
                if (original == null)
                    continue;

                newTask.m_signalActions[i] = original.Clone();
            }

            newTask.Context.CloneFrom(Context);
            return newTask;
        }
    }
}
