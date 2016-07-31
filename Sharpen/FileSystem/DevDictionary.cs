using Sharpen.Collections;
using Sharpen.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    class DevDictionary
    {
        private LongIndex m_index = new LongIndex();
        private DevList m_values = new DevList();
        
        public void Clear()
        {
            m_values.Clear();
        }

        public int Count()
        {
            return m_values.Count;
        }

        public Device GetAt(int index)
        {
            return (index != -1) ? m_values.Item[index] : null;
        }

        /// <summary>
        /// Add value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public unsafe void Add(long key, Device val)
        {
            int index = m_index.IndexOf(key);
            
            if(index == -1)
            {
                m_index.Add(key);
                m_values.Add(val);
            }
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Device GetByKey(long key)
        {
            int index = m_index.IndexOf(key);
            
            return (index != -1) ? m_values.Item[index]: null;
        }
    }
}
