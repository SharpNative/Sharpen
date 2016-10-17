using Sharpen.Mem;
using Sharpen.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                byte* pa = (byte*)p;
                sum += *pa;
            }


            sum = (sum & 0xffff) + (sum >> 16);
            sum += (sum >> 16);

            return (ushort)~sum;
        }
        
        public static unsafe byte[] StringToIp(string ipIn)
        {
            int num = String.Count(ipIn, '.');

            if (num != 3)
                return null;

            byte[] ip = new byte[4];

            int index = String.IndexOf(ipIn, ".");
            string part = String.SubString(ipIn, 0, index);
            string remaning = String.SubString(ipIn, index + 1, String.Length(ipIn) - index + 1);

            int i = 0; 
            while(i < 4)
            {
                ip[i] = (byte)Int.Parse(part);

                Heap.Free(Util.ObjectToVoidPtr(part));

                index = String.IndexOf(remaning, ".");
                if (i == 2)
                    index = String.Length(remaning);
                else if(index == -1)
                {
                    break;
                }

                part = String.SubString(remaning, 0, index);


                char* oldRemaning = (char*)Util.ObjectToVoidPtr(remaning);
                remaning = String.SubString(remaning, index + 1, String.Length(remaning) - index + 1);

                Heap.Free(oldRemaning);

                i++;
            }

            return ip;
        }
    }
}
