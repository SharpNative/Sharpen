using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Collections
{
    public class List
    {
        private const int DefaultCapacity = 8;

        private int m_currentCap = DefaultCapacity;

        private Mutex m_mutex;
        
        // Array of items
        public object[] Item { get; private set; }

        /// <summary>
        /// The amount of items currently in the list
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The current capacity before a resize is required
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_currentCap;
            }

            set
            {
                m_mutex.Lock();
                resize(value);
                m_currentCap = value;
                m_mutex.Unlock();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public List()
        {
            Item = new object[m_currentCap];
            m_mutex = new Mutex();
        }

        /// <summary>
        /// Resizes to the new capacity
        /// </summary>
        /// <param name="newCapacity">The new capacity</param>
        private unsafe void resize(int newCapacity)
        {
            object[] newArray = new object[newCapacity];
            int length = (newCapacity < m_currentCap) ? newCapacity : m_currentCap;
            CopyTo(0, newArray, 0, length);
            object[] oldArray = Item;
            Item = newArray;
            Heap.Free(oldArray);
        }

        /// <summary>
        /// Ensures there is enough capacity to hold the elements
        /// </summary>
        /// <param name="required">How much capacity is required</param>
        private void ensureCapacity(int required)
        {
            if (Item == null || required < m_currentCap)
                return;
            
            Capacity *= 2;
        }

        /// <summary>
        /// Adds an object to the list
        /// </summary>
        /// <param name="o">The object</param>
        public void Add(object o)
        {
            if (Item == null)
                return;
            
            ensureCapacity(Count + 1);
            m_mutex.Lock();
            Item[Count++] = o;
            m_mutex.Unlock();
        }

        /// <summary>
        /// Removes an object at a given index
        /// </summary>
        /// <param name="index">The index</param>
        public unsafe void RemoveAt(int index)
        {
            // Check if inside bounds
            if (index < 0 || index >= Count)
                return;

            if (Item == null)
                return;

            m_mutex.Lock();
            
            // Copy
            int destination = (int)Util.ObjectToVoidPtr(Item) + (index * sizeof(void*));
            int source = (int)Util.ObjectToVoidPtr(Item) + ((index + 1) * sizeof(void*));
            Memory.Memcpy((void*)destination, (void*)source, (Count - index - 1) * sizeof(void*));
            
            // Decrease capacity if the list has enough free space
            Count--;
            if (Count * DefaultCapacity < Capacity && Capacity > DefaultCapacity)
                Capacity /= 2;

            m_mutex.Unlock();
        }

        /// <summary>
        /// Called when the object needs to cleanup its memory
        /// </summary>
        public void Dispose()
        {
            Heap.Free(Item);
            Heap.Free(m_mutex);
            Item = null;
            m_mutex = null;
        }

        /// <summary>
        /// Removes all the items
        /// </summary>
        public unsafe void Clear()
        {
            if (Item == null)
                return;

            m_mutex.Lock();
            Memory.Memset(Util.ObjectToVoidPtr(Item), 0, m_currentCap * sizeof(void*));
            Count = 0;
            m_mutex.Unlock();
        }

        /// <summary>
        /// Checks if the list contains an item
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <returns>Of the list contains the item</returns>
        public bool Contains(object item)
        {
            m_mutex.Lock();
            for (int i = 0; i < Count; i++)
            {
                if (Item[i] == item)
                {
                    m_mutex.Unlock();
                    return true;
                }
            }

            m_mutex.Unlock();
            return false;
        }

        /// <summary>
        /// Copies the entire list to a one-dimensional array
        /// </summary>
        /// <param name="array">The target array</param>
        public void CopyTo(object[] array)
        {
            CopyTo(0, array, 0, Count);
        }

        /// <summary>
        /// Copies the entire list to a one-dimensional array starting at a specified index of the target array
        /// </summary>
        /// <param name="array">The target array</param>
        /// <param name="arrayIndex">The target array index</param>
        public void CopyTo(object[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Copies a range of the list to a one-dimensional array
        /// </summary>
        /// <param name="index">The starting index</param>
        /// <param name="array">The target array</param>
        /// <param name="arrayIndex">The target array index</param>
        /// <param name="count">The count of how much to copy</param>
        public unsafe void CopyTo(int index, object[] array, int arrayIndex, int count)
        {
            if (Item == null)
                return;

            int destination = (int)Util.ObjectToVoidPtr(array) + (sizeof(void*) * arrayIndex);
            int source = (int)Util.ObjectToVoidPtr(Item) + (sizeof(void*) * index);
            Memory.Memcpy((void*)destination, (void*)source, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence
        /// </summary>
        /// <param name="item">The item to look for</param>
        public int IndexOf(object item)
        {
            return IndexOf(item, 0, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence with a starting index
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index</param>
        public int IndexOf(object item, int index)
        {
            return IndexOf(item, index, Count - index);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence with a starting index and a count
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index in the list</param>
        /// <param name="count">The count</param>
        /// <returns>-1 if not found, the index if found</returns>
        public int IndexOf(object item, int index, int count)
        {
            m_mutex.Lock();
            for (int i = index; i < Count && i < count + index; i++)
            {
                if (Item[i] == item)
                {
                    m_mutex.Unlock();
                    return i;
                }
            }

            m_mutex.Unlock();
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the last occurrence
        /// </summary>
        /// <param name="item">The item to look for</param>
        public int LastIndexOf(object item)
        {
            return LastIndexOf(item, Count - 1, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the last occurrence with a starting index
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index</param>
        public int LastIndexOf(object item, int index)
        {
            return LastIndexOf(item, index, Count - index);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the last occurrence with a starting index and a count
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index in the list</param>
        /// <param name="count">The count</param>
        /// <returns>-1 if not found, the index if found</returns>
        public int LastIndexOf(object item, int index, int count)
        {
            m_mutex.Lock();
            for (int i = index; i >= 0 && i - index < count; i--)
            {
                if (Item[i] == item)
                {
                    m_mutex.Unlock();
                    return i;
                }
            }

            m_mutex.Unlock();
            return -1;
        }
    }
}
