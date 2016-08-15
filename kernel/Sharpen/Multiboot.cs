namespace Sharpen
{
    public sealed class Multiboot
    {
        // Magic value used in the multiboot header
        public const int HeaderMagic = 0x1BADB002;

        // Magic value that needs to be checked against
        public const int Magic = 0x2BADB002;

        // Memory information provided
        public const int FlagMem = 0x001;

        // Device information provided
        public const int FlagDevice = 0x002;

        // Commandline provided
        public const int FlagCMDLine = 0x004;

        // Modules provided
        public const int FlagMods = 0x008;

        // If AOUT
        public const int FlagAOUT = 0x010;

        // If ELF
        public const int FlagELF = 0x020;

        // MMAP information provided
        public const int FlagMMAP = 0x040;

        // Config table provided
        public const int FlagConfig = 0x080;

        // Load information
        public const int FlagLoader = 0x100;

        // APM table provided
        public const int FlagAPM = 0x200;

        // VBE information provided
        public const int FlagVBE = 0x400;

        // Multiboot header structure
        public unsafe struct Header
        {
            public uint  Flags;                 // Flags
            public uint  MemLow;                // The amount of lower memory
            public uint  MemHi;                 // The amount of higher memory
            public uint  BootDevice;            // Device that booted us
            public void* CMDLine;               // Kernel command line
            public uint  ModsCount;             // How many modules were loaded
            public void* ModsAddr;              // Module info addresses
            public uint  Num;                   // ELF: Number of symbols
            public uint  Size;                  // ELF: size
            public uint  Addr;                  // ELF: address
            public uint  Shndx;                 // ELF: index of string table
            public uint  MMAPLen;               // Length of the memory map
            public void* MMAPAddr;              // Address of the memory map
            public uint  DrivesLen;             // Drive info buffer length
            public void* DriverAddr;            // Address of the drives info
            public void* ConfigTable;           // ROM configuration table
            public void* BootloaderName;        // The name of the bootloader
            public void* ApmTable;              // APM table
            public void* VbeCTRLInfo;           // VBE control info
            public void* VbeModeInfo;           // Information about the current VBE mode
            public uint  VbeMode;               // Current VBE mode number
            public void* VbeInterfaceSeg;       // Used for protected mode interface
            public void* VbeInterfaceOff;       // Used for protected mode interface
            public uint  VbeInterfaceLen;       // Used for protected mode interface
        }

        // MMAP structure
        public struct MMAP
        {
            public uint  Size;                  // The size
            public ulong Addr;                  // The address
            public ulong Length;                // The length
            public uint  Type;                  // The type
        }

        // Module structure
        public unsafe struct Module
        {
            public void* Start;                 // The start address of the module
            public void* End;                   // The end address of the module
            public void* CMDLine;               // The command line of the module
            public uint  Padding;               // Padding so the structure is 16 bytes, this is set to zero
        }
    }
}
