using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
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

    unsafe struct TCPConnection
    {
        public fixed byte IP[4];
        public ushort InPort;
        public ushort DestPort;

        public uint State;
        public bool InComing;

        public uint SequenceNumber;
        public uint NextSequenceNumber;

        public uint AcknowledgeNumber;

        public int XID;
    }
}
