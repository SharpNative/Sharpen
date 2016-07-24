namespace Sharpen.Arch
{
    public sealed class PIC
    {
        // Command port of master PIC
        public static readonly ushort MASTER_PIC_CMD = 0x20;

        // Data port of master PIC
        public static readonly ushort MASTER_PIC_DATA = 0x21;

        // Command port of slave PIC
        public static readonly ushort SLAVE_PIC_CMD = 0xA0;

        // Data port of slave PIC
        public static readonly ushort SLAVE_PIC_DATA = 0xA1;

        // Initialize command
        public static readonly byte PIC_INIT = 0x11;

        // End Of Interrupt
        public static readonly byte PIC_EOI = 0x20;

        // 8086 / 8088 mode
        public static readonly byte PIC_8086 = 0x01;

        // Single / cascade mode
        public static readonly byte PIC_CASCADE = 0x04;

        /// <summary>
        /// Sends an EOI signal
        /// </summary>
        /// <param name="irq">The IRQ number</param>
        public static void SendEOI(byte irq)
        {
            // Slave EOI if irq belongs to slave
            if(irq >= 8)
                PortIO.Out8(SLAVE_PIC_CMD, PIC_EOI);

            // EOI to master
            PortIO.Out8(MASTER_PIC_CMD, PIC_EOI);
        }

        /// <summary>
        /// Remaps the IRQs
        /// </summary>
        public static void Remap()
        {
            // Save masks
            byte masterMask = PortIO.In8(MASTER_PIC_DATA);
            byte slaveMask = PortIO.In8(SLAVE_PIC_DATA);

            // Initialize
            PortIO.Out8(MASTER_PIC_CMD, PIC_INIT);
            PortIO.Out8(SLAVE_PIC_CMD, PIC_INIT);

            // Offsets
            PortIO.Out8(MASTER_PIC_DATA, IRQ.MASTER_OFFSET);
            PortIO.Out8(SLAVE_PIC_DATA, IRQ.SLAVE_OFFSET);

            // Tell master there's a slave PIC at IRQ2
            PortIO.Out8(MASTER_PIC_DATA, 4);

            // Tell slave its cascade identity
            PortIO.Out8(SLAVE_PIC_DATA, PIC_CASCADE);

            // 8086 mode
            PortIO.Out8(MASTER_PIC_DATA, PIC_8086);
            PortIO.Out8(SLAVE_PIC_DATA, PIC_8086);

            // Restore saved masks
            PortIO.Out8(MASTER_PIC_DATA, masterMask);
            PortIO.Out8(SLAVE_PIC_DATA, slaveMask);
        }
    }
}
