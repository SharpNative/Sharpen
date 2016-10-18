using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class TCP
    {
        private const byte PROTOCOL_TCP = 6;

        private const byte FLAG_FIN = (1 << 0);
        private const byte FLAG_SYN = (1 << 1);
        private const byte FLAG_RST = (1 << 2);
        private const byte FLAG_PSH = (1 << 3);
        private const byte FLAG_ACK = (1 << 4);
        private const byte FLAG_URG = (1 << 5);

        struct TCPHeader
        {
            public ushort SourcePort; // 2 
            public ushort DestPort; // 4
            public uint Sequence;  // 8
            public uint Acknowledge; // 12
            public byte Length; // 13
            public byte Flags; // 14
            public ushort WindowSize; // 16
            public ushort Checksum; // 18
            public ushort Urgent; // 20 / 4 = 5
        }

        unsafe struct TCPChecksum
        {
            public fixed byte SrcIP[4];
            public fixed byte DstIP[4];
            public byte Reserved;
            public byte Protocol;
            public ushort Length;
        }

        /// <summary>
        /// Initializes UDP
        /// </summary>
        public static unsafe void Init()
        {
            IPV4.RegisterHandler(PROTOCOL_TCP, handler);
            

        }

        private static unsafe TCPHeader* addHeader(NetPacketDesc* packet, ushort srcPort, ushort dstPort, byte flags, ushort seqNum, uint ackNum, ushort winSize)
        {
            packet->start -= (short)sizeof(TCPHeader);

            // Generate stub header
            TCPHeader* header = (TCPHeader*)(packet->buffer + packet->start);
            header->SourcePort = ByteUtil.ReverseBytes(srcPort);
            header->DestPort = ByteUtil.ReverseBytes(dstPort);
            header->Flags = flags;
            header->Urgent = 0;
            header->WindowSize = ByteUtil.ReverseBytes(winSize);
            header->Acknowledge = ByteUtil.ReverseBytes(ackNum);
            header->Sequence = ByteUtil.ReverseBytes(seqNum);
            header->Checksum = 0;

            return header;
        }

        private static unsafe bool FinishHeader(NetPacketDesc* packet, TCPHeader *header, byte[] sourceIp, int packetLength)
        {
            // Welp!
            if (packetLength % 4 != 0)
                return false;

            header->Length = (byte)((packetLength / 4) << 4);

            ushort size = (ushort)packetLength;
            size += (ushort)sizeof(TCPChecksum);

            // Let's introduce some junk (i love that :))
            TCPChecksum* checksumHeader = (TCPChecksum*)(packet->buffer + packet->start - sizeof(TCPChecksum));

            for (int i = 0; i < 4; i++)
                checksumHeader->SrcIP[i] = Network.Settings->IP[i];

            for (int i = 0; i < 4; i++)
                checksumHeader->DstIP[i] = sourceIp[i];

            checksumHeader->Protocol = PROTOCOL_TCP;
            checksumHeader->Reserved = 0;
            checksumHeader->Length = ByteUtil.ReverseBytes((ushort)packetLength);

            byte* ptr = (byte*)(packet->buffer + packet->start - sizeof(TCPChecksum));

            header->Checksum = NetworkTools.Checksum(ptr, size);

            return true;
        }

        private static unsafe bool SendPacket(byte[] sourceIp, uint acknumb, ushort srcPort, ushort destPort, byte flags, byte *data, int count)
        {
            NetPacketDesc* packet = NetPacket.Alloc();
            

            if(count > 0)
            {
                Memory.Memcpy(packet->buffer + packet->start, data, count);

                packet->end += (short)count;
            }

            TCPHeader* outHeader = addHeader(packet, srcPort, destPort, FLAG_SYN | FLAG_ACK, 0, acknumb, 8192);

            FinishHeader(packet, outHeader, sourceIp, sizeof(TCPHeader) + count);

            IPV4.Send(packet, sourceIp, PROTOCOL_TCP);

            NetPacket.Free(packet);

            return true;
        }

        /// <summary>
        /// UDP packet handler
        /// </summary>
        /// <param name="xid">Identification ID</param>
        /// <param name="buffer">Buffer pointer</param>
        /// <param name="size">Packet size</param>
        private static unsafe void handler(byte[] sourceIp, byte* buffer, uint size)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            ushort source = ByteUtil.ReverseBytes(header->SourcePort);
            ushort dest = ByteUtil.ReverseBytes(header->DestPort);
            
            int length = (header->Length >> 4) * 8;

            byte* data = (byte*)Heap.Alloc(length);
            Memory.Memcpy(data, buffer + 20, length - 20);


            SendPacket(sourceIp, ByteUtil.ReverseBytes(header->Sequence) + 1, dest, source, FLAG_ACK | FLAG_SYN, data, length);
        }
    }
}
