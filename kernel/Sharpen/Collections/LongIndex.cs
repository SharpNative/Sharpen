using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Collections
{
    public class LongIndex
    {
        // Default capacity of 4
        private int m_currentCap = 4;

        // Array of items
        public long[] Item { get; private set; }

        /// <summary>
        /// The amount of items currently in the list
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// The current capacity before a resize is required
        /// </summary>
        public unsafe int Capacity
        {
            get
            {
                return m_currentCap;
            }

            set
            {
                long[] newArray = new long[value];
                Memory.Memcpy(Util.ObjectToVoidPtr(newArray), Util.ObjectToVoidPtr(Item), (m_currentCap + 1) * sizeof(long));
                Item = newArray;
                m_currentCap = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LongIndex()
        {
            Item = new long[m_currentCap];
        }

        /// <summary>
        /// Ensures there is enough capacity to hold the elements
        /// </summary>
        /// <param name="required">How much capacity is required</param>
        private void EnsureCapacity(int required)
        {
            if (required < m_currentCap)
                return;
            
            Capacity *= 2;
        }

        /// <summary>
        /// Adds an object to the list
        /// </summary>
        /// <param name="o">The object</param>
        public void Add(long o)
        {
            EnsureCapacity(Count + 1);
            Item[Count++] = o;
        }

        /// <summary>
        /// Removes an object at a given index
        /// </summary>
        /// <param name="index">The index</param>
        public unsafe void RemoveAt(int index)
        {
            // Check if inside bounds
            if (index >= Count)
                return;

            // Copy
            int destination = (int)Util.ObjectToVoidPtr(Item) + (index * sizeof(long));
            int source = (int)Util.ObjectToVoidPtr(Item) + ((index + 1) * sizeof(long));
            Memory.Memcpy((void*)destination, (void*)source, (Count - index - 1) * sizeof(long));

            // Decrease capacity if the list has enough free space
            Count--;
            if (Count * 2 < Capacity)
                Capacity /= 2;
        }

        /// <summary>
        /// Removes all the items
        /// </summary>
        public unsafe void Clear()
        {
            Memory.Memset(Util.ObjectToVoidPtr(Item), 0, m_currentCap * sizeof(void*));
            Count = 0;
        }

        /// <summary>
        /// Checks if the list contains an item
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <returns>Of the list contains the item</returns>
        public bool Contains(long item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Item[i] == item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the entire list to a one-dimensional array
        /// </summary>
        /// <param name="array">The target array</param>
        public void CopyTo(long[] array)
        {
            CopyTo(0, array, 0, Count);
        }

        /// <summary>
        /// Copies the entire list to a one-dimensional array starting at a specified index of the target array
        /// </summary>
        /// <param name="array">The target array</param>
        /// <param name="arrayIndex">The target array index</param>
        public void CopyTo(long[] array, int arrayIndex)
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
        public unsafe void CopyTo(int index, long[] array, int arrayIndex, int count)
        {
            int destination = (int)Util.ObjectToVoidPtr(array) + (sizeof(void*) * arrayIndex);
            int source = (int)Util.ObjectToVoidPtr(Item) + (sizeof(void*) * index);
            Memory.Memcpy((void*)destination, (void*)source, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence
        /// </summary>
        /// <param name="item">The item to look for</param>
        public int IndexOf(long item)
        {
            return IndexOf(item, 0, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence with a starting index
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index</param>
        public int IndexOf(long item, int index)
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
        public int IndexOf(long item, int index, int count)
        {
            for (int i = index; i < Count && i < count + index; i++)
            {
                if (Item[i] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the last occurrence
        /// </summary>
        /// <param name="item">The item to look for</param>
        public int LastIndexOf(long item)
        {
            return LastIndexOf(item, Count - 1, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the index of the last occurrence with a starting index
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="index">The starting index</param>
        public int LastIndexOf(long item, int index)
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
        public int LastIndexOf(long item, int index, int count)
        {
            for (int i = index; i >= 0 && i - index < count; i--)
            {
                if (Item[i] == item)
                    return i;
            }

            return -1;
        }
    }
}
