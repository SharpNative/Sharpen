using Sharpen.Task;

namespace Sharpen.Collections
{
    public class Fifo
    {
        private byte[] m_buffer;
        private bool m_wait = true;
        private int m_head = 0;
        private int m_tail = 0;
        private int m_size;

        public uint AvailableBytes { get; private set; } = 0;

        /// <summary>
        /// Creates a fifo buffer
        /// </summary>
        /// <param name="size">The size of the buffer</param>
        public Fifo(int size, bool wait)
        {
            m_buffer = new byte[size];
            m_size = size;
            m_wait = wait;
        }

        /// <summary>
        /// Read until the buffer is full with the requested amount of data
        /// </summary>
        /// <param name="buffer">The buffer where to put the data into</param>
        /// <param name="size">The amount of bytes to read</param>
        /// <returns>The amount of read bytes</returns>
        public unsafe uint ReadWait(byte[] buffer, uint size)
        {
            uint left = size;
            uint offset = 0;

            while (left > 0)
            {
                uint sz = Read(buffer, left, offset);

                left -= sz;
                offset += sz;
            }

            return size;
        }

        /// <summary>
        /// Read from fifo
        /// </summary>
        /// <param name="buffer">The buffer where to put the data into</param>
        /// <param name="size">The amount of bytes read</param>
        /// <returns>The amount of read bytes</returns>
        public unsafe uint Read(byte[] buffer, uint size)
        {
            return Read(buffer, size, 0);
        }

        /// <summary>
        /// Read from fifo
        /// </summary>
        /// <param name="buffer">The buffer where to put the data into</param>
        /// <param name="size">The amount of bytes read</param>
        /// <param name="offset">The offset in the buffer</param>
        /// <returns>The amount of read bytes</returns>
        public unsafe uint Read(byte[] buffer, uint size, uint offset)
        {
            uint j = offset;

            if (m_wait)
            {
                while (m_head == m_tail)
                {
                    Tasking.ManualSchedule();
                }
            }

            for (uint i = 0; i < size; i++)
            {
                // Is there data?
                if (m_tail != m_head)
                {
                    AvailableBytes--;

                    buffer[j] = m_buffer[m_tail];
                    j++;
                    m_tail++;

                    // Time to flip the tail?
                    if (m_tail >= m_size)
                        m_tail = 0;
                }
                else
                {
                    // We may return if we shouldn't wait
                    return i;
                }
            }
            
            return size;
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="size">Size of buffer</param>
        /// <returns>The amount of bytes written</returns>
        public unsafe uint Write(byte* buffer, uint size)
        {
            byte* current = buffer;

            for (uint i = 0; i < size; i++)
            {
                if (!WriteByte(*current++))
                    return i;
            }
            
            return size;
        }

        /// <summary>
        /// Write byte to fifo struct
        /// </summary>
        /// <param name="byt">Byte</param>
        /// <returns>If the write was successful</returns>
        public unsafe bool WriteByte(byte byt)
        {
            // Is there any room?
            if ((m_head + 1 == m_tail) || ((m_head + 1 == m_size) && m_tail == 0))
            {
                return false;
            }
            else
            {
                m_buffer[m_head] = byt;
                m_head++;

                // Time to flip the tail?
                if (m_head >= m_size)
                    m_head = 0;
            }

            AvailableBytes++;

            return true;
        }
    }
}
