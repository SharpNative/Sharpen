using Sharpen.Arch;
using Sharpen.Power;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Power
{
    public sealed unsafe class Acpi
    {
        public struct ISAOverride
        {
            public uint GSI;
            public uint Polarity;
            public uint Trigger;
        }

        private static MADT* m_madt;
        private static ISAOverride[] m_intSourceOverrides;

        /// <summary>
        /// Parses the MADT
        /// </summary>
        /// <param name="madt">The MADT</param>
        private static void parseMADT(MADT* madt)
        {
            m_madt = madt;
            LocalApic.SetLocalControllerAddress(madt->LocalControllerAddress);

            m_intSourceOverrides = new ISAOverride[16];
            for (uint i = 0; i < 16; i++)
            {
                m_intSourceOverrides[i].GSI = i;
                m_intSourceOverrides[i].Polarity = IOApic.IOAPIC_REDIR_POLARITY_HIGH;
                m_intSourceOverrides[i].Trigger = IOApic.IOAPIC_REDIR_TRIGGER_EDGE;
            }

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

                        m_intSourceOverrides[intSourceOverride->IRQSource].GSI = intSourceOverride->GlobalSystemInterrupt;
                        m_intSourceOverrides[intSourceOverride->IRQSource].Polarity = (uint)(intSourceOverride->Flags & 0x3);
                        m_intSourceOverrides[intSourceOverride->IRQSource].Trigger = (uint)((intSourceOverride->Flags >> 2) & 0x3);
                        
                        break;
                }

                current += header->Length;
            }
        }

        /// <summary>
        /// Translate ISA IRQ to a redirection entry
        /// </summary>
        /// <param name="irq">The ISA IRQ number</param>
        /// <returns>The override</returns>
        public static ISAOverride GetISARedirection(uint irq)
        {
            if (irq >= 16)
                Panic.DoPanic("Requested an ISA redirection for IRQ >= 16");

            return m_intSourceOverrides[irq];
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
                Panic.DoPanic("[ACPI] Couldn't initialize ACPICA subsystem");
            }

            status = Acpica.AcpiInitializeTables(null, 16, false);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't initialize tables");
            }

            status = Acpica.AcpiInstallAddressSpaceHandler((void*)Acpica.ACPI_ROOT_OBJECT, Acpica.ACPI_ADR_SPACE_SYSTEM_MEMORY, (void*)Acpica.ACPI_DEFAULT_HANDLER, null, null);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not install address space handler for system memory");
            }

            status = Acpica.AcpiInstallAddressSpaceHandler((void*)Acpica.ACPI_ROOT_OBJECT, Acpica.ACPI_ADR_SPACE_SYSTEM_IO, (void*)Acpica.ACPI_DEFAULT_HANDLER, null, null);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not install address space handler for system IO");
            }

            status = Acpica.AcpiInstallAddressSpaceHandler((void*)Acpica.ACPI_ROOT_OBJECT, Acpica.ACPI_ADR_SPACE_PCI_CONFIG, (void*)Acpica.ACPI_DEFAULT_HANDLER, null, null);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Could not install address space handler for PCI");
            }

            status = Acpica.AcpiLoadTables();
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't load tables");
            }

            status = Acpica.AcpiEnableSubsystem(Acpica.ACPI_FULL_INITIALIZATION);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't enable subsystems:");
            }

            status = Acpica.AcpiInitializeObjects(Acpica.ACPI_FULL_INITIALIZATION);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't initialize objects");
            }

            // MADT table contains info about APIC, processors, ...
            Acpica.TableHeader* table;
            status = Acpica.AcpiGetTable("APIC", 1, &table);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't find MADT table");
            }

            parseMADT((MADT*)table);

            // Set ACPI to APIC using the "_PIC" method, note that this method is not always present
            // so we "can" ignore the AE_NOT_FOUND error
            AcpiObjects.IntegerObject arg1;
            arg1.Type = AcpiObjects.ObjectType.Integer;
            arg1.Value = 1;
            AcpiObjects.ObjectList args;
            args.Count = 1;
            args.Pointer = &arg1;
            status = Acpica.AcpiEvaluateObject((void*)Acpica.ACPI_ROOT_OBJECT, "_PIC", &args, null);
            if (status != Acpica.AE_NOT_FOUND && status != Acpica.AE_OK)
            {
                Panic.DoPanic("[ACPI] Couldn't call _PIC method");
            }

            // We are done here
            Console.WriteLine("[ACPI] Initialized");
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
