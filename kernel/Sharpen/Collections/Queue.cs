using Sharpen.Arch;
using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Collections
{
    public unsafe class Queue
    {
        private unsafe struct stackNode
        {
            public stackNode* Next;
            public void* Value;
        }

        private stackNode* m_next;
        private stackNode* m_last;
        private uint m_length;
        private bool Working = false;

        public uint Length
        {
            get { return m_length; }
        }

        public Queue()
        {
            m_last = m_next = null;
            m_length = 0;
        }

        public bool IsEmpty()
        {
            return m_length == 0;
        }

        public void Push(void *value)
        {
            Console.Write("VAL: ");
            Console.WriteHex((long)value);
            Console.WriteLine("");
            
            if (m_length == 0)
                m_last = null;

            stackNode* node = (stackNode*)Heap.Alloc(sizeof(stackNode));
            node->Value = value;
            node->Next = m_last;

            m_last = node;

            m_length++;
            if (m_next == null)
                m_next = m_last;

            Console.Write("");
            Console.WriteHex((long)m_next);
            Console.WriteLine("");

        }

        public unsafe void *Pop()
        {
            CPU.CLI();
            
            if (m_next == null || m_length == 0)
            {
                Working = false;
                CPU.STI();
                return null;
            }
            
            stackNode* node = m_next;
            m_next = node->Next;
            void* ret = node->Value;
            
            Heap.Free(node);

            Console.Write("NEXT: ");
            Console.WriteHex((long)node);
            Console.Write(":");
            Console.WriteHex((int)m_next);
            Console.WriteLine("");

            m_length--;

            CPU.STI();

            return ret;
        }
    }
}
