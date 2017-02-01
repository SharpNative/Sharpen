using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    class NetworkTools
    {
        /// <summary>
        /// Generate WEB checksum
        /// </summary>
        /// <param name="data">Pointer to data</param>
        /// <param name="len">Length</param>
        /// <returns>Length</returns>
        public static unsafe ushort Checksum(byte* data, int len)
        {
            uint sum = 0;
            ushort* p = (ushort*)data;

            while (len > 1)
            {
                sum += *p++;
                len -= 2;
            }

            if (len > 0)
            {
                sum += *(byte*)p;
            }

            sum = (sum & 0xffff) + (sum >> 16);
            sum += (sum >> 16);

            return (ushort)~sum;
        }

        /// <summary>
        /// Converts a string to an IP address
        /// </summary>
        /// <param name="ipIn">The IP address string</param>
        /// <returns>The IP address</returns>
        public static unsafe byte[] StringToIp(string ipIn)
        {
            int num = String.Count(ipIn, '.');
            if (num != 3)
                return null;

            byte[] ip = new byte[4];

            int previousIndex = 0;
            int length = ipIn.Length;
            for (int i = 1; i <= 4; i++)
            {
                int currentIndex = String.IndexOf(ipIn, ".", previousIndex) + 1;
                if (currentIndex == 0)
                    currentIndex = length + 1;

                string part = ipIn.Substring(previousIndex, currentIndex - previousIndex - 1);

                previousIndex = currentIndex;
                ip[i - 1] = (byte)int.Parse(part);

                Heap.Free(part);
            }

            return ip;
        }
    }
}
