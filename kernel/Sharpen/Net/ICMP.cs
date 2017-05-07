using Sharpen.Mem;

namespace Sharpen.Net
{
    class ICMP
    {
        private const byte TYPE_ECHO_REQUEST = 8;
        private const byte TYPE_ECHO_REPLY = 0;

        public unsafe struct ICMPHeader
        {
            public byte Type;
            public byte Code;
            public ushort CheckSum;
            public ushort ID;
            public ushort SeqNum;
        }

        /// <summary>
        /// Initialize ICMP handler
        /// </summary>
        public static unsafe void Init()
        {
            IPV4.RegisterHandler(0x01, handler);
        }

        /// <summary>
        /// Echoes
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="id"></param>
        /// <param name="seq"></param>
        /// <param name="data"></param>
        /// <param name="length"></param>
        private static unsafe void EchoReply(byte[] ip, ushort id, ushort seq, byte* data, int length)
        {
            NetPacketDesc* packet = NetPacket.Alloc();

            ICMPHeader* hdr = (ICMPHeader*)(packet->buffer + packet->start);
            hdr->Type = TYPE_ECHO_REPLY;
            hdr->ID = id;
            hdr->SeqNum = seq;
            hdr->Code = 0;
            hdr->CheckSum = 0;
            
            packet->end += (short)sizeof(ICMPHeader);
            Memory.Memcpy(packet->buffer + packet->end, data, length);
            packet->end += (short)length;

            hdr->CheckSum = NetworkTools.Checksum(packet->buffer + packet->start, sizeof(ICMPHeader) + length);

            IPV4.Send(packet, ip, 0x01);

            NetPacket.Free(packet);
        }

        /// <summary>
        /// Handles an ICMP request
        /// </summary>
        /// <param name="sourceIp"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void handler(byte[] sourceIp, byte* buffer, uint size)
        {
            ICMPHeader* hdr = (ICMPHeader*)buffer;

            if (hdr->Type == TYPE_ECHO_REQUEST)
            {
                int length = (int)size - sizeof(ICMPHeader);
                byte* data = buffer + sizeof(ICMPHeader);

                EchoReply(sourceIp, hdr->ID, hdr->SeqNum, data, length);
            }
        }
    }
}