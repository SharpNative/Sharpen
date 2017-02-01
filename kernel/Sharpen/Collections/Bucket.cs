using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Collections
{
    class Bucket
    {
        public List m_index = new List();
        public List m_values = new List();

        public int Count { get; private set; }

        /// <summary>
        /// Adds a new key-value pair to the bucket
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="val">The value</param>
        public void Add(string key, object val)
        {
            m_index.Add(key);
            m_values.Add(val);
            Count++;
        }

        /// <summary>
        /// Gets the index of a key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The index</returns>
        public int GetIndex(string key)
        {
            uint hash = String.GetHashCode(key);

            // Loop until the correct index has been found
            // First check against the hash, then check against value
            int index = 0;
            bool found = false;
            while (index < Count && !found)
            {
                string currentKey = (string)m_index.Item[index];
                if (String.GetHashCode(currentKey) == hash && currentKey.Equals(key))
                {
                    found = true;
                }
                else
                {
                    index++;
                }
            }
            
            if (!found)
                return -1;

            return index;
        }

        /// <summary>
        /// Gets a value by a key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        public object Get(string key)
        {
            int index = GetIndex(key);
            if (index == -1)
                return null;
            
            return m_values.Item[index];
        }

        /// <summary>
        /// Gets the value at a certain index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The value</returns>
        public object GetAt(int index)
        {
            return m_values.Item[index];
        }

        /// <summary>
        /// Removes a key-value using a key
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(string key)
        {
            int index = GetIndex(key);
            if (index == -1)
                return;

            m_index.RemoveAt(index);
            m_values.RemoveAt(index);
            Count--;
        }

        /// <summary>
        /// Cleans up the bucket memory
        /// </summary>
        public void Dispose()
        {
            Heap.Free(m_index);
            Heap.Free(m_values);
            m_index = null;
            m_values = null;
            Count = 0;
        }
    }
}
