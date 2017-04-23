using Sharpen.Collections;
using Sharpen.FileSystem;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    unsafe struct TCPPacketSmallDescriptor
    {
        public TCPPacketDescriptorTypes Type;
        public int Size;
        public long xid;
    }
    
    unsafe struct TCPPacketSendDescriptor
    {
        public TCPPacketSendTypes Type;
        public int Size;
        public long xid;
        public byte* data;
    }

    /// <summary>
    /// Node: needs cleanup!
    /// </summary>
    class TCPSocketDevice
    {

        private TCPConnection Connection { get; set; }
        
        private ushort m_sourcePort;
        private ushort m_targetPort;
        private bool m_connected = false;
        private byte[] m_ip;

        private Queue m_queue;
        private bool m_ipSpecified = false;

        public ushort SourcePort
        {
            get { return m_sourcePort; }
            set { m_sourcePort = value; }
        }

        public ushort TargetPort
        {
            get { return m_targetPort; }
        }

        /// <summary>
        /// Connect to TCP client
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns>If the connection was successful</returns>
        public bool Connect(string ip, ushort port)
        {
            m_ip = NetworkTools.StringToIp(ip);
            if (m_ip == null)
                return false;

            bool found = Route.FindRoute(m_ip);
            if (!found)
                return false;

            m_connected = true;

            m_queue = new Queue();

            m_targetPort = port;

            m_ipSpecified = true;

            Connection = TCP.Connect(m_ip, m_targetPort);

            return true;
        }


        /// <summary>
        /// Bind to port
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns>If the connection was successful</returns>
        public bool Bind(ushort port)
        {
            m_connected = true;

            m_queue = new Queue();

            m_sourcePort = port;

            m_ipSpecified = true;

            Connection = TCP.Bind(m_sourcePort);

            return true;
        }

        public static Node BindNode(string name)
        {
            int port = int.Parse(name);
            if (port == -1)
                return null;

            TCPSocketDevice sock = new TCPSocketDevice();
            bool found = sock.Bind((ushort)port);

            if (!found)
                return null;

            Node node = new Node();
            node.Flags = NodeFlags.FILE;
            node.Read = readImpl;
            node.Write = writeImpl;
            
            // TODO: you need a cookie for this
            //node.Cookie = sock;

            return node;
        }

        public static Node ConnectNode(string name)
        {
            int foundIndex = name.IndexOf(':');
            if (foundIndex == -1)
                return null;

            string ip = name.Substring(0, foundIndex);
            string portText = name.Substring(foundIndex + 1, name.Length - foundIndex - 1);

            int port = int.Parse(portText);
            if (port == -1)
            {
                Heap.Free(portText);
                Heap.Free(ip);

                return null;
            }

            TCPSocketDevice sock = new TCPSocketDevice();
            bool found = sock.Connect(ip, (ushort)port);

            if (!found)
                return null;

            Node node = new Node();
            node.Flags = NodeFlags.FILE;
            node.Read = readImpl;
            node.Write = writeImpl;

            // TODO: you need a cookie for this
            //node.Cookie = sock;

            return node;
        }

        /// <summary>
        /// Read  from TCP connection
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {

            TCPSocketDevice socket = (TCPSocketDevice)node.Cookie;

            if (!socket.m_ipSpecified)
                return 0;

            /**
             * Create entry
             */
            TCPPacketDescriptor* entry = TCP.Read(socket.Connection);
            if (entry == null)
                return 0;

            /**
             * Check size of packet
             */
            int totalSize = entry->Size + sizeof(TCPPacketSmallDescriptor); 

            if (size > totalSize)
                size = (uint)totalSize;
            uint remaining = size;
            int entrySize = (int)size;

            /**
             * Copy descriptor
             */
            if (size > sizeof(TCPPacketSmallDescriptor))
                entrySize = sizeof(TCPPacketSmallDescriptor);

            Memory.Memcpy(Util.ObjectToVoidPtr(buffer), entry, entrySize);


            remaining -= (uint)entrySize;
            if(remaining > 0)
            {
                /**
                 * Copy data
                 */
                Memory.Memcpy((byte *)Util.ObjectToVoidPtr(buffer) + entrySize, entry->Data, entrySize);
            }

            Heap.Free(entry);

            return size;
        }


        /// <summary>
        /// Writes data to the TCP connection
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The destination buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            if (size < sizeof(TCPPacketSendDescriptor))
                return 0;

            TCPSocketDevice socket = (TCPSocketDevice)node.Cookie;

            if (!socket.m_ipSpecified)
                return 0;

            TCPPacketSendDescriptor* descr = (TCPPacketSendDescriptor*)Util.ObjectToVoidPtr(buffer);
            
            switch (descr->Type)
            {
                case TCPPacketSendTypes.CLOSE:
                    if (socket.Connection.State == TCPConnectionState.LISTEN)
                        TCP.Close(socket.Connection, descr->xid);
                    break;

                case TCPPacketSendTypes.SEND:
                    if (descr->data == null)
                        return 0;

                    if (socket.Connection.State == TCPConnectionState.LISTEN)
                        TCP.Send(socket.Connection, descr->data, size);
                    else
                        TCP.Send(socket.Connection, descr->data, size);
                    break;
            }

            return size;
        }

    }
}
