namespace Sharpen.Collections
{
    class Dictionary
    {
        private LongIndex m_index = new LongIndex();
        private List m_values = new List();

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            m_index.Clear();
            m_values.Clear();
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

            return m_values.Item[index];
        }

        /// <summary>
        /// Add value by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="val">The value</param>
        public void Add(long key, object val)
        {
            int index = m_index.IndexOf(key);

            if (index == -1)
            {
                m_index.Add(key);
                m_values.Add(val);
            }
        }

        /// <summary>
        /// Removes an object at an index
        /// </summary>
        /// <param name="index">The index</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= m_index.Count)
                return;

            m_index.RemoveAt(index);
            m_values.RemoveAt(index);
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
