using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Collections
{
    class StringDictionary
    {
        private Mutex m_mutex = new Mutex();

        private int m_bucketCount;
        private Bucket[] m_buckets;

        public int Count { get; private set; }
        
        /// <summary>
        /// Initializes a dictionary
        /// </summary>
        /// <param name="buckets">The amount of buckets</param>
        public StringDictionary(int buckets)
        {
            m_bucketCount = buckets;
            m_buckets = new Bucket[buckets];
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            m_mutex.Lock();

            for (int i = 0; i < m_bucketCount; i++)
            {
                if (m_buckets[i] != null)
                {
                    m_buckets[i].Dispose();
                    Heap.Free(m_buckets[i]);
                    m_buckets[i] = null;
                }
            }

            Count = 0;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Add value by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="val">The value</param>
        public void Add(string key, object val)
        {
            uint hash = String.GetHashCode(key);
            uint bucket = (uint)(hash % m_bucketCount);

            m_mutex.Lock();

            // Check if bucket exists
            if (m_buckets[bucket] == null)
                m_buckets[bucket] = new Bucket();

            m_buckets[bucket].Add(key, val);

            Count++;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Removes an object using a key
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(string key)
        {
            uint hash = String.GetHashCode(key);
            uint bucket = (uint)(hash % m_bucketCount);

            m_mutex.Lock();

            // Check if bucket exists
            if (m_buckets[bucket] == null)
            {
                m_mutex.Unlock();
                return;
            }

            m_buckets[bucket].Remove(key);

            Count--;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object</returns>
        public object Get(string key)
        {
            uint hash = String.GetHashCode(key);
            uint bucket = (uint)(hash % m_bucketCount);

            m_mutex.Lock();

            // Check if bucket exists
            if (m_buckets[bucket] == null)
            {
                m_mutex.Unlock();
                return null;
            }

            object val = m_buckets[bucket].Get(key);

            m_mutex.Unlock();

            return val;
        }

        /// <summary>
        /// Gets a value at a certain index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The value</returns>
        public object GetAt(int index)
        {
            int currentIndex = 0;
            for (int i = 0; i < m_bucketCount; i++)
            {
                Bucket bucket = m_buckets[i];
                if (bucket == null)
                    continue;

                int offset = index - currentIndex;
                if (offset < bucket.Count)
                    return bucket.GetAt(offset);
                
                currentIndex += bucket.Count;
            }

            return null;
        }
    }
}
