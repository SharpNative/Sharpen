using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Collections
{
    public class BitArray
    {
        private int[] m_bitmap;
        private int m_N;

        /// <summary>
        /// Initializes a bit array with N integers
        /// </summary>
        /// <param name="N">The amount of integers (or N*32 = amount of bits)</param>
        public unsafe BitArray(int N)
        {
            m_bitmap = new int[N];
            fixed(int* ptr = m_bitmap)
            {
                Memory.Memset(ptr, 0, N * sizeof(int));
            }
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
        }

        /// <summary>
        /// Tests the bit k
        /// </summary>
        /// <param name="k">The bit number</param>
        public bool TestBit(int k)
        {
            int bitmap = k >> 5;
            int index = k & (32 - 1);
            return ((m_bitmap[bitmap] & (1 << index)) > 0);
        }

        /// <summary>
        /// Finds the first free bit
        /// </summary>
        /// <returns></returns>
        public int FindFirstFree()
        {
            for (int i = 0; i < m_N; i++)
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
                        return i * 32 + j;
                    }
                }
            }

            return -1;
        }
    }
}
