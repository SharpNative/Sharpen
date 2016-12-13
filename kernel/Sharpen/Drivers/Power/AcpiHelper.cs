using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Power
{
    class AcpiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static unsafe RDSP* FindRSDP()
        {
            RDSP* rdsp = null;
            RDSP *rdspRet = (RDSP*)Heap.Alloc(sizeof(RDSP));

            // First attempt in bios data
            byte* biosp = (byte*)0x000E0000;

            while ((uint)biosp < 0x000FFFFF)
            {
                if (Memory.Compare((char*)biosp, (char*)Util.ObjectToVoidPtr("RSD PTR "), 8) && CheckSum((uint*)biosp, (uint)sizeof(RSDT)))
                {
                    rdsp = (RDSP*)biosp;
                    break;
                }

                biosp += 16;
            }

            // Search the rdsp through bios data when the rdsp is not found in the ebda
            if (rdsp == null)
            {
                ushort* adr = (ushort*)0x040E;
                byte* ebdap = (byte*)((*adr) << 4);

                while ((int)ebdap < 0x000A0000)
                {
                    if (Memory.Compare((char*)ebdap, (char*)Util.ObjectToVoidPtr("RSD PTR "), 8) && CheckSum((uint*)ebdap, (uint)sizeof(RSDT)))
                    {
                        rdsp = (RDSP*)biosp;
                        break;
                    }

                    ebdap += 16;
                }

                if (rdsp == null)
                    return null;
            }

            Memory.Memcpy(rdspRet, rdsp, sizeof(RDSP));

            return rdspRet;
        }


        /// <summary>
        /// Checksum
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="length">The length</param>
        /// <returns>If the check was successfull</returns>
        public unsafe static bool CheckSum(void* address, uint length)
        {
            char* bptr = (char*)address;
            byte check = 0;

            for (int i = 0; i < length; i++)
            {
                check += (byte)*bptr;
                bptr++;
            }

            if (check == 0)
                return true;

            return false;
        }
    }
}
