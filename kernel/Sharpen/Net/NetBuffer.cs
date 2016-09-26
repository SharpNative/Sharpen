using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    public unsafe struct NetBufferDescriptor
    {
        public short start;
        public short end;
        public fixed byte buffer[2048];
    }

    class NetBuffer
    {
        public static unsafe NetBufferDescriptor *Alloc()
        {
            NetBufferDescriptor *desc = (NetBufferDescriptor*)Heap.Alloc(sizeof(NetBufferDescriptor));
            desc->start = 256;
            desc->end = 256;

            return desc;
        }

        public static unsafe void Free(NetBufferDescriptor *packet)
        {
            Heap.Free(packet);
        }
    }
}
