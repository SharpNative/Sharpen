using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    public unsafe struct NetPacketDesc
    {
        public short start;
        public short end;
        public fixed byte buffer[4096];
    }

    class NetPacket
    {
        public static unsafe NetPacketDesc *Alloc()
        {
            NetPacketDesc *desc = (NetPacketDesc*)Heap.Alloc(sizeof(NetPacketDesc));
            Memory.Memset(desc->buffer, 0x00, 4096);
            desc->start = 256;
            desc->end = 256;

            return desc;
        }

        public static unsafe void Free(NetPacketDesc *packet)
        {
            Heap.Free(packet);
        }
    }
}
