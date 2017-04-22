using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Power
{
    public sealed unsafe class Acpi
    {
        private static RSDP* m_rsdp;
        private static RSDT* m_rsdt;
        private static FADT* m_fadt;
        private static MADT* m_madt;
        private static RSDTH* m_dsdt;

        private static ushort SLP_TYPa;
        private static ushort SLP_TYPb;

        private const uint SLP_EN = 1 << 13;

        /// <summary>
        /// Finds SLP_TYPa and SLP_TYPb
        /// </summary>
        private static void setTypes()
        {
            // TODO: make this work though a AML interpreter/parser
            
            m_dsdt = mapEntry((RSDTH*)m_fadt->Dsdt);
            uint dsdtLength = m_dsdt->Length - (uint)sizeof(RSDTH);
            byte* s5Address = (byte*)m_dsdt + sizeof(RSDTH);
            
            if (dsdtLength > 0)
            {
                while (dsdtLength-- > 0)
                {
                    if (Memory.Compare((char*)s5Address, (char*)Util.ObjectToVoidPtr("_S5_"), 4))
                    {
                        break;
                    }

                    s5Address++;
                }

                if (dsdtLength == 0)
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
        /// Parses the MADT
        /// </summary>
        /// <param name="madt">The MADT</param>
        private static void parseMADT(MADT* madt)
        {
            LocalApic.SetLocalControllerAddress(madt->LocalControllerAddress);

            // After the flags field, the rest of the table contains a variable length of records
            uint current = (uint)madt + (uint)sizeof(MADT);
            uint end = current + madt->Header.Length - (uint)sizeof(MADT);

            while (current < end)
            {
                ApicEntryHeader* header = (ApicEntryHeader*)current;
                ApicEntryHeaderType type = (ApicEntryHeaderType)header->Type;

                switch (type)
                {
                    case ApicEntryHeaderType.LOCAL_APIC:
                        ApicLocalApic* localAPIC = (ApicLocalApic*)(current + sizeof(ApicEntryHeader));
                        Console.Write("[ACPI] Found CPU ");
                        Console.WriteNum(localAPIC->ProcessorID);
                        Console.Write('\n');
                        break;

                    case ApicEntryHeaderType.IO_APIC:
                        ApicIOApic* IOApicStruct = (ApicIOApic*)(current + sizeof(ApicEntryHeader));

                        Console.Write("[ACPI] Found IO APIC at ");
                        Console.WriteHex(IOApicStruct->IOAPICAddress);
                        Console.Write('\n');
                        
                        IOApic IOApic = new IOApic(IOApicStruct->IOAPIC_ID, (void*)IOApicStruct->IOAPICAddress, IOApicStruct->GlobalSystemInterruptBase);
                        IOApicManager.Add(IOApic);

                        break;

                    case ApicEntryHeaderType.INTERRUPT_SOURCE_OVERRIDE:
                        //ApicInterruptSourceOverride* intSourceOverride = (ApicInterruptSourceOverride*)(current + sizeof(ApicEntryHeader));
                        
                        //int polarity = intSourceOverride->Flags & 0x3;
                        //int trigger = (intSourceOverride->Flags & (3 << 2)) >> 2;

                        //Console.Write("Found interrupt override: ");
                        //Console.WriteNum(intSourceOverride->BusSource);
                        //Console.Write(' ');
                        //Console.WriteNum(intSourceOverride->IRQSource);
                        //Console.Write(' ');
                        //Console.WriteNum((int)intSourceOverride->GlobalSystemInterrupt);
                        //Console.Write(' ');
                        //Console.WriteNum(polarity);
                        //Console.Write(' ');
                        //Console.WriteNum(trigger);
                        //Console.Write('\n');
                        
                        break;
                }

                current += header->Length;
            }
        }

        /// <summary>
        /// Maps an entry
        /// </summary>
        /// <param name="entry">The entry</param>
        /// <returns>The mapped entry</returns>
        private static RSDTH* mapEntry(RSDTH* entry)
        {
            // Align address down so we can get the offset
            uint down = Paging.AlignDown((uint)entry);
            uint offset = (uint)entry - down;

            // If we don't have enough to map the header, add an extra page
            int mapSize = 0x1000;
            if (mapSize - offset < sizeof(RSDTH))
            {
                mapSize += 0x1000;
            }

            // Actual accessible size after mapping
            uint accessibleSize = (uint)mapSize - offset;

            // Map header, if we don't have enough yet, we can map more
            void* mapped = Paging.MapToVirtual(Paging.CurrentDirectory, (int)entry, mapSize, Paging.PageFlags.Present);
            RSDTH* header = (RSDTH*)((uint)mapped + offset);

            // Length is bigger than what we already can access using the mapping?
            // then unmap and map more
            if (header->Length > accessibleSize)
            {
                Paging.UnMap(mapped, mapSize);
                mapped = Paging.MapToVirtual(Paging.CurrentDirectory, (int)entry, (int)Paging.AlignUp(header->Length + offset), Paging.PageFlags.Present);
                header = (RSDTH*)((uint)mapped + offset);
            }

            return header;
        }

        /// <summary>
        /// Unmaps an entry
        /// </summary>
        /// <param name="entry">The entry</param>
        private static void unMapEntry(RSDTH* entry)
        {
            // Align address down, get size of entry and unmap
            uint down = Paging.AlignDown((uint)entry);
            uint offset = (uint)entry - down;
            Paging.UnMap((void*)down, (int)(entry->Length + offset));
        }

        /// <summary>
        /// Compares a signature of a header with a string
        /// </summary>
        /// <param name="rsdth">The header</param>
        /// <param name="signature">The signature to check for</param>
        /// <returns>If the signature matches</returns>
        private static bool compareSignature(RSDTH* rsdth, string signature)
        {
            return (rsdth->Signature[0] == signature[0] &&
                rsdth->Signature[1] == signature[1] &&
                rsdth->Signature[2] == signature[2] &&
                rsdth->Signature[3] == signature[3]);
        }

        /// <summary>
        /// Finds the RSDP
        /// </summary>
        /// <returns>The RSDP</returns>
        private static unsafe RSDP* findRSDP()
        {
            char* checkPtr = (char*)Util.ObjectToVoidPtr("RSD PTR ");
            RSDP* rsdp = (RSDP*)Util.FindStructure(checkPtr, 8, (void*)0x000E0000, (void*)0x000FFFFF, sizeof(RSDT));

            // Search the rdsp through bios data when the rdsp is not found in the ebda
            if (rsdp == null)
            {
                ushort* adr = (ushort*)0x040E;
                byte* ebdap = (byte*)((*adr) << 4);

                rsdp = (RSDP*)Util.FindStructure(checkPtr, 8, ebdap, (void*)0x000A0000, sizeof(RSDT));
            }

            return rsdp;
        }

        /// <summary>
        /// Set addresses of other tables FADT, etc...
        /// </summary>
        public static void SetTables()
        {
            m_rsdt = (RSDT*)mapEntry((RSDTH*)m_rsdp->RsdtAddress);
            if (m_rsdt == null)
                Panic.DoPanic("RSDT not found!");

            // Entry count
            uint entries = (uint)(m_rsdt->Header.Length - sizeof(RSDTH)) / 4;
            uint* addresses = (uint*)((uint)m_rsdt + (uint)sizeof(RSDTH));
            
            for (uint i = 0; i < entries; i++)
            {
                RSDTH* header = (RSDTH*)addresses[i];
                RSDTH* mapped = mapEntry(header);

                // Found a valid entry
                if (Util.ZeroCheckSum(mapped, mapped->Length))
                {
                    if (compareSignature(mapped, "FACP"))
                    {
                        m_fadt = (FADT*)mapped;
                    }
                    else if (compareSignature(mapped, "APIC"))
                    {
                        m_madt = (MADT*)mapped;
                    }
                }
                // Invalid entry
                else
                {
                    unMapEntry(mapped);
                }
            }

            // Check if the needed entries have been found
            if (m_fadt == null)
                Panic.DoPanic("FACP not found!");

            if (m_madt == null)
                Panic.DoPanic("MADT not found!");

            parseMADT(m_madt);
        }

        /// <summary>
        /// Initalize ACPI
        /// </summary>
        public static void Init()
        {
            m_rsdp = findRSDP();
            if (m_rsdp == null)
                Panic.DoPanic("No RSDP found");

            SetTables();
            setTypes();
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

        /// <summary>
        /// Sleeps a couple of ns
        /// </summary>
        /// <param name="cnt"></param>
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
            if (m_fadt->Header.Revision >= 2)
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
            // Try 1 through the pm1a control block
            PortIO.Out16((ushort)m_fadt->PM1aControlBlock, (ushort)(SLP_TYPa | SLP_EN));

            // Try 2 through the pm1b control block
            PortIO.Out16((ushort)m_fadt->PM1aControlBlock, (ushort)(SLP_TYPb | SLP_EN));
        }
    }
}
