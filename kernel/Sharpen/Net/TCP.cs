using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Lib;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    unsafe class TCP
    {
        private const byte PROTOCOL_TCP = 6;

        private const byte FLAG_FIN = (1 << 0);
        private const byte FLAG_SYN = (1 << 1);
        private const byte FLAG_RST = (1 << 2);
        private const byte FLAG_PSH = (1 << 3);
        private const byte FLAG_ACK = (1 << 4);
        private const byte FLAG_URG = (1 << 5);


        private static ushort m_portOffset = 49100;

        private static TCPConnection[] m_connections;

        /// <summary>
        /// Initializes TCP
        /// </summary>
        public static unsafe void Init()
        {
            m_connections = new TCPConnection[65536];

            IPV4.RegisterHandler(PROTOCOL_TCP, handler);
        }

        /// <summary>
        /// Add header to packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="srcPort"></param>
        /// <param name="dstPort"></param>
        /// <param name="flags"></param>
        /// <param name="seqNum"></param>
        /// <param name="ackNum"></param>
        /// <param name="winSize"></param>
        /// <returns></returns>
        private static unsafe TCPHeader* addHeader(NetPacketDesc* packet, ushort srcPort, ushort dstPort, byte flags, uint seqNum, uint ackNum, ushort winSize)
        {
            packet->start -= (short)sizeof(TCPHeader);

            // Generate stub header
            TCPHeader* header = (TCPHeader*)(packet->buffer + packet->start);
            header->SourcePort = Utilities.Byte.ReverseBytes(srcPort);
            header->DestPort = Utilities.Byte.ReverseBytes(dstPort);
            header->Flags = flags;
            header->Urgent = 0;
            header->WindowSize = Utilities.Byte.ReverseBytes(winSize);
            header->Acknowledge = ackNum;
            header->Sequence = seqNum;
            header->Checksum = 0;

            return header;
        }

        /// <summary>
        /// Finish header and create checksum
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="header"></param>
        /// <param name="sourceIp"></param>
        /// <param name="packetLength"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private static unsafe bool FinishHeader(NetPacketDesc* packet, TCPHeader *header, byte[] sourceIp, int packetLength, int dataLength)
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
            checksumHeader->Length = Utilities.Byte.ReverseBytes((ushort)((ushort)packetLength + dataLength));

            byte* ptr = (byte*)(packet->buffer + packet->start - sizeof(TCPChecksum));

            header->Checksum = NetworkTools.Checksum(ptr, size + dataLength);

            return true;
        }

        /// <summary>
        /// Free connection
        /// </summary>
        /// <param name="con"></param>
        public static void Free(TCPConnection con)
        {
            Close(con);

            if (con.Clients != null)
                Heap.Free(con.Clients);

            Heap.Free(con.IP);
            Heap.Free(con);
        }

        /// <summary>
        /// Send packet to TCP
        /// </summary>
        /// <param name="destIP"></param>
        /// <param name="seqNum"></param>
        /// <param name="acknumb"></param>
        /// <param name="srcPort"></param>
        /// <param name="destPort"></param>
        /// <param name="flags"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static unsafe bool SendPacket(byte[] destIP, uint seqNum, uint acknumb, ushort srcPort, ushort destPort, byte flags, byte *data, int count)
        {
            NetPacketDesc* packet = NetPacket.Alloc();
            

            if(count > 0)
            {
                Memory.Memcpy(packet->buffer + packet->start, data, count);

                packet->end += (short)count;
            }

            TCPHeader* outHeader = addHeader(packet, srcPort, destPort, flags, seqNum, acknumb, 8192);

            FinishHeader(packet, outHeader, destIP, sizeof(TCPHeader), count);

            IPV4.Send(packet, destIP, PROTOCOL_TCP);

            NetPacket.Free(packet);

            return true;
        }

        /// <summary>
        /// Get random port :)
        /// </summary>
        /// <returns></returns>
        public static ushort RequestPort()
        {
            return m_portOffset++;
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
            
            ushort dest = Utilities.Byte.ReverseBytes(header->DestPort);
            
            if(m_connections[dest] != null)
            {
                handleConnection(m_connections[dest], sourceIp, buffer, size);
            }
        }

        /// <summary>
        /// Handle connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void handleConnection(TCPConnection connection, byte[] sourceIP, byte* buffer, uint size)
        {

            switch(connection.State)
            {
                case TCPConnectionState.WAITING_FOR_ACK:
                case TCPConnectionState.SYNC_ACK:
                        Acknowledge(connection, buffer, size);
                    break;

                case TCPConnectionState.ACKNOWLEDGE:
                        HandleReceive(connection, buffer, size);
                    break;

                case TCPConnectionState.LISTEN:
                        HandleListen(connection, sourceIP, buffer, size);
                    break;
            }
        }

        /// <summary>
        /// Acknowledge
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void Acknowledge(TCPConnection connection, byte* buffer, uint size)
        {

            TCPHeader* header = (TCPHeader*)buffer;
            

            /**
             * Do we need to ACK or SYN|ACK?
             */
            if (!connection.InComing && (header->Flags & (FLAG_SYN | FLAG_ACK)) > 0)
            {
                connection.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection.SequenceNumber) + 1);

                connection.State = TCPConnectionState.ACKNOWLEDGE;

                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(connection.IP, connection.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), connection.InPort, connection.DestPort, FLAG_ACK, null, 0);
            }
            else if (connection.InComing && (header->Flags & FLAG_SYN) > 0)
            {
                connection.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection.SequenceNumber) + 1);

                connection.State = TCPConnectionState.SYNC_ACK;

                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);
                
                SendPacket(connection.IP, connection.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), connection.InPort, connection.DestPort, FLAG_SYN | FLAG_ACK, null, 0);
            }
            else if (connection.State == TCPConnectionState.SYNC_ACK && (header->Flags & FLAG_ACK) > 0)
            {
                connection.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection.SequenceNumber) + 1);

                connection.State = TCPConnectionState.ACKNOWLEDGE;

                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);
            }
        }

        /// <summary>
        /// Handle packet receiption
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void HandleReceive(TCPConnection connection, byte* buffer, uint size)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            if((header->Flags & FLAG_PSH) > 0)
            {
                int sizePacket = (int)size - sizeof(TCPHeader);
                
                /**
                 * Push packet in Queue
                 */
                TCPPacketDescriptor* buf = (TCPPacketDescriptor *)Heap.Alloc(sizeof(TCPPacketDescriptor));
                buf->Size = sizePacket;
                buf->Type = TCPPacketDescriptorTypes.RECEIVE;
                buf->Data = (byte*)Heap.Alloc(sizePacket);
                buf->xid = connection.XID;
                Memory.Memcpy(buf->Data, buffer + sizeof(TCPHeader), sizePacket);
                
                Queue queue = connection.ReceiveQueue;

                /**
                 * Is this a connection or a member?
                 */
                if (connection.Type != TCPConnectionType.CONNECTION)
                    queue = connection.BaseConnection.ReceiveQueue;

                queue.Push(buf);


                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + (uint)sizePacket);

                SendPacket(connection.IP, connection.SequenceNumber, connection.AcknowledgeNumber, connection.InPort, connection.DestPort, FLAG_ACK, null, 0);
            }
            else if((header->Flags & FLAG_RST) > 0)
            {
                connection.State = TCPConnectionState.CLOSED;

                if(connection.Type == TCPConnectionType.CONNECTION)
                    m_connections[connection.InPort] = null;
                else
                {
                    /**
                     * We need to remove the key here!
                     */

                    /**
                     * put RESET in QUEUE
                     */
                    TCPPacketDescriptor* buf = (TCPPacketDescriptor*)Heap.Alloc(sizeof(TCPPacketDescriptor));
                    buf->Size = 0;
                    buf->Type = TCPPacketDescriptorTypes.RESET;
                    buf->Data = null;
                    buf->xid = connection.XID;

                    connection.BaseConnection.ReceiveQueue.Push(buf);
                }
            }

        }
        
        /// <summary>
        /// Read packet from connection
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public unsafe static TCPPacketDescriptor *Read(TCPConnection con)
        {
            Queue queue = con.ReceiveQueue;
            while (queue.IsEmpty())
                CPU.HLT();

            return (TCPPacketDescriptor *)con.ReceiveQueue.Pop();
        }

        /// <summary>
        /// Send packet on connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void Send(TCPConnection connection, byte *buffer, uint size)
        {
            connection.SequenceNumber = connection.NextSequenceNumber;
            connection.NextSequenceNumber = connection.SequenceNumber + size;


            SendPacket(connection.IP, connection.SequenceNumber, connection.AcknowledgeNumber, connection.InPort, connection.DestPort, FLAG_PSH | FLAG_ACK, buffer, (int)size);
        }

        /// <summary>
        /// Close (reset) connection
        /// </summary>
        /// <param name="connection"></param>
        public static unsafe void Close(TCPConnection connection)
        {
            connection.SequenceNumber = connection.NextSequenceNumber;
            connection.NextSequenceNumber = connection.SequenceNumber + 1;

            SendPacket(connection.IP, connection.SequenceNumber, connection.AcknowledgeNumber, connection.InPort, connection.DestPort, FLAG_RST | FLAG_ACK, null, 0);
        }



        #region Bound handling
        private static unsafe void HandleListen(TCPConnection connection, byte[] sourceIP, byte* buffer, uint size)
        {
            TCPHeader* header = (TCPHeader*)buffer;
            ushort srcPort = header->SourcePort;

            long id = GenerateID(sourceIP, srcPort);

            TCPConnection clientConnection = (TCPConnection)connection.Clients.GetByKey(id);

            /**
             * Does client have a connection?
             */
            if (clientConnection == null)
            {
                /**
                 * Add connection to clients list
                 */
                TCPConnection con = new TCPConnection();

                con.InComing = true;
                con.XID = id;
                con.InPort = connection.InPort;
                con.DestPort = Byte.ReverseBytes(header->SourcePort);
                con.SequenceNumber = (uint)Random.Rand();
                con.NextSequenceNumber = connection.SequenceNumber + 1;
                con.State = TCPConnectionState.WAITING_FOR_ACK;
                con.IP = new byte[4];
                for (int i = 0; i < 4; i++)
                    con.IP[i] = sourceIP[i];
                con.AcknowledgeNumber = Byte.ReverseBytes(header->Acknowledge);
                con.Type = TCPConnectionType.CHILD_CONNECTION;
                con.Clients = connection.Clients;
                con.BaseConnection = connection;

                connection.Clients.Add(id, con);

                clientConnection = con;

                /**
                 * Put ACCEPT in queue
                 */
                TCPPacketDescriptor* buf = (TCPPacketDescriptor*)Heap.Alloc(sizeof(TCPPacketDescriptor));
                buf->Size = 4;
                buf->Type = TCPPacketDescriptorTypes.ACCEPT;
                buf->Data = (byte*)Heap.Alloc(4);
                buf->xid = id;
                
                for (int i = 0; i < 4; i++)
                    buf->Data[i] = con.IP[i];

                connection.ReceiveQueue.Push(buf);
            }

            handleConnection(clientConnection, sourceIP, buffer, size);
        }

        /// <summary>
        /// Generate ID from incoming IP and sourceport
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="srcPort"></param>
        /// <returns></returns>
        public static unsafe long GenerateID(byte[] ip, ushort srcPort)
        {
            return ((long)ip[0] << 8 | (long)ip[1] << 16 | (long)ip[2] << 24 | (long)ip[3] << 32 | (long)srcPort << 40);
        }


        /// <summary>
        /// Bind to IP
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static unsafe TCPConnection Bind(ushort port)
        {
            if (m_connections[port] != null)
                return null;

            TCPConnection ptr = new TCPConnection();
            ptr.InPort = port;
            ptr.State = TCPConnectionState.LISTEN;
            ptr.Clients = new Collections.Dictionary();
            ptr.Type = TCPConnectionType.CONNECTION;
            ptr.ReceiveQueue = new Collections.Queue();

            m_connections[port] = ptr;

            return ptr;
        }
        #endregion

        /// <summary>
        /// Connection to IP
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <returns></returns>
        public static unsafe TCPConnection Connect(byte[] ip, ushort port)
        {
            ushort inPort = RequestPort();
            int startSeq = Random.Rand();


            if (!ARP.IpExists(ip))
            {

                byte[] mac = new byte[6];
                for (int i = 0; i < 6; i++)
                    mac[i] = 0xFF;

                ARP.ArpSend(ARP.OP_REQUEST, mac, ip);
                
                /**
                 * Todo: free mac
                 */
            }

            while (!ARP.IpExists(ip))
                CPU.HLT();

            TCPConnection ptr = new TCPConnection();
            ptr.DestPort = port;
            ptr.InPort = inPort;
            ptr.State = TCPConnectionState.WAITING_FOR_ACK;
            ptr.InComing = false;
            ptr.XID = Random.Rand();
            ptr.SequenceNumber = (uint)startSeq;
            ptr.NextSequenceNumber = (uint)Byte.ReverseBytes(Byte.ReverseBytes(ptr.SequenceNumber) + 1);
            ptr.Type = TCPConnectionType.CONNECTION;
            ptr.ReceiveQueue = new Collections.Queue();

            for (int i = 0; i < 4; i++)
                ptr.IP[i] = ip[i];

            m_connections[inPort] = ptr;
            
            SendPacket(ip, (uint)startSeq, 0, inPort, port, FLAG_SYN, null, 0);

            return ptr;
        }
    }
}
