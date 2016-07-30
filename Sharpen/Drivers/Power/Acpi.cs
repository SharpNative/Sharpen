using Sharpen.Arch;
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

        private static readonly uint SLP_EN = 1 << 13;

        private static void find()
        {

            // Finding the RSDP

            // First attempt in bios data
            byte* biosp = (byte*)0x000E0000;

            while((uint) biosp < 0x000FFFFF)
            {

                if(Memory.Compare((char *)biosp, (char *)Util.ObjectToVoidPtr("RSD PTR "), 8) && checkSum((uint *)biosp, (uint)sizeof(RSDT)))
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
                    if (Memory.Compare((char*)ebdap, (char*)Util.ObjectToVoidPtr("RSD PTR "), 8) && checkSum((uint*)ebdap, (uint)sizeof(RSDT)))
                    {
                        rdsp = (RDSP*)biosp;
                        break;
                    }

                    ebdap += 16;
                }

                if (rdsp == (RDSP*)0)
                    Panic.DoPanic("RDSP not found!");
            }

            char b = rdsp->OEMID[0];
            
            rsdt = (RSDT*)rdsp->RsdtAddress;
            if (rsdt == (RSDT*)0)
                Panic.DoPanic("RDST not found!");
            fadt = (FADT*)getEntry("FACP");
            if (fadt == (FADT*)0)
                Panic.DoPanic("FACP not found!");

        }

        private static bool checkSum(void *address, uint length)
        {
            Console.WriteHex((int)address);
            Console.Write(" ");
            Console.WriteNum((int)length);
            Console.WriteLine(" ");

            char* bptr = (char*)address;
            byte check = 0;

            for(int i = 0; i < length; i++)
            {
                check += (byte)*bptr;
                bptr++;
            }


            Console.WriteNum((int)check);
            Console.WriteLine("");
            if (check == 0)
                return true;

            return false;
        }

        private static void parseS5Object()
        {
            bool s5_found = false;

            uint dsdtLength = (fadt->Dsdt + 1) - 36;
            byte* s5_addr = (byte*)fadt->Dsdt + 36;

            if(dsdtLength > 0)
            {
                while(dsdtLength > 0)
                {
                    if(Memory.Compare((char*)s5_addr, (char *)Util.ObjectToVoidPtr( "_S5_"), 4))
                    {
                        s5_found = true;
                        break;
                    }

                    s5_addr++;
                }

                if (!s5_found)
                    Panic.DoPanic("Could not find S5 object!");

                // Check S5 object structure
                if ((*(s5_addr - 1) == 0x08 || (*(s5_addr - 2) == 0x08 && *(s5_addr - 1) == '\\')) && *(s5_addr + 4) == 0x12)
                {
                    s5_addr += 5;
                    s5_addr += ((*s5_addr & 0xC0) >> 6) + 2;

                    if (*s5_addr == 0x0A)
                        s5_addr++;

                    SLP_TYPa = (ushort)(*(s5_addr) << 10);

                    s5_addr++;

                    if (*s5_addr == 0x0A)
                        s5_addr++;

                    SLP_TYPb = (ushort)(*(s5_addr) << 10);
                }
                else
                {
                    Panic.DoPanic("S5 object has not the right structure");
                }
            }
        }

        private static void *getEntry(string signature)
        {
            if (rsdt == null)
                return null;

            uint n = (uint)(rsdt->header.Length - sizeof(RDSTH)) / 4;
            
            for(uint i = 0; i < n; i++)
            {
                RDSTH* header = (RDSTH*)(rsdt->firstSDT + i);

                if (Memory.Compare((char*)header, (char*)Util.ObjectToVoidPtr(signature), 4))
                    if(checkSum(header, header->Length))
                        return (void*)header;

            }

            return null;
        }

        /// <summary>
        /// Initalize ACPI
        /// </summary>
        public static void Init()
        {
            find();
            parseS5Object();
            Enable();
        }

        /// <summary>
        /// Enable ACPI
        /// </summary>
        public static void Enable()
        {
            PortIO.Out8((ushort)fadt->SMI_CommandPort, fadt->AcpiEnable);
        }

        /// <summary>
        /// Disable ACPI
        /// </summary>
        public static void Disable()
        {
            PortIO.Out8((ushort)fadt->SMI_CommandPort, fadt->AcpiDisable);
        }

        public static void Reset()
        {
            // TODO: Fix nice reset :)
            // PortIO.Out8((ushort)fadt->ResetReg.Address, fadt->ResetValue);
            
            byte good = 0x02;
            while ((good & 0x02) > 0)
                good = PortIO.In8(0x64);
            PortIO.Out8(0x64, 0xFE);
        }

        public static void Shutdown()
        {
            // try 1 through the pm1a control block
            PortIO.Out16((ushort)fadt->PM1aControlBlock, (ushort)(SLP_TYPa | SLP_EN));
            // try 2 through the pm1b control block
            PortIO.Out16((ushort)fadt->PM1aControlBlock, (ushort)(SLP_TYPb | SLP_EN));
        }
    }
}
