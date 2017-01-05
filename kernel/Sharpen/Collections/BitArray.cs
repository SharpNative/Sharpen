namespace Sharpen.Collections
{
    public class BitArray
    {
        private int[] m_bitmap;

        private int m_N;
        private int m_leastClear = 0;

        /// <summary>
        /// Initializes a bit array with N integers
        /// </summary>
        /// <param name="N">The amount of integers (or N*32 = amount of bits)</param>
        public unsafe BitArray(int N)
        {
            m_bitmap = new int[N];
            m_N = N;
        }

        /// <summary>
        /// Sets the bit k
        /// </summary>
        /// <param name="k">The bit number</param>
        public void SetBit(int k)
        {
            int bitmap = k >> 5;
            int index = k & (32 - 1);
            m_bitmap[bitmap] |= 1 << index;
        }

        /// <summary>
        /// Clears the bit k
        /// </summary>
        /// <param name="k">The bit number</param>
        public void ClearBit(int k)
        {
            int bitmap = k >> 5;
            int index = k & (32 - 1);
            m_bitmap[bitmap] &= ~(1 << index);

            if (bitmap < m_leastClear)
                m_leastClear = bitmap;
        }

        /// <summary>
        /// Tests the bit k
        /// </summary>
        /// <param name="k">The bit number</param>
        public bool IsBitSet(int k)
        {
            int bitmap = k >> 5;
            int index = k & (32 - 1);
            return ((m_bitmap[bitmap] & (1 << index)) > 0);
        }

        /// <summary>
        /// Finds free range and potentially set it
        /// </summary>
        /// <param name="size">The size of the range of free bits</param>
        /// <param name="set">If it should also be set</param>
        /// <returns></returns>
        public int FindFirstFreeRange(int size, bool set)
        {
            int start = FindFirstFree();
            
            // We start with one because from a relative offset, bit zero is not set
            int i = 1;
            while (i < size)
            {
                // The current bit is set
                if (IsBitSet(start + i))
                {
                    start++;
                    i = 0;
                }
                // The current bit is not set
                else
                {
                    i++;
                }
            }
            
            // Set it if needed
            if (set)
            {
                for (int j = 0; j < size; j++)
                {
                    SetBit(start + j);
                }
            }

            return start;
        }

        /// <summary>
        /// Finds the first free bit and potentially set it
        /// </summary>
        /// <param name="size">The size of the range of free bits</param>
        /// <param name="set">If it should also be set</param>
        /// <returns></returns>
        public int FindFirstFree(bool set)
        {
            for (int i = m_leastClear; i < m_N; i++)
            {
                int bitmap = m_bitmap[i];

                // All bits set?
                if (bitmap == -1)
                    continue;

                // Check the bits
                for (int j = 0; j < 32; j++)
                {
                    if ((bitmap & (1 << j)) == 0)
                    {
                        if (set)
                            m_bitmap[i] |= (1 << j);

                        return (i << 5) + j;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds the first free bit
        /// </summary>
        /// <returns></returns>
        public int FindFirstFree()
        {
            return FindFirstFree(false);
        }
    }
}
