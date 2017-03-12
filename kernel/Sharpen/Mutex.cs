namespace Sharpen
{
    sealed class Mutex
    {
        private int m_lock;

        /// <summary>
        /// Locks the mutex
        /// </summary>
        public unsafe void Lock()
        {
            fixed (int* ptr = &m_lock)
            {
                InternalLock(ptr);
            }
        }

        /// <summary>
        /// Unlocks the mutex
        /// </summary>
        public unsafe void Unlock()
        {
            fixed (int* ptr = &m_lock)
            {
                InternalUnlock(ptr);
            }
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
