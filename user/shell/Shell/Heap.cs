using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    unsafe class Heap
    {
        public static extern void* Alloc(int size);
        public static extern void Free(void* ptr);
    }
}
