using Sharpen.Mem;

namespace Sharpen.Collections
{
    public unsafe class Queue
    {
        private unsafe struct stackNode
        {
            public stackNode* Next;
            public void* Value;
        }

        private stackNode* m_next;
        private stackNode* m_last;

        private Mutex m_mutex;

        public uint Length { get; private set; }

        /// <summary>
        /// Queue
        /// </summary>
        public Queue()
        {
            m_last = m_next = null;
        }

        /// <summary>
        /// Returns if the queue is empty
        /// </summary>
        /// <returns>If it's empty</returns>
        public bool IsEmpty()
        {
            return (Length == 0);
        }

        /// <summary>
        /// Pushes a new value onto the queue
        /// </summary>
        /// <param name="value">The value</param>
        public void Push(void* value)
        {
            stackNode* node = (stackNode*)Heap.Alloc(sizeof(stackNode));
            node->Value = value;

            m_mutex.Lock();

            if (Length == 0)
                m_last = null;

            node->Next = m_last;

            m_last = node;

            Length++;
            if (m_next == null)
                m_next = m_last;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Pops a value from the queue
        /// </summary>
        /// <returns>The value</returns>
        public unsafe void* Pop()
        {
            m_mutex.Lock();

            if (m_next == null || Length == 0)
            {
                m_mutex.Unlock();
                return null;
            }

            stackNode* node = m_next;
            m_next = node->Next;
            void* ret = node->Value;

            Heap.Free(node);

            Length--;

            m_mutex.Unlock();

            return ret;
        }
    }
}
