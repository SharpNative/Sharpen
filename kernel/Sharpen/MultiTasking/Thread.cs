﻿using Sharpen.Arch;
using Sharpen.Arch.X86;
using Sharpen.Mem;

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

        public Thread NextThread;
        
        public Task OwningTask { get; set; }

        public int TID { get; private set; }

        // TID counter
        public static int NextTID = 0;
        
        /// <summary>
        /// Creates a new thread
        /// </summary>
        public Thread()
        {
            TID = NextTID++;
            Context = new X86ThreadContext();
        }

        /// <summary>
        /// Cleans up this thread
        /// </summary>
        public void Cleanup()
        {
            Context.Cleanup();
            Heap.Free(Context);
        }

        /// <summary>
        /// Sets the task sleeping until a given time
        /// </summary>
        /// <param name="fullTicks">Full ticks</param>
        /// <param name="subTicks">Sub ticks</param>
        /// <returns>The amount of time the task still needs to sleep (only if interrupted)</returns>
        public uint SleepUntil(uint fullTicks, uint subTicks)
        {
            m_flags |= ThreadFlags.SLEEPING;
            m_sleepUntilFullTicks = fullTicks;
            m_sleepUntilSubTicks = subTicks;
            OwningTask.SleepingThreadCount++;
            Tasking.ManualSchedule();
            return 0;
        }

        /// <summary>
        /// Returns if the task is sleeping
        /// </summary>
        /// <returns>True if the task is sleeping, False if it's not sleeping</returns>
        public bool IsSleeping()
        {
            return ((m_flags & ThreadFlags.SLEEPING) == ThreadFlags.SLEEPING);
        }

        /// <summary>
        /// Wakes the thread up
        /// </summary>
        public void Awake()
        {
            // If the full ticks are greater than the fullticks we needed to sleep until, we know we're done sleeping
            if (PIT.FullTicks > m_sleepUntilFullTicks)
            {
                m_flags &= ~ThreadFlags.SLEEPING;
                OwningTask.SleepingThreadCount--;
            }

            // If the full ticks are the same, and the subticks are greater, we know we're done sleeping
            if (PIT.FullTicks == m_sleepUntilFullTicks && PIT.SubTicks > m_sleepUntilSubTicks)
            {
                m_flags &= ~ThreadFlags.SLEEPING;
                OwningTask.SleepingThreadCount--;
            }
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