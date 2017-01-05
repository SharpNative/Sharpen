namespace Sharpen.Arch
{
    public sealed class IRQ
    {
        // IRQ master offset (32 - 39)
        public const byte MASTER_OFFSET = 32;

        // IRQ slave offset (40 - 47)
        public const byte SLAVE_OFFSET = 40;

        // Delegate of IRQ Handler
        public unsafe delegate void IRQHandler(Regs* regsPtr);
        
        // IRQ handlers
        private static IRQHandler[] handlers = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

        /// <summary>
        /// Sets an IRQ handler
        /// </summary>
        /// <param name="num">The IRQ number</param>
        /// <param name="handler">The handler</param>
        public static void SetHandler(int num, IRQHandler handler)
        {
            handlers[num] = handler;
        }

        /// <summary>
        /// Removes an IRQ handler
        /// </summary>
        /// <param name="num">The IRQ number</param>
        public static void RemoveHandler(int num)
        {
            handlers[num] = null;
        }

        /// <summary>
        /// IRQ handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(Regs* regsPtr)
        {
            int irqNum = regsPtr->IntNum - MASTER_OFFSET;
            handlers[irqNum]?.Invoke(regsPtr);
            PIC.SendEOI((byte)irqNum);
        }
    }
}
