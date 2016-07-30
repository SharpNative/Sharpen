using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Power
{
    unsafe class Acpi
    {

        private static RDSP *rdsp;
        private static RSDT *rsdt;
        private static FADT *fadt;

        private static ushort SLP_TYPa;
        private static ushort SLP_TYPb;

        private const uint LP_EN = 1 << 13;

        private static void find()
        {

            // Finding the RSDP

            // First attempt in bios data
            byte* biosp = (byte*)0x000E0000;

            while((uint) biosp < 0x000FFFFF)
            {

                if(Memory.Compare((char *)biosp, (char *)Util.ObjectToVoidPtr("RSD PTR "), 8) && check_sum((uint *)biosp, (uint)sizeof(RSDT)))
                {
                    rdsp = (RDSP *)biosp;
                    break;
                }

                biosp += 16;
            }

            // Search the rdsp through bios data when the rdsp is not found in the ebda
            if (rdsp == (RDSP*)0)
            {

                ushort *adr = (ushort*)0x040E;
                byte* ebdap = (byte*)((*adr) << 4);

                while ((uint)ebdap < 0x000A0000)
                {
                    if (Memory.Compare((char*)ebdap, (char*)Util.ObjectToVoidPtr("RSD PTR "), 8) && check_sum((uint*)ebdap, (uint)sizeof(RSDT)))
                    {
                        rdsp = (RDSP*)biosp;
                        break;
                    }

                    ebdap += 16;
                }

                if (rdsp == (RDSP*)0)
                    Panic.DoPanic("RDSP not found!");
            }

            rsdt = (RSDT*)rdsp->RsdtAddress;
            if (rsdt == (RSDT*)0)
                Panic.DoPanic("RDST not found!");
        }

        private static bool check_sum(void *address, uint length)
        {
            char* bptr = (char*)address;
            char check = (char)0;

            for(int i = 0; i < length; i++)
            {
                check += *bptr;
                bptr++;
            }

            if (check == 0)
                return true;

            return false;
        }

        public static void Init()
        {
            find();
        }
    }
}
