using Sharpen.Collections;

namespace Sharpen.Arch
{
    class IOApicManager
    {
        private static List m_IOApics;

        /// <summary>
        /// Initializes the IO Apic Manager
        /// </summary>
        public static void Init()
        {
            m_IOApics = new List();
        }

        /// <summary>
        /// Adds a new IO Apic
        /// </summary>
        /// <param name="IOApic">The IO Apic</param>
        public static void Add(IOApic IOApic)
        {
            m_IOApics.Add(IOApic);
        }

        /// <summary>
        /// Initializes the found IO Apics
        /// </summary>
        public static void InitIOApics()
        {
            for (int i = 0; i < m_IOApics.Count; i++)
            {
                IOApic IOApic = (IOApic)m_IOApics.Item[i];
                IOApic.Init();
            }
        }

        /// <summary>
        /// Get the IO Apic responsible for the interrupt source
        /// </summary>
        /// <param name="src">Interrupt source</param>
        /// <returns>The IO Apic</returns>
        public static IOApic GetIOApicFor(uint src)
        {
            for (int i = 0; i < m_IOApics.Count; i++)
            {
                IOApic IOApic = (IOApic)m_IOApics.Item[i];
                if (src >= IOApic.GlobalSystemInterruptBase && src < IOApic.GlobalSystemInterruptBase + IOApic.RedirectionCount)
                {
                    return IOApic;
                }
            }
            return null;
        }

        public static void blar(uint src, uint dst)
        {
            IOApic IOApic = (IOApic)m_IOApics.Item[0];
            IOApic.blar(src, dst);
        }

        /// <summary>
        /// Creates an ISA IRQ redirection entry
        /// </summary>
        /// <param name="src">The source interrupt</param>
        /// <param name="dst">The destination interrupt</param>
        public static void CreateISARedirection(uint src, uint dst)
        {
            IOApic IOApic = GetIOApicFor(src);
            if (IOApic == null)
            {
                Console.Write("[IOAPIC] Could not setup ISA redirection: ");
                Console.WriteNum((int)src);
                Console.Write("->");
                Console.WriteNum((int)dst);
                Console.Write('\n');
                return;
            }

            IOApic.CreateISARedirection(src - IOApic.GlobalSystemInterruptBase, dst);
        }
    }
}
