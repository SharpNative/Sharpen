namespace Sharpen.Collections
{
    class Dictionary
    {
        // TODO: add buckets (?)

        private LongList m_index = new LongList();
        private List m_values = new List();
        private Mutex m_mutex = new Mutex();
        
        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            m_mutex.Lock();
            m_index.Clear();
            m_values.Clear();
            m_mutex.Unlock();
        }

        /// <summary>
        /// Counts the amount of elements
        /// </summary>
        /// <returns>The amount of elements</returns>
        public int Count()
        {
            return m_values.Count;
        }

        /// <summary>
        /// Gets an object at an index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The object</returns>
        public object GetAt(int index)
        {
            if (index < 0 || index >= m_values.Count)
                return null;

            m_mutex.Lock();
            object ret = m_values.Item[index];
            m_mutex.Unlock();
            return ret;
        }

        /// <summary>
        /// Add value by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="val">The value</param>
        public void Add(long key, object val)
        {
            int index = m_index.IndexOf(key);

            m_mutex.Lock();
            if (index == -1)
            {
                m_index.Add(key);
                m_values.Add(val);
            }
            m_mutex.Unlock();
        }

        /// <summary>
        /// Removes an object at an index
        /// </summary>
        /// <param name="index">The index</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= m_index.Count)
                return;

            m_mutex.Lock();
            m_index.RemoveAt(index);
            m_values.RemoveAt(index);
            m_mutex.Unlock();
        }

        /// <summary>
        /// Removes an object using a key
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(long key)
        {
            int index = m_index.IndexOf(key);
            RemoveAt(index);
        }

        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object</returns>
        public object GetByKey(long key)
        {
            int index = m_index.IndexOf(key);
            return GetAt(index);
        }
    }
}
