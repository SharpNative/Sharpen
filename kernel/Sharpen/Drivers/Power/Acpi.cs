using Sharpen.Arch;
using Sharpen.Drivers.Char;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Power
{
    public sealed unsafe class Acpi
    {
        private static RDSP* m_rdsp;
        private static RSDT* m_rsdt;
        private static FADT* m_fadt;

        private static ushort SLP_TYPa;
        private static ushort SLP_TYPb;

        private const uint SLP_EN = 1 << 13;

        /// <summary>
        /// 
        /// Finds SLP_TYPa and SLP_TYPb
        /// 
        /// </summary>
        private static void SetTypes()
        {
            // TODO: make this work though a AML interpreter/parser

            bool s5Found = false;

            uint dsdtLength = (m_fadt->Dsdt + 1) - 36;
            byte* s5Address = (byte*)m_fadt->Dsdt + 36;

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
            if (m_rsdt == null)
                return null;

            uint n = (uint)(m_rsdt->header.Length - sizeof(RDSTH)) / 4;

            for (uint i = 0; i < n; i++)
            {
                RDSTH* header = (RDSTH*)(m_rsdt->firstSDT + i);

                if (Memory.Compare((char*)header, (char*)Util.ObjectToVoidPtr(signature), 4))
                    if (AcpiHelper.CheckSum(header, header->Length))
                        return (void*)header;

            }

            return null;
        }

        /// <summary>
        /// Find and set RDSP table
        /// </summary>
        public static void SetRDSP()
        {
            RDSP *rdsp = AcpiHelper.FindRSDP();

            if (rdsp == null)
                return;

            m_rdsp = (RDSP*)Heap.Alloc(sizeof(RDSP));

            Memory.Memcpy(m_rdsp, rdsp, sizeof(RDSP));
        }

        /// <summary>
        /// Set addresses of other tables FADT, etc...
        /// </summary>
        public static void SetTables()
        {
            RSDT* rsdt = null;
            FADT* fadt = null;
            m_rsdt = (RSDT*)Heap.Alloc(sizeof(RSDT));
            m_fadt = (FADT*)Heap.Alloc(sizeof(FADT));


            rsdt = (RSDT*)m_rdsp->RsdtAddress;
            if (rsdt == null)
                Panic.DoPanic("RDST not found!");

            Memory.Memcpy(m_rsdt, rsdt, sizeof(RSDT));

            fadt = (FADT*)getEntry("FACP");
            if (fadt == null)
                Panic.DoPanic("FACP not found!");

            Memory.Memcpy(m_fadt, fadt, sizeof(FADT));
        }

        /// <summary>
        /// Initalize ACPI
        /// </summary>
        public static void Init()
        {
            SetRDSP();
            if (m_rdsp == null)
                return;

            SetTables();
            SetTypes();
            Enable();
        }

        /// <summary>
        /// Enable ACPI
        /// </summary>
        public static void Enable()
        {
            PortIO.Out8((ushort)m_fadt->SMI_CommandPort, m_fadt->AcpiEnable);
        }

        /// <summary>
        /// Disable ACPI
        /// </summary>
        public static void Disable()
        {
            PortIO.Out8((ushort)m_fadt->SMI_CommandPort, m_fadt->AcpiDisable);
        }

        private static void Sleep(int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }

        /// <summary>
        /// Power reset
        /// </summary>
        public static void Reboot()
        {
            // If we have a REV 2 header, then we can just extract it
            if(m_fadt->Header.Revision >= 2)
            {
                PortIO.Out8((ushort)m_fadt->ResetReg.Address, m_fadt->ResetValue);
            }

            // Do a lucky try
            PortIO.Out8(0xCF9, 0x06);

            Sleep(5000);

            // Otherwise just reset through keyboard
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
            for (;;) ;
            // Try 1 through the pm1a control block
            PortIO.Out16((ushort)m_fadt->PM1aControlBlock, (ushort)(SLP_TYPa | SLP_EN));
            // Try 2 through the pm1b control block
            PortIO.Out16((ushort)m_fadt->PM1aControlBlock, (ushort)(SLP_TYPb | SLP_EN));
        }
    }
}
