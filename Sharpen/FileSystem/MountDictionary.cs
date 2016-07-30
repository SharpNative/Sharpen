using Sharpen.Collections;
using Sharpen.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    class MountDictionary
    {
        private LongIndex m_index = new LongIndex();
        private MountList m_values = new MountList();
        
        public void Clear()
        {
            m_values.Clear();
        }

        /// <summary>
        /// Add value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public unsafe void Add(long key, MountPoint val)
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
        public MountPoint GetByKey(long key)
        {
            int index = m_index.IndexOf(key);
            
            return (index != -1) ? m_values.Item[index]: null;
        }
    }
}
