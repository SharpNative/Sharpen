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

        #region Network handling

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
            TCPHeader* header = (TCPHeader *)buffer;
            
            switch(connection.State)
            {

                case TCPConnectionState.CLOSED:
                    ClosedHandler(connection);
                    break;

                case TCPConnectionState.LISTEN:
                    HandleListen(connection, sourceIP, buffer, size);
                    break;

                case TCPConnectionState.SYN_SENT:
                    SynSentHandler(connection, buffer);
                    break;

                case TCPConnectionState.SYN_RECEIVED:
                    SynReceivedHandler(connection, buffer);
                    break;

                case TCPConnectionState.ESTABLISHED:
                    EstablishedHandler(connection, sourceIP, buffer, size);
                    break;

                case TCPConnectionState.FIN_WAIT1:
                    finWaitOneHandler(connection, buffer);
                    break;

                case TCPConnectionState.FIN_WAIT2:
                    finWaitTwoHandler(connection, buffer);
                    break;

                case TCPConnectionState.CLOSING:
                    closingHandler(connection, buffer);
                    break;

                case TCPConnectionState.TIME_WAIT:
                    Reset(connection, buffer);
                    break;

                case TCPConnectionState.LAST_ACK:
                    LastAckHandler(connection, sourceIP, buffer, size);
                    break;
            }
        }

        #endregion

        #region Handshake
        /// <summary>
        /// Closed state handler
        /// </summary>
        /// <param name="con"></param>
        private static void ClosedHandler(TCPConnection con)
        {
            SendPacket(con.IP, con.SequenceNumber, 0, con.InPort, con.DestPort, FLAG_SYN, null, 0);

            con.State = TCPConnectionState.SYN_SENT;
        }

        /// <summary>
        /// Syn sent handler
        /// </summary>
        /// <param name="con"></param>
        /// <param name="buffer"></param>
        private unsafe static void SynSentHandler(TCPConnection con, byte *buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            if ((header->Flags & (FLAG_SYN | FLAG_ACK)) == (FLAG_SYN | FLAG_ACK))
            {
                con.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);

                con.State = TCPConnectionState.ESTABLISHED;

                con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(con.IP, con.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), con.InPort, con.DestPort, FLAG_ACK, null, 0);
            }
            else
            {
                // Failed?
            }
        }

        /// <summary>
        /// Syn received
        /// </summary>
        /// <param name="con"></param>
        /// <param name="buffer"></param>
        private unsafe static void SynReceivedHandler(TCPConnection con, byte* buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;
            
            /**
             * ACK received?
             */
            if ((header->Flags & (FLAG_ACK)) > 0)
            {
                con.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);

                con.State = TCPConnectionState.ESTABLISHED;
                

                /**
                 * Do we need to notify we have a new connection?
                 */
                if(con.Type == TCPConnectionType.CHILD_CONNECTION)
                {
                    /**
                     * Put ACCEPT in queue
                     */
                    TCPPacketDescriptor* buf = (TCPPacketDescriptor*)Heap.Alloc(sizeof(TCPPacketDescriptor));
                    buf->Size = 4;
                    buf->Type = TCPPacketDescriptorTypes.ACCEPT;
                    buf->Data = (byte*)Heap.Alloc(4);
                    buf->xid = con.XID;

                    for (int i = 0; i < 4; i++)
                        buf->Data[i] = con.IP[i];

                    con.BaseConnection.ReceiveQueue.Push(buf);
                }
            }
            else
            {
                // Failed?
            }
        }

        #endregion

        #region Closing

        /// <summary>
        /// Reset connection
        /// </summary>
        /// <param name="con"></param>
        private unsafe static void Reset(TCPConnection con, byte* buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

            SendPacket(con.IP, con.SequenceNumber, con.AcknowledgeNumber, con.InPort, con.DestPort, FLAG_RST | FLAG_ACK, null, 0);
        }

        /// <summary>
        /// FIN wait1 handler
        /// </summary>
        /// <param name="con"></param>
        /// <param name="buffer"></param>
        private unsafe static void finWaitOneHandler(TCPConnection con, byte* buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            /**
             * ACK received?
             */
            if ((header->Flags & (FLAG_ACK)) > 0)
            {

                con.State = TCPConnectionState.FIN_WAIT2;

                con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);
            }
            else if ((header->Flags & (FLAG_FIN)) > 0)
            {

                con.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);

                con.State = TCPConnectionState.CLOSING;

                con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(con.IP, con.SequenceNumber, con.AcknowledgeNumber, con.InPort, con.DestPort, FLAG_FIN, null, 0);
            }
            else
            {
                // Failed?
            }
        }

        /// <summary>
        /// Find wait2 handler
        /// </summary>
        /// <param name="con"></param>
        /// <param name="buffer"></param>
        private unsafe static void finWaitTwoHandler(TCPConnection con, byte* buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            if ((header->Flags & (FLAG_FIN)) > 0)
            {

                con.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);
                

                con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(con.IP, con.SequenceNumber, con.AcknowledgeNumber, con.InPort, con.DestPort, FLAG_ACK, null, 0);

                setConnectionForWait(con);

            }
            else
            {

            }
        }

        /// <summary>
        /// CLOSING handler
        /// </summary>
        /// <param name="con"></param>
        /// <param name="buffer"></param>
        private unsafe static void closingHandler(TCPConnection con, byte* buffer)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            if ((header->Flags & (FLAG_ACK)) > 0)
            {

                con.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);


                con.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);
                
                // AND We're DONE!
                setConnectionForWait(con);

            }
            else
            {
                // Error handling?
            }
        }


        #endregion

        #region Listening

        /// <summary>
        /// Listen state handler
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sourceIP"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void HandleListen(TCPConnection connection, byte[] sourceIP, byte* buffer, uint size)
        {
            TCPHeader* header = (TCPHeader*)buffer;


            ushort srcPort = header->SourcePort;

            long id = GenerateID(sourceIP, srcPort);

            TCPConnection clientConnection = (TCPConnection)connection.Clients.GetByKey(id);

            /**
             * Do we need to create a connection or just pass it though?
             */
            if (clientConnection == null)
            {
                /**
                 * We only handle SYN packets here!
                 */
                if ((header->Flags & FLAG_SYN) == 0)
                {
                    Close(connection);

                    return;
                }

                /**
                 * Add connection to clients list
                 */
                clientConnection = new TCPConnection();

                clientConnection.InComing = true;
                clientConnection.XID = id;
                clientConnection.InPort = connection.InPort;
                clientConnection.DestPort = Byte.ReverseBytes(header->SourcePort);
                clientConnection.SequenceNumber = (uint)Random.Rand();
                clientConnection.NextSequenceNumber = connection.SequenceNumber + 1;
                clientConnection.State = TCPConnectionState.SYN_RECEIVED;
                clientConnection.IP = new byte[4];
                for (int i = 0; i < 4; i++)
                    clientConnection.IP[i] = sourceIP[i];
                clientConnection.AcknowledgeNumber = Byte.ReverseBytes(header->Acknowledge);
                clientConnection.Type = TCPConnectionType.CHILD_CONNECTION;
                clientConnection.Clients = connection.Clients;
                clientConnection.BaseConnection = connection;

                connection.Clients.Add(id, clientConnection);

                /**
                 * Send SYN_ACK and transition to SYN_RECEIVED state
                 */
                clientConnection.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection.SequenceNumber) + 1);

                clientConnection.State = TCPConnectionState.SYN_RECEIVED;

                clientConnection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(clientConnection.IP, clientConnection.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), clientConnection.InPort, clientConnection.DestPort, FLAG_SYN | FLAG_ACK, null, 0);
            }
            else
            {
                handleConnection(clientConnection, sourceIP, buffer, size);
            }
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

        #endregion

        #region Established

        /// <summary>
        /// Established handler
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sourceIP"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void EstablishedHandler(TCPConnection connection, byte[] sourceIP, byte* buffer, uint size)
        {

            TCPHeader* header = (TCPHeader*)buffer;

            /**
             * 
             * Possible flags:
             *   - PUSH
             *   - FIN
             */
             
            if ((header->Flags & (FLAG_FIN | FLAG_ACK)) == (FLAG_FIN | FLAG_ACK))
            {
                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(connection.IP, connection.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), connection.InPort, connection.DestPort, FLAG_ACK, null, 0);
                
                setConnectionForWait(connection);

            }
            else if ((header->Flags & FLAG_FIN) > 0)
            {

                /**
                 * Todo: We need acknowledge our application here with status CLOSE_WAIT, for now we shift over to the LAST_ACK state
                 */
                connection.AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                connection.State = TCPConnectionState.LAST_ACK;
                SendPacket(connection.IP, connection.SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), connection.InPort, connection.DestPort, FLAG_ACK, null, 0);

                Sleep(300);

                SendPacket(connection.IP, connection.SequenceNumber, 0x00, connection.InPort, connection.DestPort, FLAG_FIN, null, 0);
            }
            if ((header->Flags & FLAG_PSH) > 0)
            {
                int sizePacket = (int)size - sizeof(TCPHeader);

                /**
                 * Push packet in Queue
                 */
                TCPPacketDescriptor* buf = (TCPPacketDescriptor*)Heap.Alloc(sizeof(TCPPacketDescriptor));
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
            else
            {

            }
        }



        /// <summary>
        /// Sleep for X ms
        /// </summary>
        /// <param name="cnt"></param>
        private static void Sleep(int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }


        /// <summary>
        /// Established handler
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sourceIP"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void LastAckHandler(TCPConnection connection, byte[] sourceIP, byte* buffer, uint size)
        {

            TCPHeader* header = (TCPHeader*)buffer;

            /**
             * 
             * Possible flags:
             *   - PUSH
             *   - FIN
             */

            if ((header->Flags & FLAG_ACK) > 0)
            {
                
                connection.State = TCPConnectionState.TIME_WAIT;
            }
            else
            {

            }
        }

        #endregion

        #region Misc

        private static void setConnectionForWait(TCPConnection connection)
        {
            connection.State = TCPConnectionState.TIME_WAIT;

            if (connection.Type == TCPConnectionType.CONNECTION)
            {
                TCPPacketDescriptor* buf = (TCPPacketDescriptor*)Heap.Alloc(sizeof(TCPPacketDescriptor));
                buf->Size = 0;
                buf->Type = TCPPacketDescriptorTypes.CLOSE;
                buf->Data = null;
                buf->xid = 0x000;

                connection.ReceiveQueue.Push(buf);
            }
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
                buf->Type = TCPPacketDescriptorTypes.CLOSE;
                buf->Data = null;
                buf->xid = connection.XID;

                connection.BaseConnection.ReceiveQueue.Push(buf);
                connection.BaseConnection.Clients.Remove(connection.XID);
            }
        }

        #endregion

        #region Connection controls

        /// <summary>
        /// Send packet on connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void Send(TCPConnection connection, byte* buffer, uint size)
        {
            SendPacket(connection.IP, connection.SequenceNumber, connection.AcknowledgeNumber, connection.InPort, connection.DestPort, FLAG_PSH | FLAG_ACK, buffer, (int)size);
            connection.SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection.SequenceNumber) + size);

        }



        /// <summary>
        /// Close (reset) connection
        /// </summary>
        /// <param name="connection"></param>
        public static unsafe void Close(TCPConnection connection)
        {
            connection.State = TCPConnectionState.FIN_WAIT1;

            SendPacket(connection.IP, connection.SequenceNumber, connection.AcknowledgeNumber, connection.InPort, connection.DestPort, FLAG_FIN | FLAG_ACK, null, 0);
        }


        #endregion

        #region Connection controls XID


        /// <summary>
        /// Send packet on XID connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void Send(TCPConnection baseConnection, long xid, byte* buffer, uint size)
        {
            TCPConnection connection = (TCPConnection)baseConnection.Clients.GetByKey(xid);
            if (connection == null)
                return;

            Send(connection, buffer, size);
        }

        /// <summary>
        /// Close connection on XID connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void Close(TCPConnection baseConnection, long xid)
        {
            TCPConnection connection = (TCPConnection)baseConnection.Clients.GetByKey(xid);
            if (connection == null)
                return;

            Close(connection);
        }

        #endregion
        
        #region helper methods

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

        #endregion
        
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

            TCPConnection con = new TCPConnection();
            con.DestPort = port;
            con.InPort = inPort;
            con.State = TCPConnectionState.CLOSED;
            con.InComing = false;
            con.XID = Random.Rand();
            con.SequenceNumber = (uint)startSeq;
            con.NextSequenceNumber = (uint)Byte.ReverseBytes(Byte.ReverseBytes(con.SequenceNumber) + 1);
            con.Type = TCPConnectionType.CONNECTION;
            con.ReceiveQueue = new Collections.Queue();

            for (int i = 0; i < 4; i++)
                con.IP[i] = ip[i];

            m_connections[inPort] = con;

            handleConnection(con, null, null, 0);



            return con;
        }
    }
}
