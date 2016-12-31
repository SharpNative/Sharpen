using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class DHCP
    {
        private static byte[] m_mac;

        private const byte DHCP_DISCOVER = 1;
        private const byte DHCP_OFFER = 2;
        private const byte DHCP_REQUEST = 3;
        private const byte DHCP_DECLINE = 4;
        private const byte DHCP_AK = 5;
        private const byte DHCP_NAK = 6;
        private const byte DHCP_RELEASE = 7;
        private const byte DHCP_INFORM = 8;

        private const byte HARDTYPE_ETH = 1;

        private const byte OPT_PADDING = 0;
        private const byte OPT_SUBNET = 1;
        private const byte OPT_ROUTER = 3;
        private const byte OPT_DNS = 6;
        private const byte OPT_HOSTNAME = 12;
        private const byte OPT_NTP = 42;
        private const byte OPT_REQ_IP = 50;
        private const byte OPT_LEASE_TIME = 51;
        private const byte OPT_DHCP_MESSAGE_TYPE = 53;
        private const byte OPT_SERVER_ID = 54;
        private const byte OPT_PARAMETER_REQUEST = 55;
        private const byte OPT_CLIENT_ID = 61;
        private const byte OPT_END = 255;


        private const uint MAGISCH_KOEKJE = 0x63825363;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct DHCPBootstrapHeader
        {
            public byte Opcode;

            public byte HardwareType;
            public byte HardwareAddressLength;
            public byte Hops;
            public uint TransactionID;
            public UInt16 SecondsElapsed;
            public UInt16 BootpFlags;
            public fixed byte ClientIP[4];
            public fixed byte YourClientIP[4];
            public fixed byte NextServerIP[4];
            public fixed byte RelayServerIP[4];
            public fixed byte ClientMac[6];
            public fixed byte AddressPadding[10];
            public fixed byte Server[64];
            public fixed byte File[128];
        }

        /// <summary>
        /// Add DHCP header in packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="xid"></param>
        /// <param name="clientIP"></param>
        /// <param name="messageType"></param>
        private static unsafe void addHeader(NetPacketDesc *packet, uint xid, byte[] clientIP, byte messageType)
        {
            DHCPBootstrapHeader *header = (DHCPBootstrapHeader *)(packet->buffer + packet->start);
            Memory.Memset(header, 0, sizeof(DHCPBootstrapHeader));

            header->Opcode = DHCP_REQUEST; // REQUEST
            header->HardwareType = HARDTYPE_ETH; // Ethernet
            header->HardwareAddressLength = 6; // IPV4
            header->Hops = 0;
            header->TransactionID = ByteUtil.ReverseBytes(xid);
            header->SecondsElapsed = 0; // NULLLLL
            header->BootpFlags = 0; // NONNNN

            for (int i = 0; i < 4; i++)
                header->ClientIP[i] = clientIP[i];

            byte* mac = (byte*)Heap.Alloc(6);
            Network.GetMac(mac);

            for (int i = 0; i < 6; i++)
                header->ClientMac[i] = mac[i];

            Heap.Free(mac);

            packet->end += (short)sizeof(DHCPBootstrapHeader);

            // Default options
            byte* opt = (byte*)(packet->buffer + packet->end);

            uint* topt = (uint*)opt;
            *topt = ByteUtil.ReverseBytes(MAGISCH_KOEKJE); // 4 bytes!;
            opt += 4; // Another FOUR!

            // Set message type
            *opt++ = OPT_DHCP_MESSAGE_TYPE; // OPT_DHCP_MESSAGE_TYPE
            *opt++ = 1;
            *opt++ = messageType;

            packet->end += 7;
        }

        private static unsafe void Debug(DHCPBootstrapHeader *header)
        {
            Console.WriteLine("================================\nDHCP DEBUG\n==========================");
            Console.Write("DHCP: opcode = ");
            Console.WriteHex(header->Opcode);
            Console.Write(", htype = ");
            Console.WriteHex(header->HardwareType);
            Console.Write(", hlen = ");
            Console.WriteHex(header->HardwareAddressLength);
            Console.Write(", hops = ");
            Console.WriteHex(header->Hops);
            Console.Write(", xid = ");
            Console.WriteHex(ByteUtil.ReverseBytes(header->TransactionID));
            Console.Write(", secs = ");
            Console.WriteNum(ByteUtil.ReverseBytes(header->SecondsElapsed));
            Console.Write(", flags = ");
            Console.WriteHex(ByteUtil.ReverseBytes(header->BootpFlags));

            Console.WriteLine("");

        }

        public static unsafe void Init()
        {
            // Bind UDP Port :)
            UDP.Bind(68, PacketHandler);

            Discover();
        }

        private static unsafe void PacketHandler(byte[] ip, ushort port, ushort destport, byte* buffer, uint size)
        {
            // We need to do a size check?

            byte type = buffer[sizeof(DHCPBootstrapHeader) + 6];

            switch (type)
            {
                case DHCP_OFFER:
                    request(buffer);
                    break;

                case DHCP_NAK:
                    nak(buffer);
                    break;

                case DHCP_AK:
                    handleAck(buffer);
                    break;
            }

        }

        private static unsafe void handleAck(byte* buffer)
        {
            DHCPBootstrapHeader* header = (DHCPBootstrapHeader*)buffer;

            for (int i = 0; i < 4; i++)
            {
                Network.Settings->IP[i] = header->YourClientIP[i];
            }


            int offset = sizeof(DHCPBootstrapHeader) + 7;
            
            // With sanity check
            while(*(buffer + offset) != 255)
            {
                byte type = *(buffer + offset);
                
                switch (type)
                {
                    case OPT_SERVER_ID:
                        for (int i = 0; i < 4; i++)
                            Network.Settings->ServerID[i] = buffer[offset + 2 + i];

                        offset += 6;
                        break;

                    case OPT_LEASE_TIME:
                        offset += 6;
                        break;

                    case OPT_ROUTER:
                        for (int i = 0; i < 4; i++)
                            Network.Settings->Gateway[i] = buffer[offset + 2 + i];
                        
                        offset += 6;
                        break;

                    case OPT_SUBNET:
                        for (int i = 0; i < 4; i++)
                            Network.Settings->Subnet[i] = buffer[offset + 2 + i];

                        offset += 6;
                        break;

                    case OPT_DNS:
                        {
                            byte len = buffer[offset + 1];

                            for (int i = 0; i < 4; i++)
                                Network.Settings->DNS1[i] = buffer[offset + 2 + i];

                            offset += 6;

                            // More then 1 DNS?
                            if (len > 4)
                            {
                                for (int i = 0; i < 4; i++)
                                    Network.Settings->DNS2[i] = buffer[offset + i];

                                offset += 4;
                            }


                            break;
                        }

                    default:
                        offset++;
                        break;
                }
            }
        }

        private static unsafe void request(byte* buffer)
        {
            DHCPBootstrapHeader* header = (DHCPBootstrapHeader*)buffer;
            NetPacketDesc* packet = NetPacket.Alloc();

            addHeader(packet, ByteUtil.ReverseBytes(header->TransactionID), new byte[4], DHCP_REQUEST);
            
            byte* buf = (byte*)(packet->buffer + packet->end);
            *buf++ = OPT_REQ_IP;
            *buf++ = 4; // IP is 4 bytes
            for (int i = 0; i < 4; i++)
                *buf++ = header->YourClientIP[i];

            packet->end += 6;

            buf = (byte*)(packet->buffer + packet->end);
            *buf++ = OPT_HOSTNAME;
            *buf++ = 8;

            string hostname = Network.GetHostName();

            for (int i = 0; i < 8; i++)
                *buf++ = (byte)hostname[i];

            Heap.Free(Util.ObjectToVoidPtr(hostname));

            packet->end += 10;


            buf = (byte*)(packet->buffer + packet->end);
            *buf++ = OPT_PARAMETER_REQUEST; // OPT_PARAMETER_REQUEST
            *buf++ = 4; // Length of 4 :)
            *buf++ = OPT_SUBNET; // SUBNET
            *buf++ = OPT_ROUTER; // ROUTER
            *buf++ = OPT_NTP; // NTP
            *buf++ = OPT_DNS; // DNS
            *buf++ = OPT_END; // And then
            packet->end += 14;

            byte[] dest = new byte[4];
            for (int i = 0; i < 4; i++)
                dest[i] = 0xFF;

            UDP.Send(packet, dest, 68, 67);

            NetPacket.Free(packet);
        }

        private static unsafe void nak(byte* buffer)
        {
            // TODO: We need to do this dynamic

            char* buf = (char*)(buffer + sizeof(DHCPBootstrapHeader) + 14);

            int len = *buf;

            Console.WriteLine("DHCP Failed:");
            for (int i = 0; i < len; i++)
                Console.Write(buf[i + 1]);
            Console.WriteLine("");
        }

        /// <summary>
        /// DHCP discover (test version)
        /// </summary>
        public static unsafe void Discover()
        {
            m_mac = new byte[6];

            Network.GetMac((byte *)Util.ObjectToVoidPtr(m_mac));

            byte[] dest = new byte[4];
            for (int i = 0; i < 4; i++)
                dest[i] = 0xFF;

            byte[] src = new byte[4];
            for (int i = 0; i < 4; i++)
                src[i] = 0x00;

            uint xid = 0x6666;

            NetPacketDesc* packet = NetPacket.Alloc();

            addHeader(packet, xid, src, 1); // DHCP discover


            // Client ID
            byte* buf = (byte*)(packet->buffer + packet->end);
            *buf++ = OPT_CLIENT_ID; // OPT_CLIENT_ID
            *buf++ = 7; // Length
            *buf++ = 1; // Ethernet

            byte* mac = (byte*)Heap.Alloc(6);
            Network.GetMac(mac);

            for (int i = 0; i < 6; i++)
                *buf++ = mac[i];

            Heap.Free(mac);
            
            packet->end += 9;

            // Request Empty IP
            *buf++ = OPT_REQ_IP; // OPT_REG_IP
            *buf++ = 4;

            packet->end += 6;


            // Specific options
            buf = (byte *)(packet->buffer + packet->end);
            *buf++ = OPT_PARAMETER_REQUEST; // OPT_PARAMETER_REQUEST
            *buf++ = 4; // Length of 4 :)
            *buf++ = OPT_SUBNET; // SUBNET
            *buf++ = OPT_ROUTER; // ROUTER
            *buf++ = OPT_NTP; // NTP
            *buf++ = OPT_DNS; // DNS
            *buf++ = OPT_END; // And then

            packet->end += 14;

            UDP.Send(packet, dest, 68, 67);

            NetPacket.Free(packet);
        }
    }
}
