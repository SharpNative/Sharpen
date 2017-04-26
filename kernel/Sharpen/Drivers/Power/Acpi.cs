using Sharpen.Arch;
using Sharpen.Power;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Power
{
    public sealed unsafe class Acpi
    {
        private static MADT* m_madt;

        /// <summary>
        /// Parses the MADT
        /// </summary>
        /// <param name="madt">The MADT</param>
        private static void parseMADT(MADT* madt)
        {
            m_madt = madt;
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
                        ApicInterruptSourceOverride* intSourceOverride = (ApicInterruptSourceOverride*)(current + sizeof(ApicEntryHeader));

                        int polarity = intSourceOverride->Flags & 0x3;
                        int trigger = (intSourceOverride->Flags & (3 << 2)) >> 2;

                        Console.Write("Found interrupt override: ");
                        Console.WriteNum(intSourceOverride->BusSource);
                        Console.Write(' ');
                        Console.WriteNum(intSourceOverride->IRQSource);
                        Console.Write(' ');
                        Console.WriteNum((int)intSourceOverride->GlobalSystemInterrupt);
                        Console.Write(' ');
                        Console.WriteNum(polarity);
                        Console.Write(' ');
                        Console.WriteNum(trigger);
                        Console.Write('\n');

                        break;
                }

                current += header->Length;
            }
        }

        /// <summary>
        /// Finds the RSDP
        /// </summary>
        /// <returns>The RSDP</returns>
        public static unsafe void* FindRSDP()
        {
            char* checkPtr = (char*)Util.ObjectToVoidPtr("RSD PTR ");
            void* rsdp = Util.FindStructure(checkPtr, 8, (void*)0x000E0000, (void*)0x000FFFFF, 36);

            // Search the rdsp through bios data when the rdsp is not found in the ebda
            if (rsdp == null)
            {
                ushort* adr = (ushort*)0x040E;
                byte* ebdap = (byte*)((*adr) << 4);

                rsdp = Util.FindStructure(checkPtr, 8, ebdap, (void*)0x000A0000, 36);
            }

            return rsdp;
        }

        /// <summary>
        /// Initializes ACPI
        /// </summary>
        public static void Init()
        {
            int status = Acpica.AcpiInitializeSubsystem();
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not initialize ACPICA subsystem");
                return;
            }

            status = Acpica.AcpiInitializeTables(null, 16, false);
            if (status != Acpica.AE_OK)
            {
                Console.WriteHex(status);
                Panic.DoPanic("[ACPI] Could not initialize tables");
                return;
            }

            status = Acpica.AcpiLoadTables();
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not load tables");
                return;
            }

            status = Acpica.AcpiEnableSubsystem(Acpica.ACPI_FULL_INITIALIZATION);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not enable subsystems:");
                return;
            }

            status = Acpica.AcpiInitializeObjects(Acpica.ACPI_FULL_INITIALIZATION);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not initialize objects");
                return;
            }

            Console.WriteLine("[ACPI] Initialized");

            Acpica.ACPI_TABLE_HEADER* table;
            status = Acpica.AcpiGetTable("APIC", 1, &table);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not find MADT table");
                return;
            }

            parseMADT((MADT*)table);
        }

        /// <summary>
        /// Power reset
        /// </summary>
        public static void Reboot()
        {
            Acpica.AcpiReset();
        }

        /// <summary>
        /// Power shutdown
        /// </summary>
        public static void Shutdown()
        {
            Acpica.AcpiEnterSleepStatePrep(5);
            CPU.CLI();
            Acpica.AcpiEnterSleepState(5);
            CPU.HLT();
        }
    }
}
