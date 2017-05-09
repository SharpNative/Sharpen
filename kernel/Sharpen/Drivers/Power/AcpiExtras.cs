using Sharpen.Power;
using System.Runtime.InteropServices;

namespace Sharpen.Drivers.Power
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MADT
    {
        public Acpica.TableHeader Header;
        public uint LocalControllerAddress;
        public uint Flags;
        // After the flags field, the rest of the table contains a variable length of records
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ApicEntryHeader
    {
        public byte Type;
        public byte Length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ApicLocalApic
    {
        public byte ProcessorID;
        public byte APICID;
        public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ApicIOApic
    {
        public byte IOAPIC_ID;
        public byte Reserved;
        public uint IOAPICAddress;
        public uint GlobalSystemInterruptBase;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ApicInterruptSourceOverride
    {
        public byte BusSource;
        public byte IRQSource;
        public uint GlobalSystemInterrupt;
        public ushort Flags;
    }

    enum ApicEntryHeaderType
    {
        LOCAL_APIC,
        IO_APIC,
        INTERRUPT_SOURCE_OVERRIDE,
        NMI,
        LOCAL_APIC_NMI,
        LOCAL_APIC_ADDRESS_OVERRIDE,
        IO_SAPIC,
        LOCAL_SAPIC,
        PLATFORM_INTERRUPT_SOURCES,
        PROCESSOR_LOCAL_2XAPIC,
        LOCAL_X2APIC_NMI,
        GIC,
        GICD
    }
}
