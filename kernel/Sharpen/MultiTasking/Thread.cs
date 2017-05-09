using Sharpen.Arch;
using Sharpen.Arch.X86;
using Sharpen.Exec;
using Sharpen.Mem;
using Sharpen.Synchronisation;

namespace Sharpen.MultiTasking
{
    public class Thread
    {
        // Flags that a thread can hold
        public enum ThreadFlags
        {
            SLEEPING = 1
        }

        // Sleeping
        private uint m_sleepUntilFullTicks;
        private uint m_sleepUntilSubTicks;

        public IThreadContext Context { get; private set; }
        private ThreadFlags m_flags;

        public Thread NextThread { get; set; }

        public Task OwningTask { get; set; }

        public int TID { get; private set; }

        // Signals
        private ISignalContext m_currentSignalContext;
        private Mutex m_signalMutex;

        // TID counter
        private static int NextTID = 0;

        /// <summary>
        /// Creates a new thread
        /// </summary>
        public Thread()
        {
            TID = NextTID++;
            Context = new X86ThreadContext();
            m_currentSignalContext = null;
            m_signalMutex = new Mutex();
        }

        /// <summary>
        /// Checks if the thread has a flag
        /// </summary>
        /// <param name="flag">The flag</param>
        /// <returns>If it has the flag</returns>
        public bool HasFlag(ThreadFlags flag)
        {
            return ((m_flags & flag) == flag);
        }

        /// <summary>
        /// Adds a flag to the thread
        /// </summary>
        /// <param name="flag">The flag</param>
        public void AddFlag(ThreadFlags flag)
        {
            m_flags |= flag;
        }

        /// <summary>
        /// Removes a flag from the thread
        /// </summary>
        /// <param name="flag">The flag</param>
        public void RemoveFlag(ThreadFlags flag)
        {
            m_flags &= ~flag;
        }

        /// <summary>
        /// Cleans up this thread
        /// </summary>
        public void Cleanup()
        {
            Context.Cleanup();
            Heap.Free(Context);

            if (m_currentSignalContext != null)
            {
                m_currentSignalContext.Dispose();
                Heap.Free(m_currentSignalContext);
            }
            Heap.Free(m_signalMutex);
        }

        /// <summary>
        /// Sets the task sleeping until a given time
        /// </summary>
        /// <param name="fullTicks">Full ticks</param>
        /// <param name="subTicks">Sub ticks</param>
        /// <returns>The amount of time the task still needs to sleep (only if interrupted)</returns>
        public uint SleepUntil(uint fullTicks, uint subTicks)
        {
            m_sleepUntilFullTicks = fullTicks;
            m_sleepUntilSubTicks = subTicks;

            /**
             * If we're the only thread that and we are sleeping, we have nowhere to switch to
             * when we have a taskswitch that happens.
             * This case can only happen if we're the KernelTask with all threads sleeping but this one, so we just wait here
             */
            if (OwningTask == Tasking.KernelTask && OwningTask.ThreadCount == OwningTask.SleepingThreadCount + 1)
            {
                while (!hasSleepTimeExpired())
                {
                    // Wait for all interrupts, not just a task switch...
                    CPU.HLT();
                }
            }
            else
            {
                AddFlag(ThreadFlags.SLEEPING);
                OwningTask.SleepingThreadCount++;

                // Will return when waiting is done
                Tasking.Yield();
            }

            return 0;
        }
        
        /// <summary>
        /// Sleeps some time
        /// </summary>
        /// <param name="seconds">Whole seconds</param>
        /// <param name="usec">Microseconds</param>
        /// <returns>The amount of time the task still needs to sleep</returns>
        public uint Sleep(uint seconds, uint usec)
        {
            // 1,000,000 usec = 1 second
            // 1,000,000 usec = PIT.Frequency subticks
            uint fullTicks = Time.FullTicks + seconds;
            uint subTicks = Time.SubTicks + (Time.TicksPerSecond * usec / 1000000);
            fullTicks += subTicks / Time.TicksPerSecond;
            subTicks %= Time.TicksPerSecond;
            return SleepUntil(fullTicks, subTicks);
        }

        /// <summary>
        /// Returns if the task is sleeping
        /// </summary>
        /// <returns>True if the task is sleeping, False if it's not sleeping</returns>
        public bool IsSleeping()
        {
            return HasFlag(ThreadFlags.SLEEPING);
        }

        /// <summary>
        /// Returns true if the sleeping time has expired
        /// </summary>
        /// <returns>If the sleeping time has expired</returns>
        private bool hasSleepTimeExpired()
        {
            // If the full ticks are greater than the fullticks we needed to sleep until, we know we're done sleeping
            // If the full ticks are the same, and the subticks are greater, we know we're done sleeping
            return ((Time.FullTicks > m_sleepUntilFullTicks) || (Time.FullTicks == m_sleepUntilFullTicks && Time.SubTicks > m_sleepUntilSubTicks));
        }

        /// <summary>
        /// Wakes the thread up
        /// </summary>
        /// <returns>Returns true if the thread has waken up</returns>
        public bool Awake()
        {
            if (hasSleepTimeExpired())
            {
                RemoveFlag(ThreadFlags.SLEEPING);
                OwningTask.SleepingThreadCount--;
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Returns from a signal (restores original context)
        /// </summary>
        public unsafe void ReturnFromSignal()
        {
            m_signalMutex.Lock();

            if (m_currentSignalContext == null)
            {
                m_signalMutex.Unlock();
                return;
            }

            Context.ReturnFromSignal(m_currentSignalContext);

            m_currentSignalContext.Dispose();
            Heap.Free(m_currentSignalContext);
            m_currentSignalContext = null;

            m_signalMutex.Unlock();
        }

        /// <summary>
        /// Processes a signal
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The signal context</returns>
        public void ProcessSignal(SignalAction action)
        {
            while (m_currentSignalContext != null)
                Tasking.Yield();

            m_signalMutex.Lock();
            m_currentSignalContext = Context.ProcessSignal(action);
            m_signalMutex.Unlock();
        }

        /// <summary>
        /// Clones this thread
        /// </summary>
        /// <returns>The clone</returns>
        public Thread Clone()
        {
            Thread thread = new Thread();
            thread.Context.CloneFrom(Context);
            return thread;
        }
    }
}
