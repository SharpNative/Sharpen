namespace Sharpen.Arch
{
    public sealed class IRQ
    {
        // Offset
        public const byte MASTER_OFFSET = 32;
        public const byte SLAVE_OFFSET = 40;

        // Delegate
        public unsafe delegate bool IRQHandler();

        // Handlers
        private static IRQHandler[][] handlers;

        public const int IRQ_MAX_SHARING = 3;

        /// <summary>
        /// Initializes interrupts requests
        /// </summary>
        public static void Init()
        {
            handlers = new IRQHandler[256 - MASTER_OFFSET][];
        }

        /// <summary>
        /// Sets an IRQ handler
        /// </summary>
        /// <param name="num">The IRQ number</param>
        /// <param name="handler">The handler</param>
        public static void SetHandler(uint num, IRQHandler handler)
        {
            // No entry yet?
            handlers[num] = new IRQHandler[3];

            // Add entry to handler
            bool found = false;
            for (int i = 0; i < IRQ_MAX_SHARING; i++)
            {
                if (handlers[num][i] == null)
                {
                    handlers[num][i] = handler;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.Write("[IRQ] Couldn't set a handler for IRQ ");
                Console.WriteNum((int)num);
                Console.WriteLine(" because the maximum amount of sharing has been reached for this IRQ number");
            }
        }

        /// <summary>
        /// Returns if an IRQ has a handler
        /// </summary>
        /// <param name="num">The IRQ number</param>
        /// <returns>If this IRQ has a handler</returns>
        public static bool HasHandler(uint num)
        {
            return (handlers[num] != null);
        }

        /// <summary>
        /// Removes a  handler
        /// </summary>
        /// <param name="num">The IRQ number</param>
        public static void RemoveHandler(uint num)
        {
            handlers[num] = null;
        }

        /// <summary>
        /// IRQ handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe Regs* Handler(Regs* regsPtr)
        {
            int irqNum = regsPtr->IntNum;

            if (handlers[irqNum] != null)
            {
                // Loop through handlers to see who has sent interrupt
                for (int i = 0; i < IRQ_MAX_SHARING; i++)
                {
                    if (handlers[irqNum][i] == null || handlers[irqNum][i]())
                    {
                        break;
                    }
                }
            }

            LocalApic.SendEOI();
            return regsPtr;
        }
    }
}
