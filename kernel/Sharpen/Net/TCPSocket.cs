using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class TCPSocket
    {
        private ushort m_sourcePort;
        private ushort m_targetPort;
        private bool m_connected = false;
        private byte[] m_ip;
        private bool m_ipSpecified = false;


        /// <summary>
        /// Connect to UDP client
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns></returns>
        public bool Connect(string ip, ushort port)
        {
            m_ip = NetworkTools.StringToIp(ip);
            if (m_ip == null)
                return false;

            bool found = ARP.IpExists(m_ip);
            if (!found)
                return false;

            m_connected = true;
            
            m_targetPort = port;

            m_ipSpecified = true;

            // Register a sourcePort
            //TCP.BindSocketRequest(this);

            return true;
        }


        /// <summary>
        ///  Bind to port
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Target port</param>
        /// <returns></returns>
        public bool Bind(ushort port)
        {
            m_sourcePort = port;

            // Register a sourcePort
            //TCP.BindSocket(this);

            m_ipSpecified = false;
            m_connected = true;

            return true;
        }
    }
}
