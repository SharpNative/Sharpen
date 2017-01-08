using Sharpen.Arch;
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

        private static TCPConnection*[] m_connections;

        /// <summary>
        /// Initializes TCP
        /// </summary>
        public static unsafe void Init()
        {
            m_connections = new TCPConnection*[65536];
            Memory.Memset(Util.ObjectToVoidPtr(m_connections), 0x00, sizeof(TCPConnection*) * 65536);

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
                handleConnection(m_connections[dest], buffer, size);
            }
        }

        /// <summary>
        /// Handle connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void handleConnection(TCPConnection *connection, byte* buffer, uint size)
        {

            switch(connection->State)
            {
                case 0:
                        Acknowledge(connection, buffer, size);
                    break;

                case 1:
                        HandleReceive(connection, buffer, size);
                    break;

            }
        }

        /// <summary>
        /// Acknowledge
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void Acknowledge(TCPConnection* connection, byte* buffer, uint size)
        {

            TCPHeader* header = (TCPHeader*)buffer;

            /**
             * Do we need to ACK or SYN|ACK
             */
            if (!connection->InComing && (header->Flags & FLAG_SYN | FLAG_ACK) > 0)
            {
                connection->SequenceNumber = Byte.ReverseBytes(Byte.ReverseBytes(connection->SequenceNumber) + 1);

                connection->State = 1;

                connection->AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1);

                SendPacket(Util.PtrToArray(connection->IP), connection->SequenceNumber, Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + 1), connection->InPort, connection->DestPort, FLAG_ACK, null, 0);
            }
        }

        /// <summary>
        /// Handle packet receiption
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        private static unsafe void HandleReceive(TCPConnection* connection, byte* buffer, uint size)
        {
            TCPHeader* header = (TCPHeader*)buffer;

            if((header->Flags & FLAG_PSH) > 0)
            {
                Console.WriteLine("I has data:");

                int sizePacket = (int)size - sizeof(TCPHeader);

                for (int i = 0; i < sizePacket; i++)
                    Console.Write((char)buffer[sizeof(TCPHeader) + i]);
                Console.WriteLine("\n");

                connection->AcknowledgeNumber = Byte.ReverseBytes(Byte.ReverseBytes(header->Sequence) + (uint)sizePacket);

                SendPacket(Util.PtrToArray(connection->IP), connection->SequenceNumber, connection->AcknowledgeNumber, connection->InPort, connection->DestPort, FLAG_ACK, null, 0);
            }
            else if((header->Flags & FLAG_RST) > 0)
            {
                connection->State = 0xFF;

                // Clear connection!
            }

        }

        /// <summary>
        /// Send packet on connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public static unsafe void Send(TCPConnection *connection, byte *buffer, uint size)
        {
            connection->SequenceNumber = connection->NextSequenceNumber;
            connection->NextSequenceNumber = connection->SequenceNumber + size;


            SendPacket(Util.PtrToArray(connection->IP), connection->SequenceNumber, connection->AcknowledgeNumber, connection->InPort, connection->DestPort, FLAG_PSH | FLAG_ACK, buffer, (int)size);
        }

        /// <summary>
        /// Connection to IP
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <returns></returns>
        public static unsafe TCPConnection* Connect(byte[] ip, ushort port)
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

            TCPConnection* ptr = (TCPConnection*)Heap.Alloc(sizeof(TCPConnection));
            ptr->DestPort = port;
            ptr->InPort = inPort;
            ptr->State = 0;
            ptr->InComing = false;
            ptr->XID = Random.Rand();
            ptr->SequenceNumber = (uint)startSeq;
            ptr->NextSequenceNumber = (uint)Byte.ReverseBytes(Byte.ReverseBytes(ptr->SequenceNumber) + 1);

            for (int i = 0; i < 4; i++)
                ptr->IP[i] = ip[i];

            m_connections[inPort] = ptr;
            
            SendPacket(ip, (uint)startSeq, 0, inPort, port, FLAG_SYN, null, 0);

            return ptr;
        }
    }
}
