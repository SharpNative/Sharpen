using Sharpen.Arch;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Power
{
    public sealed unsafe class Acpi
    {
        private static RDSP* rdsp;
        private static RSDT* rsdt;
        private static FADT* fadt;

        private static ushort SLP_TYPa;
        private static ushort SLP_TYPb;

        private static readonly uint SLP_EN = 1 << 13;

        /// <summary>
        /// Find the RSDT and other entries
        /// </summary>
        private static void Find()
        {
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
                    Panic.DoPanic("RDSP not found!");
            }

            rsdt = (RSDT*)rdsp->RsdtAddress;
            if (rsdt == null)
                Panic.DoPanic("RDST not found!");

            fadt = (FADT*)getEntry("FACP");
            if (fadt == null)
                Panic.DoPanic("FACP not found!");
        }

        /// <summary>
        /// Checksum
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="length">The length</param>
        /// <returns>If the check was successfull</returns>
        private static bool CheckSum(void* address, uint length)
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

        /// <summary>
        /// Parses the S5 object
        /// </summary>
        private static void parseS5Object()
        {
            bool s5Found = false;

            uint dsdtLength = (fadt->Dsdt + 1) - 36;
            byte* s5Address = (byte*)fadt->Dsdt + 36;

            if (dsdtLength > 0)
            {
                while (dsdtLength > 0)
                {
                    if (Memory.Compare((char*)s5Address, (char*)Util.ObjectToVoidPtr("_S5_"), 4))
                    {
                        s5Found = true;
                        break;
                    }

                    s5Address++;
                }

                if (!s5Found)
                    Panic.DoPanic("Could not find S5 object!");

                // Check S5 object structure
                if ((*(s5Address - 1) == 0x08 || (*(s5Address - 2) == 0x08 && *(s5Address - 1) == '\\')) && *(s5Address + 4) == 0x12)
                {
                    s5Address += 5;
                    s5Address += ((*s5Address & 0xC0) >> 6) + 2;

                    if (*s5Address == 0x0A)
                        s5Address++;

                    SLP_TYPa = (ushort)(*(s5Address) << 10);

                    s5Address++;

                    if (*s5Address == 0x0A)
                        s5Address++;

                    SLP_TYPb = (ushort)(*(s5Address) << 10);
                }
                else
                {
                    Panic.DoPanic("S5 object has not the right structure");
                }
            }
        }

        /// <summary>
        /// Get an entry from the RSDT
        /// </summary>
        /// <param name="signature">The signature</param>
        /// <returns>The entry</returns>
        private static void* getEntry(string signature)
        {
            if (rsdt == null)
                return null;

            uint n = (uint)(rsdt->header.Length - sizeof(RDSTH)) / 4;

            for (uint i = 0; i < n; i++)
            {
                RDSTH* header = (RDSTH*)(rsdt->firstSDT + i);

                if (Memory.Compare((char*)header, (char*)Util.ObjectToVoidPtr(signature), 4))
                    if (CheckSum(header, header->Length))
                        return (void*)header;

            }

            return null;
        }

        /// <summary>
        /// Initalize ACPI
        /// </summary>
        public static void Init()
        {
            Find();
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

        /// <summary>
        /// Power reset
        /// </summary>
        public static void Reset()
        {
            // TODO: Fix nice reset :)
            // PortIO.Out8((ushort)fadt->ResetReg.Address, fadt->ResetValue);

            byte good = 0x02;
            while ((good & 0x02) > 0)
                good = PortIO.In8(0x64);
            PortIO.Out8(0x64, 0xFE);
        }

        /// <summary>
        /// Power shutdown
        /// </summary>
        public static void Shutdown()
        {
            // Try 1 through the pm1a control block
            PortIO.Out16((ushort)fadt->PM1aControlBlock, (ushort)(SLP_TYPa | SLP_EN));
            // Try 2 through the pm1b control block
            PortIO.Out16((ushort)fadt->PM1aControlBlock, (ushort)(SLP_TYPb | SLP_EN));
        }
    }
}
