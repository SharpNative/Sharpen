namespace Sharpen.Drivers.Power
{
    public unsafe class AcpiObjects
    {
        // Object types
        public enum ObjectType
        {
            Any, // Note: this type is used to indicate a NULL package element or an unresolved reference
            Integer,
            String,
            Buffer,
            Package,
            FieldUnit,
            Device,
            Event,
            Method,
            Mutex,
            Region,
            Power,
            Processor,
            Thermal,
            BufferField,
            DDBHandle,
            DebugObject
        }

        // Resource types
        public enum ResourceType
        {
            IRQ,
            DMA,
            START_DEPENDENT,
            END_DEPENDENT,
            IO,
            FIXED_IO,
            VENDOR,
            END_TAG,
            MEMORY24,
            MEMORY32,
            FIXED_MEMORY32,
            ADDRESS16,
            ADDRESS32,
            ADDRESS64,
            EXTENDED_ADDRESS64,
            EXTENDED_IRQ,
            GENERIC_REGISTER,
            GPIO,
            FIXED_DMA,
            SERIAL_BUS
        }

        // Object structs
        public struct IntegerObject
        {
            public ObjectType Type;
            public ulong Value;
        }

        public struct StringObject
        {
            public ObjectType Type;
            public uint Length;
            public char* Pointer;
        }

        public struct BufferObject
        {
            public ObjectType Type;
            public uint Length;
            public byte* Pointer;
        }

        public struct PackageObject
        {
            public ObjectType Type;
            public uint Count;
            public void* Elements;
        }

        public struct ReferenceObject
        {
            public ObjectType Type;
            public ObjectType ActualType;
            public void* Handle;
        }

        public struct ObjectList
        {
            public uint Count;
            public void* Pointer;
        }

        public struct PNPDeviceID
        {
            public uint Length; // Length of string + null
            public char* String;
        }

        public struct PNPDeviceIDList
        {
            public uint Count;
            public uint ListSize;
            // Followed by array of PNPDeviceID
            //public PNPDeviceID[] IDs;
        }

        public struct DeviceInfo
        {
            public uint InfoSize;
            public uint Name;
            public ObjectType Type;
            public byte ParamCount;
            public ushort Valid;
            public byte Flags;
            public fixed byte HighestDstates[4];
            public fixed byte LowestDstates[5];
            public uint CurrentStatus;
            public ulong Address;
            public PNPDeviceID HardwareId;
            public PNPDeviceID UniqueId;
            public PNPDeviceID ClassCode;
            public PNPDeviceIDList CompatibleIDList;
        }

        public struct Buffer
        {
            public uint Length;
            public void* Pointer;
        }

        public struct ResourceSource
        {
            public byte Index;
            public ushort StringLength;
            public char* StringPtr;
        }

        public struct ResourceIRQ
        {
            public byte DescriptorLength;
            public byte Trigger;
            public byte Polarity;
            public byte Sharable;
            public byte WakeCapable;
            public byte InterruptCount;
            // Followed by an array of interrupts
            //public byte[] Interrupts;
        }

        public struct ResourceExtendedIRQ
        {
            public byte ProducerConsumer;
            public byte Trigger;
            public byte Polarity;
            public byte Sharable;
            public byte WakeCapable;
            public byte InterruptCount;
            public ResourceSource ResourceSource;
            // Followed by an array of interrupts
            //public uint[] Interrupts;
        }

        public struct Resource
        {
            public ResourceType Type;
            public uint Length;
            // Followed by the actual resource data (depends on Type)
        }
    }
}
