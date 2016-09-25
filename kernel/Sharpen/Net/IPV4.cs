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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IPV4Header
    {
        public byte Version;
        public byte ServicesField;
        public UInt16 totalLength;
        public UInt16 ID;
        public UInt16 FragmentOffset;
        public byte TTL;
        public byte Protocol;
        public UInt16 HeaderChecksum;
        public fixed byte Source[4];
        public fixed byte Destination[4];
    }

    class IPV4
    {
        // Network packet type handler
        public unsafe delegate void PackerHandler(uint xid, byte* buffer, uint size);

        private static PackerHandler[] m_handlers;

        public static unsafe void Init()
        {
            m_handlers = new PackerHandler[255];
            Network.RegisterHandler(0x0800, Handle);
        }

        private static unsafe void Handle(byte* buffer, uint size)
        {
            IPV4Header* header = (IPV4Header*)buffer;

            byte proto = header->Protocol;
            
            m_handlers[proto]?.Invoke(ByteUtil.ReverseBytes(header->ID), buffer + sizeof(IPV4Header), size);
        }

        public static void RegisterHandler(byte proto, PackerHandler handler)
        {
            m_handlers[proto] = handler;
        }
        

        public static unsafe IPV4Header *FillHeader(NetBufferDescriptor *packet, byte[] destMac, byte[] sourceIP, byte[] destIP, byte protocol)
        {
            byte[] mymac = new byte[6];
            Network.GetMac((byte*)Util.ObjectToVoidPtr(mymac));

            packet->start -= (short)sizeof(IPV4Header);


            IPV4Header* header = (IPV4Header*)packet->buffer + packet->start;

            header->Version = 0x45;
            header->ServicesField = 0;
            header->totalLength = ByteUtil.ReverseBytes((ushort)(packet->end - packet->start));
            header->ID = 0x36A8;
            header->FragmentOffset = 0;
            header->TTL = 250;
            header->Protocol = protocol;
            header->HeaderChecksum = ByteUtil.ReverseBytes(0x178B);
            for (int i = 0; i < 4; i++)
                header->Source[i] = sourceIP[i];
            for (int i = 0; i < 4; i++)
                header->Destination[i] = destIP[i];


            return header;
        }
        

        public static unsafe void Send(NetBufferDescriptor* packet, byte[] destMac, byte[] destIP, byte protocol)
        {
            byte[] sourceIP = { 0x00, 0x00, 0x00, 0x00 };
            FillHeader(packet, destMac, destIP, sourceIP, protocol);


        }
    }
}
