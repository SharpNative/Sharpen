using Sharpen.Utilities;

namespace Sharpen.Arch
{
    unsafe class IOApic
    {
        // Registers
        public const int IOAPIC_ID = 0x0;
        public const int IOAPIC_VERSION = 0x1;
        public const int IOAPIC_ARB = 0x2;
        public const int IOAPIC_REDIR = 0x10;

        public const int IOAPIC_REDIR_DELIVERY_FIXED = (0 << 8);
        public const int IOAPIC_REDIR_DELIVERY_LOWEST = (1 << 8);
        public const int IOAPIC_REDIR_DELIVERY_SMI = (2 << 8);
        public const int IOAPIC_REDIR_DELIVERY_NMI = (4 << 8);
        public const int IOAPIC_REDIR_DELIVERY_INIT = (5 << 8);
        public const int IOAPIC_REDIR_DELIVERY_EXTINT = (7 << 8);

        public const int IOAPIC_REDIR_DESTMODE_PHYS = (0 << 10);
        public const int IOAPIC_REDIR_DESTMODE_LOGIC = (1 << 10);

        public const int IOAPIC_REDIR_POLARITY_HIGH = (0 << 12);
        public const int IOAPIC_REDIR_POLARITY_LOW = (1 << 12);

        public const int IOAPIC_REDIR_TRIGGER_EDGE = (0 << 14);
        public const int IOAPIC_REDIR_TRIGGER_LEVEL = (1 << 14);

        public const int IOAPIC_REDIR_INT_UNMASKED = (0 << 15);
        public const int IOAPIC_REDIR_INT_MASKED = (1 << 15);

        // Memory mapped offsets
        public const int IOAPIC_REGSEL = 0x0;
        public const int IOAPIC_REGWIN = 0x10;


        private void* m_address;

        public int Id { get; private set; }
        public uint RedirectionCount { get; private set; }
        public uint GlobalSystemInterruptBase { get; private set; }

        /// <summary>
        /// Creates a new IO Apic
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="address">The physical address</param>
        /// <param name="globalSystemInterruptBase">The GSI base</param>
        public IOApic(int id, void* address, uint globalSystemInterruptBase)
        {
            Id = id;
            m_address = Paging.MapToVirtual(Paging.CurrentDirectory, (int)address, 0x2000, Paging.PageFlags.Present | Paging.PageFlags.Writable/* | Paging.PageFlags.NoCache*/);
            GlobalSystemInterruptBase = globalSystemInterruptBase;
        }

        /// <summary>
        /// Initializes the IO Apic
        /// </summary>
        public void Init()
        {
            uint versionReg = Read(IOAPIC_VERSION);
            RedirectionCount = ((versionReg >> 16) & 0xFF) + 1;

            // Incorrect id???
            int readId = (int)(Read(IOAPIC_ID) >> 24) & 0x0F;
            if (readId != Id)
            {
                Id = readId;
            }

            Console.Write("[IOAPIC] IO Apic with ID ");
            Console.WriteNum(Id);
            Console.Write(" has ");
            Console.WriteNum((int)RedirectionCount);
            Console.WriteLine(" redirection entries");
        }

        /// <summary>
        /// Reads a value from a register
        /// </summary>
        /// <param name="reg">The register</param>
        /// <returns>The value</returns>
        public uint Read(uint reg)
        {
            Util.WriteVolatile32((uint)m_address + IOAPIC_REGSEL, reg);
            return Util.ReadVolatile32((uint)m_address + IOAPIC_REGWIN);
        }

        /// <summary>
        /// Writes a value to a register
        /// </summary>
        /// <param name="reg">The register</param>
        /// <param name="value">The value</param>
        public void Write(uint reg, uint value)
        {
            Util.WriteVolatile32((uint)m_address + IOAPIC_REGSEL, reg);
            Util.WriteVolatile32((uint)m_address + IOAPIC_REGWIN, value);
        }

        /// <summary>
        /// Create a redirection entry for an ISA IRQ
        /// </summary>
        /// <param name="pin">The source interrupt</param>
        /// <param name="interrupt">The destination interrupt</param>
        /// <param name="flags">The flags</param>
        public void CreateRedirection(uint pin, uint interrupt, ulong flags)
        {
            ulong entry = flags;
            entry |= IOAPIC_REDIR_DELIVERY_FIXED;
            entry |= IOAPIC_REDIR_DESTMODE_PHYS;
            entry |= IOAPIC_REDIR_INT_UNMASKED;
            entry |= ((ulong)(Id & 0xFF) << 56) | IOAPIC_REDIR_DESTMODE_PHYS;
            entry |= interrupt & 0xFF;

            Write(IOAPIC_REDIR + ((pin * 2) + 0), (uint)(entry & 0xFFFFFFFF));
            Write(IOAPIC_REDIR + ((pin * 2) + 1), (uint)(entry >> 32));
        }
    }
}
