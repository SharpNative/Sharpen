namespace Sharpen.FileSystem
{
    public class DevList
    {
        // Default capacity of 4
        private int m_currentCap = 4;

        // Array of items
        public Device[] Item { get; private set; }

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
                Device[] newArray = new Device[value];
                Memory.Memcpy(Util.ObjectToVoidPtr(newArray), Util.ObjectToVoidPtr(Item), m_currentCap * sizeof(void*));
                Item = newArray;
                m_currentCap = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DevList()
        {
            Item = new Device[m_currentCap];
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
        public void Add(Device o)
        {
            EnsureCapacity(Count + 1);
            Item[Count++] = o;
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
            int destination = (int)Util.ObjectToVoidPtr(array) + (sizeof(void*) * arrayIndex);
            int source = (int)Util.ObjectToVoidPtr(Item) + (sizeof(void*) * index);
            Memory.Memcpy((void*)destination, (void*)source, count);
        }
    }
}
