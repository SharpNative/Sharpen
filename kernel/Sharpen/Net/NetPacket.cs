using Sharpen.Mem;

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
        /// <summary>
        /// Allocates a new network packet descriptor
        /// </summary>
        /// <returns>New network packet descriptor</returns>
        public static unsafe NetPacketDesc* Alloc()
        {
            NetPacketDesc* desc = (NetPacketDesc*)Heap.Alloc(sizeof(NetPacketDesc));
            Memory.Memset(desc->buffer, 0x00, 4096);
            desc->start = 256;
            desc->end = 256;

            return desc;
        }

        /// <summary>
        /// Frees a network packet descriptor
        /// </summary>
        /// <param name="packet">The network packet descriptor</param>
        public static unsafe void Free(NetPacketDesc* packet)
        {
            Heap.Free(packet);
        }
    }
}
