using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class Route
    {
        private static byte[] m_gateway_mac;
        private static byte[] m_ip_local_network;

        public static void Init()
        {
            m_gateway_mac = new byte[6];
            m_ip_local_network = new byte[3];
        }

        /// <summary>
        /// Set gateway
        /// </summary>
        /// <param name="ip">IP</param>
        public unsafe static void SetGateway(byte[] ip)
        {
            /*
             * We just always assume subnet is 255.255.255.0
             */
            for (int i = 0; i < 3; i++)
                m_ip_local_network[i] = ip[i];
            
            ARP.Lookup(ip, (byte *)Util.ObjectToVoidPtr(m_gateway_mac));
        }

        /// <summary>
        /// Find route "mac" address
        /// </summary>
        /// <param name="ip"></param>
        public unsafe static bool FindRoute(byte[] ip, byte* DestMac)
        {
            if(InLocalNetwork(ip))
            {
                ARP.Lookup(ip, DestMac);
                
                if (DestMac[0] == 0x00 && DestMac[1] == 0x00 && DestMac[2] == 0x00 && DestMac[3] == 0x00 && DestMac[4] == 0x00 && DestMac[5] == 0x00)
                {
                    byte[] mac = new byte[6];
                    for (int i = 0; i < 6; i++)
                        mac[i] = 0xFF;

                    ARP.ArpSend(ARP.OP_REQUEST, mac, ip);

                    return false;
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    DestMac[i] = m_gateway_mac[i];
            }
            
            return true;
        }

        /// <summary>
        /// Find route "mac" address
        /// </summary>
        /// <param name="ip"></param>
        public unsafe static bool FindRoute(byte[] ip)
        {
            if (InLocalNetwork(ip))
                return ARP.IpExists(ip);
            
            return true;
        }

        /// <summary>
        /// Is ip in local network?
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private unsafe static bool InLocalNetwork(byte[] ip)
        {
            if (ip[0] == 0xFF && ip[1] == 0xFF && ip[2] == 0xFF && ip[3] == 0xFF)
                return true;

            bool local = true;
            for (int i = 0; i < 3; i++)
                if (m_ip_local_network[i] != ip[i])
                    local = false;

            return local;
        }
    }
}
