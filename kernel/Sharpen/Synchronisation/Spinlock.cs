namespace Sharpen.Synchronisation
{
    sealed class Spinlock
    {
        private int m_lock;
        
        /// <summary>
        /// Locks the spinlock
        /// </summary>
        public unsafe void Lock()
        {
            fixed (int* ptr = &m_lock)
            {
                InternalLock(ptr);
            }
        }

        /// <summary>
        /// Unlocks the spinlock
        /// </summary>
        public unsafe void Unlock()
        {
            fixed (int* ptr = &m_lock)
            {
                InternalUnlock(ptr);
            }
        }

        /// <summary>
        /// Returns if the spinlock is locked
        /// </summary>
        /// <returns>If the spinlock is locked</returns>
        public bool IsLocked()
        {
            return (m_lock == 1);
        }

        /// <summary>
        /// Internal locking method
        /// </summary>
        /// <param name="x">The lock pointer</param>
        public static unsafe extern void InternalLock(int* x);

        /// <summary>
        /// Internal unlocking method
        /// </summary>
        /// <param name="x">The lock pointer</param>
        public static unsafe extern void InternalUnlock(int* x);
    }
}
