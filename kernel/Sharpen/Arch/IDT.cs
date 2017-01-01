using Sharpen.Utilities;
using System.Runtime.InteropServices;

namespace Sharpen.Arch
{
    public sealed class IDT
    {
        // One entry in the table
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IDT_Entry
        {
            public ushort AddressLow;   // Lower part of the address to jump to when int fires
            public ushort Selector;     // Code segment selector
            public byte   Zero;         // Always zero, unused
            public byte   Flags;        // Flags
            public ushort AddressHigh;  // Higher part of the address to jump to when int fires
        }

        // IDT pointer
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IDT_Pointer
        {
            public ushort Limit;
            public uint   BaseAddress;
        }

        // Types
        enum Type
        {
            TASK32 = 0x5,
            INT16 = 0x6,
            TRAP16 = 0x7,
            INT32 = 0xE,
            TRAP32 = 0xF
        }

        // Predefined flags
        public static readonly byte FLAG_ISR  = (byte)(Present(1) | Privilege(0) | (int)Type.INT32);
        public static readonly byte FLAG_IRQ  = (byte)(Present(1) | Privilege(0) | (int)Type.TRAP32);
        public static readonly byte FLAG_INT  = (byte)(Present(1) | Privilege(3) | (int)Type.INT32);
        public static readonly byte FLAG_TRAP = (byte)(Present(1) | Privilege(3) | (int)Type.TRAP32);

        private static IDT_Entry[] m_entries;
        private static IDT_Pointer m_ptr;

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

        /// <summary>
        /// Converts present
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static int Present(int a)
        {
            return (a << 0x07);
        }

        #endregion

        /// <summary>
        /// Sets an IDT entry in the table
        /// </summary>
        /// <param name="num">The entry number</param>
        /// <param name="address">The address that needs to be jumped to when this int fires</param>
        /// <param name="selector">The code segment selector</param>
        /// <param name="flags"> The flags</param>
        public static unsafe void SetEntry(int num, void* address, ushort selector, byte flags)
        {
            uint addr = (uint)address;

            m_entries[num].AddressLow = (ushort)(addr & 0xFFFF);
            m_entries[num].AddressHigh = (ushort)((addr >> 16) & 0xFFFF);
            m_entries[num].Selector = selector;
            m_entries[num].Zero = 0;
            m_entries[num].Flags = flags;
        }
        
        /// <summary>
        /// Initializes the IDT
        /// </summary>
        public static unsafe void Init()
        {
            #region Init

            // Allocate data
            m_entries = new IDT_Entry[256];
            m_ptr = new IDT_Pointer();

            // Set IDT table pointer
            m_ptr.Limit = (ushort)((256 * sizeof(IDT_Entry)) - 1);
            fixed (IDT_Entry* ptr = m_entries)
            {
                m_ptr.BaseAddress = (uint)ptr;
            }

            #endregion

            #region Defaults

            // Default handlers: ignore
            void* ignore = Util.MethodToPtr(INTIgnore);
            for(int i = 0; i < 256; i++)
            {
                SetEntry(i, ignore, 0x08, FLAG_ISR);
            }

            #endregion

            #region ISR

            SetEntry(0, Util.MethodToPtr(ISR0), 0x08, FLAG_ISR);
            SetEntry(1, Util.MethodToPtr(ISR1), 0x08, FLAG_ISR);
            SetEntry(2, Util.MethodToPtr(ISR2), 0x08, FLAG_ISR);
            SetEntry(3, Util.MethodToPtr(ISR3), 0x08, FLAG_ISR);
            SetEntry(4, Util.MethodToPtr(ISR4), 0x08, FLAG_ISR);
            SetEntry(5, Util.MethodToPtr(ISR5), 0x08, FLAG_ISR);
            SetEntry(6, Util.MethodToPtr(ISR6), 0x08, FLAG_ISR);
            SetEntry(7, Util.MethodToPtr(ISR7), 0x08, FLAG_ISR);
            SetEntry(8, Util.MethodToPtr(ISR8), 0x08, FLAG_ISR);
            SetEntry(9, Util.MethodToPtr(ISR9), 0x08, FLAG_ISR);
            SetEntry(10, Util.MethodToPtr(ISR10), 0x08, FLAG_ISR);
            SetEntry(11, Util.MethodToPtr(ISR11), 0x08, FLAG_ISR);
            SetEntry(12, Util.MethodToPtr(ISR12), 0x08, FLAG_ISR);
            SetEntry(13, Util.MethodToPtr(ISR13), 0x08, FLAG_ISR);
            SetEntry(14, Util.MethodToPtr(ISR14), 0x08, FLAG_ISR);
            SetEntry(15, Util.MethodToPtr(ISR15), 0x08, FLAG_ISR);
            SetEntry(16, Util.MethodToPtr(ISR16), 0x08, FLAG_ISR);
            SetEntry(17, Util.MethodToPtr(ISR17), 0x08, FLAG_ISR);
            SetEntry(18, Util.MethodToPtr(ISR18), 0x08, FLAG_ISR);
            SetEntry(19, Util.MethodToPtr(ISR19), 0x08, FLAG_ISR);
            SetEntry(20, Util.MethodToPtr(ISR20), 0x08, FLAG_ISR);
            SetEntry(21, Util.MethodToPtr(ISR21), 0x08, FLAG_ISR);
            SetEntry(22, Util.MethodToPtr(ISR22), 0x08, FLAG_ISR);
            SetEntry(23, Util.MethodToPtr(ISR23), 0x08, FLAG_ISR);
            SetEntry(24, Util.MethodToPtr(ISR24), 0x08, FLAG_ISR);
            SetEntry(25, Util.MethodToPtr(ISR25), 0x08, FLAG_ISR);
            SetEntry(26, Util.MethodToPtr(ISR26), 0x08, FLAG_ISR);
            SetEntry(27, Util.MethodToPtr(ISR27), 0x08, FLAG_ISR);
            SetEntry(28, Util.MethodToPtr(ISR28), 0x08, FLAG_ISR);
            SetEntry(29, Util.MethodToPtr(ISR29), 0x08, FLAG_ISR);
            SetEntry(30, Util.MethodToPtr(ISR30), 0x08, FLAG_ISR);
            SetEntry(31, Util.MethodToPtr(ISR31), 0x08, FLAG_ISR);

            #endregion

            #region IRQ
            
            SetEntry(IRQ.MASTER_OFFSET + 0, Util.MethodToPtr(IRQ0), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 1, Util.MethodToPtr(IRQ1), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 2, Util.MethodToPtr(IRQ2), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 3, Util.MethodToPtr(IRQ3), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 4, Util.MethodToPtr(IRQ4), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 5, Util.MethodToPtr(IRQ5), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 6, Util.MethodToPtr(IRQ6), 0x08, FLAG_IRQ);
            SetEntry(IRQ.MASTER_OFFSET + 7, Util.MethodToPtr(IRQ7), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 0, Util.MethodToPtr(IRQ8), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 1, Util.MethodToPtr(IRQ9), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 2, Util.MethodToPtr(IRQ10), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 3, Util.MethodToPtr(IRQ11), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 4, Util.MethodToPtr(IRQ12), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 5, Util.MethodToPtr(IRQ13), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 6, Util.MethodToPtr(IRQ14), 0x08, FLAG_IRQ);
            SetEntry(IRQ.SLAVE_OFFSET + 7, Util.MethodToPtr(IRQ15), 0x08, FLAG_IRQ);

            #endregion

            #region Syscall

            SetEntry(0x80, Util.MethodToPtr(Syscall), 0x08, FLAG_INT);

            #endregion

            #region Finish

            // Flush IDT
            fixed (IDT_Pointer* ptr = &m_ptr)
            {
                flushIDT(ptr);
            }

            #endregion
        }
        
        /// <summary>
        /// Flushes the IDT table
        /// </summary>
        /// <param name="ptr">The pointer to the table</param>
        private static extern unsafe void flushIDT(IDT_Pointer* ptr);

        #region ISR routines

        private static extern void INTIgnore();
        private static extern void ISR0();
        private static extern void ISR1();
        private static extern void ISR2();
        private static extern void ISR3();
        private static extern void ISR4();
        private static extern void ISR5();
        private static extern void ISR6();
        private static extern void ISR7();
        private static extern void ISR8();
        private static extern void ISR9();
        private static extern void ISR10();
        private static extern void ISR11();
        private static extern void ISR12();
        private static extern void ISR13();
        private static extern void ISR14();
        private static extern void ISR15();
        private static extern void ISR16();
        private static extern void ISR17();
        private static extern void ISR18();
        private static extern void ISR19();
        private static extern void ISR20();
        private static extern void ISR21();
        private static extern void ISR22();
        private static extern void ISR23();
        private static extern void ISR24();
        private static extern void ISR25();
        private static extern void ISR26();
        private static extern void ISR27();
        private static extern void ISR28();
        private static extern void ISR29();
        private static extern void ISR30();
        private static extern void ISR31();

        #endregion

        #region IRQ routines

        private static extern void IRQ0();
        private static extern void IRQ1();
        private static extern void IRQ2();
        private static extern void IRQ3();
        private static extern void IRQ4();
        private static extern void IRQ5();
        private static extern void IRQ6();
        private static extern void IRQ7();
        private static extern void IRQ8();
        private static extern void IRQ9();
        private static extern void IRQ10();
        private static extern void IRQ11();
        private static extern void IRQ12();
        private static extern void IRQ13();
        private static extern void IRQ14();
        private static extern void IRQ15();

        #endregion

        #region INT routines

        private static extern void Syscall();

        #endregion
    }
}
