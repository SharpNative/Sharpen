namespace Sharpen.Arch
{
    public sealed class IRQ
    {
        // Offset
        public const byte MASTER_OFFSET = 32;
        public const byte SLAVE_OFFSET = 40;

        // Delegate
        public unsafe delegate Regs* IRQHandler(Regs* regsPtr);

        // Handlers
        private static IRQHandler[] handlers;

        /// <summary>
        /// Initializes interrupts requests
        /// </summary>
        public static void Init()
        {
            handlers = new IRQHandler[256 - MASTER_OFFSET];
        }

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
        /// Removes a  handler
        /// </summary>
        /// <param name="num">The interrupt request number</param>
        public static void RemoveHandler(int num)
        {
            handlers[num] = null;
        }

        /// <summary>
        /// IRQ handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe Regs* Handler(Regs* regsPtr)
        {
            int irqNum = regsPtr->IntNum - MASTER_OFFSET;

            Regs* ret = regsPtr;
            handlers[irqNum]?.Invoke(regsPtr);

            LocalApic.SendEOI();
            return ret;
        }
    }
}
