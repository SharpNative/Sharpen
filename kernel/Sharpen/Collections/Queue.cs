using Sharpen.Mem;
using Sharpen.Synchronisation;

namespace Sharpen.Collections
{
    public unsafe class Queue
    {
        private unsafe struct stackNode
        {
            public stackNode* Previous;
            public void* Value;
        }

        private stackNode* m_head;
        private stackNode* m_tail;

        private Mutex m_mutex;

        public uint Length { get; private set; }
        
        /// <summary>
        /// Queue
        /// </summary>
        public Queue()
        {
            m_mutex = new Mutex();
        }

        /// <summary>
        /// Returns if the queue is empty
        /// </summary>
        /// <returns>If it's empty</returns>
        public bool IsEmpty()
        {
            m_mutex.Lock();
            bool ret = (Length == 0);
            m_mutex.Unlock();
            return ret;
        }

        /// <summary>
        /// Pushes a new value onto the queue
        /// </summary>
        /// <param name="value">The value</param>
        public void Push(void* value)
        {
            stackNode* node = (stackNode*)Heap.Alloc(sizeof(stackNode));
            node->Value = value;
            node->Previous = null;

            m_mutex.Lock();

            if (Length == 0)
            {
                m_head = node;
            }
            else
            {
                m_tail->Previous = node;
            }

            m_tail = node;
            Length++;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Pops a value from the queue
        /// </summary>
        /// <returns>The value</returns>
        public unsafe void* Pop()
        {
            m_mutex.Lock();

            if (Length == 0)
            {
                m_mutex.Unlock();
                return null;
            }

            stackNode* node = m_head;
            void* ret = node->Value;
            m_head = m_head->Previous;
            Heap.Free(node);

            Length--;

            m_mutex.Unlock();

            return ret;
        }

        /// <summary>
        /// Returns the top element of the Queue without popping it
        /// </summary>
        /// <returns>The top element</returns>
        public unsafe void* Peek()
        {
            m_mutex.Lock();

            if (Length == 0)
            {
                m_mutex.Unlock();
                return null;
            }

            stackNode* node = m_head;
            void* ret = node->Value;

            m_mutex.Unlock();

            return ret;
        }

        /// <summary>
        /// Cleans up this queue
        /// </summary>
        public void Dispose()
        {
            m_mutex.Lock();

            Length = 0;
            while (m_head != null)
            {
                stackNode* prev = m_head->Previous;
                Heap.Free(m_head);
                m_head = prev;
            }

            m_mutex.Unlock();
        }
    }
}
