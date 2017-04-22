using System.Runtime.InteropServices;

namespace Sharpen.Arch
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct MPFloatingPoint
    {
        public fixed char Signature[4];
        public uint ConfigTable;
        public byte Length;
        public byte Version;
        public byte Checksum;
        public fixed byte FeatureBytes[5];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct MPConfigTable
    {
        public fixed char Signature[4];
        public ushort Length;
        public byte Revision;
        public byte Checksum;
        public fixed char OEMID[8];
        public fixed char ProductID[12];
        public uint OEMTablePointer;
        public ushort OEMTableSize;
        public ushort EntryCount;
        public uint LocalApicAddress;
        public ushort ExtendedTableLength;
        public byte ExtendedTableChecksum;
        public byte Reserved;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MPBusEntry
    {
        public byte EntryType;
        public byte BusID;
        public fixed char Type[6];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MPIOInterruptEntry
    {
        public byte EntryType;
        public byte InterruptType;
        public byte POandLE;
        public byte Reserved;
        public byte SourceBusID;
        public byte SourceBusIRQ;
        public byte DestinationApicID;
        public byte DestinationApicINTNO;
    }

    public enum MPInterruptType
    {
        INT,
        NMI,
        SMI,
        EXTINT
    }

    public enum MPEntryType
    {
        Processor,
        Bus,
        IOApic,
        IOInterruptAssignment,
        LocalInterruptAssignment
    }
}
