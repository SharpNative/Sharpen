using Sharpen.Mem;
using System.Runtime.InteropServices;

namespace Sharpen.Arch
{
    class GDT
    {
        // GDT entry in table
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct GDT_Entry
        {
            public ushort LimitLow;
            public ushort BaseLow;
            public byte   BaseMid;
            public byte   Access;
            public byte   Granularity;
            public byte   BaseHigh;
        }

        // GDT pointer
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct GDT_Pointer
        {
            public ushort Limit;
            public uint   BaseAddress;
        }

        // TSS entry
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TSS
        {
            public uint PreviousTSS;     /* The previous TSS */
            public uint ESP0;             /* The stack pointer of RING0 */
            public uint SS0;              /* The Stack Segment of RING0 */
            public uint ESP1;             /* The stack pointer of RING1 */
            public uint SS1;              /* The Stack Segment of RING1 */
            public uint ESP2;             /* The stack pointer of RING2 */
            public uint SS2;              /* The Stack Segment of RING2 */
            public uint CR3;              /* CR3 prior to switching */
            public uint EIP;              /* EIP prior to switching */
            public uint EFlags;           /* EFlags prior to switching */
            public uint EAX;              /* EAX prior to switching */
            public uint ECX;              /* ECX prior to switching */
            public uint EDX;              /* EDX prior to switching */
            public uint EBX;              /* EBX prior to switching */
            public uint ESP;              /* ESP prior to switching */
            public uint EBP;              /* EBP prior to switching */
            public uint ESI;              /* ESI prior to switching */
            public uint EDI;              /* EDI prior to switching */
            public uint ES;               /* The ES to load */
            public uint CS;               /* The CS to load */
            public uint SS;               /* The SS to load */
            public uint DS;               /* The DS to load */
            public uint FS;               /* The FS to load */
            public uint GS;               /* The GS to load */
            public uint LDT;              /* LDT prior to switching */
            public ushort Trap;           /* Set if switch should cause a Debug Exception */
            public ushort IOMap;          /* Offset in structure to IOMAP */
        }

        // Data selector constants
        enum GDT_Data
        {
            R = 0x00,       // Read only
            RA = 0x01,      // Read only, accessed
            RW = 0x02,      // Read, write
            RWA = 0x03,     // Read, write, accessed
            RED = 0x04,     // Read only, expand down
            REDA = 0x05,    // Read only, expand down, accessed
            RWED = 0x06,    // Read, write, expand down
            RWEDA = 0x07,   // Read, write, expand down, accessed
            E = 0x08,       // Execute only
            EA = 0x09,      // Execute, accessed
            ER = 0x0A,      // Execute, read
            ERA = 0x0B,     // Execute, read, accessed
            EC = 0x0C,      // Execute, conforming
            ECA = 0x0D,     // Execute, conforming, accessed
            ERC = 0x0E,     // Execute, read, conforming
            ERCA = 0x0F     // Execute, read, conforming, accessed
        };

        private static GDT_Entry[] m_entries;
        private static GDT_Pointer m_ptr;

        public static unsafe TSS* TSS_Entry { get; private set; }

        #region Helpers
        
        /// <summary>
        /// Converts privilege level
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static int Privilege(int a)
        {
            return (a << 0x05);
        }

        #endregion
        
        enum GDTFlags
        {
            DescriptorCodeOrData = (1 << 0x4),
            Size32 = (1 << 0x6),
            Present = (1 << 0x7),
            Available = (1 << 0xC),
            Granularity = (1 << 0xF)
        }
        
        /// <summary>
        /// Sets a GDT entry in the table
        /// </summary>
        /// <param name="num">The entry number</param>
        /// <param name="base_address">The base address</param>
        /// <param name="limit">The limit</param>
        /// <param name="access">The access type</param>
        /// <param name="granularity">Granularity</param>
        private static void setEntry(int num, ulong base_address, ulong limit, int access, int granularity)
        {
            // Address
            m_entries[num].BaseLow = (ushort)(base_address & 0xFFFF);
            m_entries[num].BaseMid = (byte)((base_address >> 16) & 0xFF);
            m_entries[num].BaseHigh = (byte)((base_address >> 24) & 0xFF);

            // Limit
            m_entries[num].LimitLow = (ushort)(limit & 0xFFFF);
            m_entries[num].Granularity = (byte)((limit >> 16) & 0xFF);

            // Set access and granularity
            m_entries[num].Granularity |= (byte)(granularity & 0xF0);
            m_entries[num].Access = (byte)access;
        }

        /// <summary>
        /// Writes a TSS entry into the GDT
        /// </summary>
        /// <param name="num">The index of the corresponding GDT entry</param>
        /// <param name="tss">The TSS</param>
        private static unsafe void setTSS(int num, TSS* tss)
        {
            // Base and limit
            uint baseAddr = (uint)tss;
            uint limit = (uint)(baseAddr + sizeof(TSS));

            // Set TSS
            // Kernel Data Selector = 0x10, Kernel Code Selector = 0x08
            Memory.Memclear(tss, sizeof(TSS));
            tss->SS0 = 0x10;
            tss->IOMap = (ushort)sizeof(TSS);
            tss->CS = 0x08;
            tss->DS = 0x10;
            tss->ES = 0x10;
            tss->FS = 0x10;
            tss->GS = 0x10;
            tss->SS = 0x10;

            // Add TSS descriptor to GDT
            setEntry(num, baseAddr, limit, (int)GDT_Data.EA | Privilege(3) | (int)GDTFlags.Present, 0);
        }

        /// <summary>
        /// Initializes the GDT
        /// </summary>
        public static unsafe void Init()
        {
            // Allocate data
            m_entries = new GDT_Entry[6];
            m_ptr = new GDT_Pointer();

            // Set GDT table pointer
            m_ptr.Limit = (ushort)((6 * sizeof(GDT_Entry)) - 1);
            fixed (GDT_Entry* ptr = m_entries)
            {
                m_ptr.BaseAddress = (uint)ptr;
            }

            // NULL segment
            setEntry(0, 0, 0, 0, 0);

            // Kernel code segment
            setEntry(1, 0, 0xFFFFFFFF, (int)GDT_Data.ER | (int)GDTFlags.DescriptorCodeOrData | Privilege(0) | (int)GDTFlags.Present, (int)GDTFlags.Size32 | (int)GDTFlags.Granularity);

            // Kernel data segment
            setEntry(2, 0, 0xFFFFFFFF, (int)GDT_Data.RW | (int)GDTFlags.DescriptorCodeOrData | Privilege(0) | (int)GDTFlags.Present, (int)GDTFlags.Size32 | (int)GDTFlags.Granularity);

            // User code segment
            setEntry(3, 0, 0xFFFFFFFF, (int)GDT_Data.ER | (int)GDTFlags.DescriptorCodeOrData | Privilege(3) | (int)GDTFlags.Present, (int)GDTFlags.Size32 | (int)GDTFlags.Granularity);

            // User data segment
            setEntry(4, 0, 0xFFFFFFFF, (int)GDT_Data.RW | (int)GDTFlags.DescriptorCodeOrData | Privilege(3) | (int)GDTFlags.Present, (int)GDTFlags.Size32 | (int)GDTFlags.Granularity);

            // TSS
            TSS_Entry = (TSS*)Heap.Alloc(sizeof(TSS));
            setTSS(5, TSS_Entry);

            // Flush GDT
            fixed (GDT_Pointer* ptr = &m_ptr)
            {
                flushGDT(ptr);
            }

            // Flush TSS
            flushTSS();
        }

        /// <summary>
        /// Flushes the GDT table
        /// </summary>
        /// <param name="ptr">The pointer to the table</param>
        private static extern unsafe void flushGDT(GDT_Pointer* ptr);

        /// <summary>
        /// Flushes the TSS
        /// </summary>
        private static extern void flushTSS();
    }
}
