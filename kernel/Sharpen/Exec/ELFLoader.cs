using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Task;
using Sharpen.Utilities;

namespace Sharpen.Exec
{
    public sealed class ELFLoader
    {
        public enum Ident
        {
            EI_MAG0 = 0,        /* Index of magic number 1 */
            EI_MAG1 = 1,        /* Index of magic number 2 */
            EI_MAG2 = 2,        /* Index of magic number 3 */
            EI_MAG3 = 3,        /* Index of magic number 4 */
            EI_CLASS = 4,       /* Index of class */
            EI_DATA = 5,        /* Index of endianness */
            EI_VERSION = 6,     /* Index of version */
            EI_OSABI = 7,       /* Index of target OS */
            EI_ABIVER = 8,      /* Index of ABI version */
            EI_PAD = 9,         /* Index of padding start */
        }

        public enum ExecutableType
        {
            ET_NONE = 0,        /* Unknown */
            ET_REL = 1,         /* Relocatable */
            ET_EXEC = 2,        /* Executable */
            ET_SHARE = 3,       /* Shared */
            ET_CORE = 4,        /* Core */
        }

        public enum MachineType
        {
            EM_SPARC = 0x02,    /* SPARC */
            EM_X86 = 0x03,      /* x86 */
            EM_MIPS = 0x08,     /* MIPS */
            EM_PPC = 0x14,      /* PowerPC */
            EM_ARM = 0x28,      /* ARM */
            EM_SUPERH = 0x2A,   /* SuperH */
            EM_IA64 = 0x32,     /* IA-64 */
            EM_X86_64 = 0x3E,   /* x86-64 */
            EM_AARCH64 = 0xB7,  /* AArch64 */
        }

        public enum SectionHeaderType
        {
            SHT_NULL = 0,       /* NULL section */
            SHT_PROGBITS = 1,   /* Program code */
            SHT_SYMTAB = 2,     /* Symbol table */
            SHT_STRTAB = 3,     /* String table */
            SHT_RELA = 4,       /* Relocate */
            SHT_HASH = 5,       /* Hash */
            SHT_DYNAMIC = 6,    /* Dynamic */
            SHT_NOTE = 7,       /* Just a note */
            SHT_NOBITS = 8,     /* NO bits, set to zero (bss) */
            SHT_REL = 9,        /* Relocate */
            SHT_SHLIB = 10,     /* Shared library */
            SHT_DYNSYM = 11,    /* Dynamic symbol */
        }

        private unsafe struct ELF32
        {
            public fixed byte Ident[16];   /* Identifiers */
            public ushort Type;            /* Type of this ELF */
            public ushort Machine;         /* Instruction set */
            public uint Version;           /* ELF format version */
            public uint Entry;             /* Points to entry point */
            public uint PhOff;             /* Points to start of program header table */
            public uint ShOff;             /* Points to start of section header table */
            public uint Flags;             /* Depends on architecture */
            public ushort EhSize;          /* Size of this header */
            public ushort PhEntSize;       /* Size of program header table */
            public ushort PhNum;           /* Number of entries in program header table */
            public ushort ShEntSize;       /* Size of section header table entry */
            public ushort ShNum;           /* Number of section headers */
            public ushort ShnStrNdx;       /* Index of section header of string table */
        }

        private struct ProgramHeader
        {
            public ushort Type;
            public uint Offset;
            public uint VirtAddress;
            public uint PhysAddress;
            public ushort FileSize;
            public ushort MemSize;
            public ushort Flags;
            public ushort Alignment;
        }

        private struct SectionHeader
        {
            public uint Name;
            public SectionHeaderType Type;
            public uint Flags;
            public uint Address;
            public uint Offset;
            public uint Size;
            public uint Link;
            public uint Info;
            public uint AddressAlignment;
            public uint EntrySize;
        }

        /// <summary>
        /// Checks if the ELF file is valid
        /// </summary>
        /// <param name="elf">The pointer to the ELF</param>
        /// <returns>If it's valid</returns>
        private static unsafe bool isValidELF(ELF32* elf)
        {
            if (elf->Ident[(int)Ident.EI_MAG0] != 0x7F || elf->Ident[(int)Ident.EI_MAG1] != 'E' || elf->Ident[(int)Ident.EI_MAG2] != 'L' || elf->Ident[(int)Ident.EI_MAG3] != 'F')
                return false;

            if (elf->Type != (int)ExecutableType.ET_EXEC)
                return false;

            if (elf->Version != 1)
                return false;

            if (elf->Machine != (int)MachineType.EM_X86)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a section header
        /// </summary>
        /// <param name="elf">The ELF</param>
        /// <param name="index">The index of the section</param>
        /// <returns>The pointer to the section header</returns>
        private static unsafe SectionHeader* getSection(ELF32* elf, uint index)
        {
            return (SectionHeader*)((int)elf + elf->ShOff + (index * elf->ShEntSize));
        }

        /// <summary>
        /// Gets a string from the string table
        /// </summary>
        /// <param name="elf">The ELF</param>
        /// <param name="offset">The offset in the string table</param>
        /// <returns>The string</returns>
        private static unsafe string getString(ELF32* elf, uint offset)
        {
            SectionHeader* section = getSection(elf, elf->ShnStrNdx);
            uint strtab = (uint)elf + section->Offset;
            return Util.CharPtrToString((char*)(strtab + offset));
        }

        /// <summary>
        /// Executes an ELF file
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The size of the ELF</param>
        /// <param name="argv">The arguments</param>
        /// <returns>The error code</returns>
        public static unsafe ErrorCode Execute(byte[] buffer, uint size, string[] argv)
        {
            ELF32* elf;
            fixed (byte* ptr = buffer)
            {
                elf = (ELF32*)ptr;
            }

            if (!isValidELF(elf))
                return ErrorCode.EINVAL;

            // Get program header
            ProgramHeader* programHeader = (ProgramHeader*)((int)elf + elf->PhOff);
            uint virtAddress = programHeader->VirtAddress;
            void* allocated = Heap.AlignedAlloc(0x1000, (int)size);

            // Loop through every section
            for (uint i = 0; i < elf->ShNum; i++)
            {
                SectionHeader* section = getSection(elf, i);

                // Only loadable sections
                if (section->Address == 0)
                    continue;

                uint offset = section->Address - virtAddress;

                // BSS
                if (section->Type == SectionHeaderType.SHT_NOBITS)
                {
                    Memory.Memset((void*)((uint)allocated + offset), 0, (int)section->Size);
                }
                // Copy
                else
                {

                    Memory.Memcpy((void*)((uint)allocated + offset), (void*)((uint)elf + section->Offset), (int)section->Size);
                }
            }

            // Count arguments
            int argc = 0;
            if (argv != null)
            {
                while (argv[argc] != null)
                    argc++;
            }

            int[] initialStack = new int[2];
            initialStack[0] = (int)Util.ObjectToVoidPtr(argv);
            initialStack[1] = argc;

            Task.Task newTask = Tasking.AddTask((void*)elf->Entry, TaskPriority.NORMAL, initialStack, 2/*, newDirectory*/);

            Paging.PageDirectory* newDirectory = Paging.CloneDirectory(Paging.CurrentDirectory);

            // Map memory
            for (uint j = 0; j < size; j += 0x1000)
            {
                Paging.MapPage(newDirectory, (int)Paging.GetPhysicalFromVirtual((void*)((uint)allocated + j)), (int)(virtAddress + j), (int)Paging.PageFlags.Present | (int)Paging.PageFlags.Writable | (int)Paging.PageFlags.UserMode);
            }
            
            
            
            // Add task
            
            
            
            newTask.PageDir = newDirectory;
            Tasking.ScheduleTask(newTask);
            return ErrorCode.SUCCESS;
        }
    }
}
