﻿using Sharpen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    enum TCPConnectionState
    {
        WAITING_FOR_ACK = 0,
        ACKNOWLEDGE = 1,
        SYNC_ACK = 3,
        LISTEN = 0xFE,
        CLOSED = 0xFF
    }

    enum TCPConnectionType
    {
        CONNECTION = 0,
        CHILD_CONNECTION = 1
    }

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

    class TCPConnection
    {
        public byte[] IP;
        public ushort InPort;
        public ushort DestPort;

        public TCPConnectionState State;
        public bool InComing;

        public uint SequenceNumber;
        public uint NextSequenceNumber;

        public Dictionary Clients;

        public TCPConnectionType Type;

        public TCPConnection BaseConnection;

        public Queue ReceiveQueue; 

        public uint AcknowledgeNumber;

        public long XID;
    }

    unsafe struct TCPPacketDescriptor
    {
        public TCPPacketDescriptorTypes Type;
        public int Size;
        public long xid;
        public byte* Data;
    }

    enum TCPPacketDescriptorTypes
    {
        ACCEPT = 0,
        RECEIVE = 1,
        RESET = 2
    }
}