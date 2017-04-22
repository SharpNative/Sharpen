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

            #region Requests

            SetEntry(32, Util.MethodToPtr(Request0), 0x08, FLAG_IRQ);
            SetEntry(33, Util.MethodToPtr(Request1), 0x08, FLAG_IRQ);
            SetEntry(34, Util.MethodToPtr(Request2), 0x08, FLAG_IRQ);
            SetEntry(35, Util.MethodToPtr(Request3), 0x08, FLAG_IRQ);
            SetEntry(36, Util.MethodToPtr(Request4), 0x08, FLAG_IRQ);
            SetEntry(37, Util.MethodToPtr(Request5), 0x08, FLAG_IRQ);
            SetEntry(38, Util.MethodToPtr(Request6), 0x08, FLAG_IRQ);
            SetEntry(39, Util.MethodToPtr(Request7), 0x08, FLAG_IRQ);
            SetEntry(40, Util.MethodToPtr(Request8), 0x08, FLAG_IRQ);
            SetEntry(41, Util.MethodToPtr(Request9), 0x08, FLAG_IRQ);
            SetEntry(42, Util.MethodToPtr(Request10), 0x08, FLAG_IRQ);
            SetEntry(43, Util.MethodToPtr(Request11), 0x08, FLAG_IRQ);
            SetEntry(44, Util.MethodToPtr(Request12), 0x08, FLAG_IRQ);
            SetEntry(45, Util.MethodToPtr(Request13), 0x08, FLAG_IRQ);
            SetEntry(46, Util.MethodToPtr(Request14), 0x08, FLAG_IRQ);
            SetEntry(47, Util.MethodToPtr(Request15), 0x08, FLAG_IRQ);
            SetEntry(48, Util.MethodToPtr(Request16), 0x08, FLAG_IRQ);
            SetEntry(49, Util.MethodToPtr(Request17), 0x08, FLAG_IRQ);
            SetEntry(50, Util.MethodToPtr(Request18), 0x08, FLAG_IRQ);
            SetEntry(51, Util.MethodToPtr(Request19), 0x08, FLAG_IRQ);
            SetEntry(52, Util.MethodToPtr(Request20), 0x08, FLAG_IRQ);
            SetEntry(53, Util.MethodToPtr(Request21), 0x08, FLAG_IRQ);
            SetEntry(54, Util.MethodToPtr(Request22), 0x08, FLAG_IRQ);
            SetEntry(55, Util.MethodToPtr(Request23), 0x08, FLAG_IRQ);
            SetEntry(56, Util.MethodToPtr(Request24), 0x08, FLAG_IRQ);
            SetEntry(57, Util.MethodToPtr(Request25), 0x08, FLAG_IRQ);
            SetEntry(58, Util.MethodToPtr(Request26), 0x08, FLAG_IRQ);
            SetEntry(59, Util.MethodToPtr(Request27), 0x08, FLAG_IRQ);
            SetEntry(60, Util.MethodToPtr(Request28), 0x08, FLAG_IRQ);
            SetEntry(61, Util.MethodToPtr(Request29), 0x08, FLAG_IRQ);
            SetEntry(62, Util.MethodToPtr(Request30), 0x08, FLAG_IRQ);
            SetEntry(63, Util.MethodToPtr(Request31), 0x08, FLAG_IRQ);
            SetEntry(64, Util.MethodToPtr(Request32), 0x08, FLAG_IRQ);
            SetEntry(65, Util.MethodToPtr(Request33), 0x08, FLAG_IRQ);
            SetEntry(66, Util.MethodToPtr(Request34), 0x08, FLAG_IRQ);
            SetEntry(67, Util.MethodToPtr(Request35), 0x08, FLAG_IRQ);
            SetEntry(68, Util.MethodToPtr(Request36), 0x08, FLAG_IRQ);
            SetEntry(69, Util.MethodToPtr(Request37), 0x08, FLAG_IRQ);
            SetEntry(70, Util.MethodToPtr(Request38), 0x08, FLAG_IRQ);
            SetEntry(71, Util.MethodToPtr(Request39), 0x08, FLAG_IRQ);
            SetEntry(72, Util.MethodToPtr(Request40), 0x08, FLAG_IRQ);
            SetEntry(73, Util.MethodToPtr(Request41), 0x08, FLAG_IRQ);
            SetEntry(74, Util.MethodToPtr(Request42), 0x08, FLAG_IRQ);
            SetEntry(75, Util.MethodToPtr(Request43), 0x08, FLAG_IRQ);
            SetEntry(76, Util.MethodToPtr(Request44), 0x08, FLAG_IRQ);
            SetEntry(77, Util.MethodToPtr(Request45), 0x08, FLAG_IRQ);
            SetEntry(78, Util.MethodToPtr(Request46), 0x08, FLAG_IRQ);
            SetEntry(79, Util.MethodToPtr(Request47), 0x08, FLAG_IRQ);
            SetEntry(80, Util.MethodToPtr(Request48), 0x08, FLAG_IRQ);
            SetEntry(81, Util.MethodToPtr(Request49), 0x08, FLAG_IRQ);
            SetEntry(82, Util.MethodToPtr(Request50), 0x08, FLAG_IRQ);
            SetEntry(83, Util.MethodToPtr(Request51), 0x08, FLAG_IRQ);
            SetEntry(84, Util.MethodToPtr(Request52), 0x08, FLAG_IRQ);
            SetEntry(85, Util.MethodToPtr(Request53), 0x08, FLAG_IRQ);
            SetEntry(86, Util.MethodToPtr(Request54), 0x08, FLAG_IRQ);
            SetEntry(87, Util.MethodToPtr(Request55), 0x08, FLAG_IRQ);
            SetEntry(88, Util.MethodToPtr(Request56), 0x08, FLAG_IRQ);
            SetEntry(89, Util.MethodToPtr(Request57), 0x08, FLAG_IRQ);
            SetEntry(90, Util.MethodToPtr(Request58), 0x08, FLAG_IRQ);
            SetEntry(91, Util.MethodToPtr(Request59), 0x08, FLAG_IRQ);
            SetEntry(92, Util.MethodToPtr(Request60), 0x08, FLAG_IRQ);
            SetEntry(93, Util.MethodToPtr(Request61), 0x08, FLAG_IRQ);
            SetEntry(94, Util.MethodToPtr(Request62), 0x08, FLAG_IRQ);
            SetEntry(95, Util.MethodToPtr(Request63), 0x08, FLAG_IRQ);
            SetEntry(96, Util.MethodToPtr(Request64), 0x08, FLAG_IRQ);
            SetEntry(97, Util.MethodToPtr(Request65), 0x08, FLAG_IRQ);
            SetEntry(98, Util.MethodToPtr(Request66), 0x08, FLAG_IRQ);
            SetEntry(99, Util.MethodToPtr(Request67), 0x08, FLAG_IRQ);
            SetEntry(100, Util.MethodToPtr(Request68), 0x08, FLAG_IRQ);
            SetEntry(101, Util.MethodToPtr(Request69), 0x08, FLAG_IRQ);
            SetEntry(102, Util.MethodToPtr(Request70), 0x08, FLAG_IRQ);
            SetEntry(103, Util.MethodToPtr(Request71), 0x08, FLAG_IRQ);
            SetEntry(104, Util.MethodToPtr(Request72), 0x08, FLAG_IRQ);
            SetEntry(105, Util.MethodToPtr(Request73), 0x08, FLAG_IRQ);
            SetEntry(106, Util.MethodToPtr(Request74), 0x08, FLAG_IRQ);
            SetEntry(107, Util.MethodToPtr(Request75), 0x08, FLAG_IRQ);
            SetEntry(108, Util.MethodToPtr(Request76), 0x08, FLAG_IRQ);
            SetEntry(109, Util.MethodToPtr(Request77), 0x08, FLAG_IRQ);
            SetEntry(110, Util.MethodToPtr(Request78), 0x08, FLAG_IRQ);
            SetEntry(111, Util.MethodToPtr(Request79), 0x08, FLAG_IRQ);
            SetEntry(112, Util.MethodToPtr(Request80), 0x08, FLAG_IRQ);
            SetEntry(113, Util.MethodToPtr(Request81), 0x08, FLAG_IRQ);
            SetEntry(114, Util.MethodToPtr(Request82), 0x08, FLAG_IRQ);
            SetEntry(115, Util.MethodToPtr(Request83), 0x08, FLAG_IRQ);
            SetEntry(116, Util.MethodToPtr(Request84), 0x08, FLAG_IRQ);
            SetEntry(117, Util.MethodToPtr(Request85), 0x08, FLAG_IRQ);
            SetEntry(118, Util.MethodToPtr(Request86), 0x08, FLAG_IRQ);
            SetEntry(119, Util.MethodToPtr(Request87), 0x08, FLAG_IRQ);
            SetEntry(120, Util.MethodToPtr(Request88), 0x08, FLAG_IRQ);
            SetEntry(121, Util.MethodToPtr(Request89), 0x08, FLAG_IRQ);
            SetEntry(122, Util.MethodToPtr(Request90), 0x08, FLAG_IRQ);
            SetEntry(123, Util.MethodToPtr(Request91), 0x08, FLAG_IRQ);
            SetEntry(124, Util.MethodToPtr(Request92), 0x08, FLAG_IRQ);
            SetEntry(125, Util.MethodToPtr(Request93), 0x08, FLAG_IRQ);
            SetEntry(126, Util.MethodToPtr(Request94), 0x08, FLAG_IRQ);
            SetEntry(129, Util.MethodToPtr(Request97), 0x08, FLAG_IRQ);
            SetEntry(130, Util.MethodToPtr(Request98), 0x08, FLAG_IRQ);
            SetEntry(131, Util.MethodToPtr(Request99), 0x08, FLAG_IRQ);
            SetEntry(132, Util.MethodToPtr(Request100), 0x08, FLAG_IRQ);
            SetEntry(133, Util.MethodToPtr(Request101), 0x08, FLAG_IRQ);
            SetEntry(134, Util.MethodToPtr(Request102), 0x08, FLAG_IRQ);
            SetEntry(135, Util.MethodToPtr(Request103), 0x08, FLAG_IRQ);
            SetEntry(136, Util.MethodToPtr(Request104), 0x08, FLAG_IRQ);
            SetEntry(137, Util.MethodToPtr(Request105), 0x08, FLAG_IRQ);
            SetEntry(138, Util.MethodToPtr(Request106), 0x08, FLAG_IRQ);
            SetEntry(139, Util.MethodToPtr(Request107), 0x08, FLAG_IRQ);
            SetEntry(140, Util.MethodToPtr(Request108), 0x08, FLAG_IRQ);
            SetEntry(141, Util.MethodToPtr(Request109), 0x08, FLAG_IRQ);
            SetEntry(142, Util.MethodToPtr(Request110), 0x08, FLAG_IRQ);
            SetEntry(143, Util.MethodToPtr(Request111), 0x08, FLAG_IRQ);
            SetEntry(144, Util.MethodToPtr(Request112), 0x08, FLAG_IRQ);
            SetEntry(145, Util.MethodToPtr(Request113), 0x08, FLAG_IRQ);
            SetEntry(146, Util.MethodToPtr(Request114), 0x08, FLAG_IRQ);
            SetEntry(147, Util.MethodToPtr(Request115), 0x08, FLAG_IRQ);
            SetEntry(148, Util.MethodToPtr(Request116), 0x08, FLAG_IRQ);
            SetEntry(149, Util.MethodToPtr(Request117), 0x08, FLAG_IRQ);
            SetEntry(150, Util.MethodToPtr(Request118), 0x08, FLAG_IRQ);
            SetEntry(151, Util.MethodToPtr(Request119), 0x08, FLAG_IRQ);
            SetEntry(152, Util.MethodToPtr(Request120), 0x08, FLAG_IRQ);
            SetEntry(153, Util.MethodToPtr(Request121), 0x08, FLAG_IRQ);
            SetEntry(154, Util.MethodToPtr(Request122), 0x08, FLAG_IRQ);
            SetEntry(155, Util.MethodToPtr(Request123), 0x08, FLAG_IRQ);
            SetEntry(156, Util.MethodToPtr(Request124), 0x08, FLAG_IRQ);
            SetEntry(157, Util.MethodToPtr(Request125), 0x08, FLAG_IRQ);
            SetEntry(158, Util.MethodToPtr(Request126), 0x08, FLAG_IRQ);
            SetEntry(159, Util.MethodToPtr(Request127), 0x08, FLAG_IRQ);
            SetEntry(160, Util.MethodToPtr(Request128), 0x08, FLAG_IRQ);
            SetEntry(161, Util.MethodToPtr(Request129), 0x08, FLAG_IRQ);
            SetEntry(162, Util.MethodToPtr(Request130), 0x08, FLAG_IRQ);
            SetEntry(163, Util.MethodToPtr(Request131), 0x08, FLAG_IRQ);
            SetEntry(164, Util.MethodToPtr(Request132), 0x08, FLAG_IRQ);
            SetEntry(165, Util.MethodToPtr(Request133), 0x08, FLAG_IRQ);
            SetEntry(166, Util.MethodToPtr(Request134), 0x08, FLAG_IRQ);
            SetEntry(167, Util.MethodToPtr(Request135), 0x08, FLAG_IRQ);
            SetEntry(168, Util.MethodToPtr(Request136), 0x08, FLAG_IRQ);
            SetEntry(169, Util.MethodToPtr(Request137), 0x08, FLAG_IRQ);
            SetEntry(170, Util.MethodToPtr(Request138), 0x08, FLAG_IRQ);
            SetEntry(171, Util.MethodToPtr(Request139), 0x08, FLAG_IRQ);
            SetEntry(172, Util.MethodToPtr(Request140), 0x08, FLAG_IRQ);
            SetEntry(173, Util.MethodToPtr(Request141), 0x08, FLAG_IRQ);
            SetEntry(174, Util.MethodToPtr(Request142), 0x08, FLAG_IRQ);
            SetEntry(175, Util.MethodToPtr(Request143), 0x08, FLAG_IRQ);
            SetEntry(176, Util.MethodToPtr(Request144), 0x08, FLAG_IRQ);
            SetEntry(177, Util.MethodToPtr(Request145), 0x08, FLAG_IRQ);
            SetEntry(178, Util.MethodToPtr(Request146), 0x08, FLAG_IRQ);
            SetEntry(179, Util.MethodToPtr(Request147), 0x08, FLAG_IRQ);
            SetEntry(180, Util.MethodToPtr(Request148), 0x08, FLAG_IRQ);
            SetEntry(181, Util.MethodToPtr(Request149), 0x08, FLAG_IRQ);
            SetEntry(182, Util.MethodToPtr(Request150), 0x08, FLAG_IRQ);
            SetEntry(183, Util.MethodToPtr(Request151), 0x08, FLAG_IRQ);
            SetEntry(184, Util.MethodToPtr(Request152), 0x08, FLAG_IRQ);
            SetEntry(185, Util.MethodToPtr(Request153), 0x08, FLAG_IRQ);
            SetEntry(186, Util.MethodToPtr(Request154), 0x08, FLAG_IRQ);
            SetEntry(187, Util.MethodToPtr(Request155), 0x08, FLAG_IRQ);
            SetEntry(188, Util.MethodToPtr(Request156), 0x08, FLAG_IRQ);
            SetEntry(189, Util.MethodToPtr(Request157), 0x08, FLAG_IRQ);
            SetEntry(190, Util.MethodToPtr(Request158), 0x08, FLAG_IRQ);
            SetEntry(191, Util.MethodToPtr(Request159), 0x08, FLAG_IRQ);
            SetEntry(192, Util.MethodToPtr(Request160), 0x08, FLAG_IRQ);
            SetEntry(193, Util.MethodToPtr(Request161), 0x08, FLAG_IRQ);
            SetEntry(194, Util.MethodToPtr(Request162), 0x08, FLAG_IRQ);
            SetEntry(195, Util.MethodToPtr(Request163), 0x08, FLAG_IRQ);
            SetEntry(196, Util.MethodToPtr(Request164), 0x08, FLAG_IRQ);
            SetEntry(197, Util.MethodToPtr(Request165), 0x08, FLAG_IRQ);
            SetEntry(198, Util.MethodToPtr(Request166), 0x08, FLAG_IRQ);
            SetEntry(199, Util.MethodToPtr(Request167), 0x08, FLAG_IRQ);
            SetEntry(200, Util.MethodToPtr(Request168), 0x08, FLAG_IRQ);
            SetEntry(201, Util.MethodToPtr(Request169), 0x08, FLAG_IRQ);
            SetEntry(202, Util.MethodToPtr(Request170), 0x08, FLAG_IRQ);
            SetEntry(203, Util.MethodToPtr(Request171), 0x08, FLAG_IRQ);
            SetEntry(204, Util.MethodToPtr(Request172), 0x08, FLAG_IRQ);
            SetEntry(205, Util.MethodToPtr(Request173), 0x08, FLAG_IRQ);
            SetEntry(206, Util.MethodToPtr(Request174), 0x08, FLAG_IRQ);
            SetEntry(207, Util.MethodToPtr(Request175), 0x08, FLAG_IRQ);
            SetEntry(208, Util.MethodToPtr(Request176), 0x08, FLAG_IRQ);
            SetEntry(209, Util.MethodToPtr(Request177), 0x08, FLAG_IRQ);
            SetEntry(210, Util.MethodToPtr(Request178), 0x08, FLAG_IRQ);
            SetEntry(211, Util.MethodToPtr(Request179), 0x08, FLAG_IRQ);
            SetEntry(212, Util.MethodToPtr(Request180), 0x08, FLAG_IRQ);
            SetEntry(213, Util.MethodToPtr(Request181), 0x08, FLAG_IRQ);
            SetEntry(214, Util.MethodToPtr(Request182), 0x08, FLAG_IRQ);
            SetEntry(215, Util.MethodToPtr(Request183), 0x08, FLAG_IRQ);
            SetEntry(216, Util.MethodToPtr(Request184), 0x08, FLAG_IRQ);
            SetEntry(217, Util.MethodToPtr(Request185), 0x08, FLAG_IRQ);
            SetEntry(218, Util.MethodToPtr(Request186), 0x08, FLAG_IRQ);
            SetEntry(219, Util.MethodToPtr(Request187), 0x08, FLAG_IRQ);
            SetEntry(220, Util.MethodToPtr(Request188), 0x08, FLAG_IRQ);
            SetEntry(221, Util.MethodToPtr(Request189), 0x08, FLAG_IRQ);
            SetEntry(222, Util.MethodToPtr(Request190), 0x08, FLAG_IRQ);
            SetEntry(223, Util.MethodToPtr(Request191), 0x08, FLAG_IRQ);
            SetEntry(224, Util.MethodToPtr(Request192), 0x08, FLAG_IRQ);
            SetEntry(225, Util.MethodToPtr(Request193), 0x08, FLAG_IRQ);
            SetEntry(226, Util.MethodToPtr(Request194), 0x08, FLAG_IRQ);
            SetEntry(227, Util.MethodToPtr(Request195), 0x08, FLAG_IRQ);
            SetEntry(228, Util.MethodToPtr(Request196), 0x08, FLAG_IRQ);
            SetEntry(229, Util.MethodToPtr(Request197), 0x08, FLAG_IRQ);
            SetEntry(230, Util.MethodToPtr(Request198), 0x08, FLAG_IRQ);
            SetEntry(231, Util.MethodToPtr(Request199), 0x08, FLAG_IRQ);
            SetEntry(232, Util.MethodToPtr(Request200), 0x08, FLAG_IRQ);
            SetEntry(233, Util.MethodToPtr(Request201), 0x08, FLAG_IRQ);
            SetEntry(234, Util.MethodToPtr(Request202), 0x08, FLAG_IRQ);
            SetEntry(235, Util.MethodToPtr(Request203), 0x08, FLAG_IRQ);
            SetEntry(236, Util.MethodToPtr(Request204), 0x08, FLAG_IRQ);
            SetEntry(237, Util.MethodToPtr(Request205), 0x08, FLAG_IRQ);
            SetEntry(238, Util.MethodToPtr(Request206), 0x08, FLAG_IRQ);
            SetEntry(239, Util.MethodToPtr(Request207), 0x08, FLAG_IRQ);
            SetEntry(240, Util.MethodToPtr(Request208), 0x08, FLAG_IRQ);
            SetEntry(241, Util.MethodToPtr(Request209), 0x08, FLAG_IRQ);
            SetEntry(242, Util.MethodToPtr(Request210), 0x08, FLAG_IRQ);
            SetEntry(243, Util.MethodToPtr(Request211), 0x08, FLAG_IRQ);
            SetEntry(244, Util.MethodToPtr(Request212), 0x08, FLAG_IRQ);
            SetEntry(245, Util.MethodToPtr(Request213), 0x08, FLAG_IRQ);
            SetEntry(246, Util.MethodToPtr(Request214), 0x08, FLAG_IRQ);
            SetEntry(247, Util.MethodToPtr(Request215), 0x08, FLAG_IRQ);
            SetEntry(248, Util.MethodToPtr(Request216), 0x08, FLAG_IRQ);
            SetEntry(249, Util.MethodToPtr(Request217), 0x08, FLAG_IRQ);
            SetEntry(250, Util.MethodToPtr(Request218), 0x08, FLAG_IRQ);
            SetEntry(251, Util.MethodToPtr(Request219), 0x08, FLAG_IRQ);
            SetEntry(252, Util.MethodToPtr(Request220), 0x08, FLAG_IRQ);
            SetEntry(253, Util.MethodToPtr(Request221), 0x08, FLAG_IRQ);
            SetEntry(254, Util.MethodToPtr(Request222), 0x08, FLAG_IRQ);
            SetEntry(255, Util.MethodToPtr(Request223), 0x08, FLAG_IRQ);

            #endregion

            #region Syscall

            SetEntry(0x80, Util.MethodToPtr(Syscall), 0x08, FLAG_TRAP);
            SetEntry(0x81, Util.MethodToPtr(Yield), 0x08, FLAG_INT);

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

        #region Request routines

        private static extern void Request0();
        private static extern void Request1();
        private static extern void Request2();
        private static extern void Request3();
        private static extern void Request4();
        private static extern void Request5();
        private static extern void Request6();
        private static extern void Request7();
        private static extern void Request8();
        private static extern void Request9();
        private static extern void Request10();
        private static extern void Request11();
        private static extern void Request12();
        private static extern void Request13();
        private static extern void Request14();
        private static extern void Request15();
        private static extern void Request16();
        private static extern void Request17();
        private static extern void Request18();
        private static extern void Request19();
        private static extern void Request20();
        private static extern void Request21();
        private static extern void Request22();
        private static extern void Request23();
        private static extern void Request24();
        private static extern void Request25();
        private static extern void Request26();
        private static extern void Request27();
        private static extern void Request28();
        private static extern void Request29();
        private static extern void Request30();
        private static extern void Request31();
        private static extern void Request32();
        private static extern void Request33();
        private static extern void Request34();
        private static extern void Request35();
        private static extern void Request36();
        private static extern void Request37();
        private static extern void Request38();
        private static extern void Request39();
        private static extern void Request40();
        private static extern void Request41();
        private static extern void Request42();
        private static extern void Request43();
        private static extern void Request44();
        private static extern void Request45();
        private static extern void Request46();
        private static extern void Request47();
        private static extern void Request48();
        private static extern void Request49();
        private static extern void Request50();
        private static extern void Request51();
        private static extern void Request52();
        private static extern void Request53();
        private static extern void Request54();
        private static extern void Request55();
        private static extern void Request56();
        private static extern void Request57();
        private static extern void Request58();
        private static extern void Request59();
        private static extern void Request60();
        private static extern void Request61();
        private static extern void Request62();
        private static extern void Request63();
        private static extern void Request64();
        private static extern void Request65();
        private static extern void Request66();
        private static extern void Request67();
        private static extern void Request68();
        private static extern void Request69();
        private static extern void Request70();
        private static extern void Request71();
        private static extern void Request72();
        private static extern void Request73();
        private static extern void Request74();
        private static extern void Request75();
        private static extern void Request76();
        private static extern void Request77();
        private static extern void Request78();
        private static extern void Request79();
        private static extern void Request80();
        private static extern void Request81();
        private static extern void Request82();
        private static extern void Request83();
        private static extern void Request84();
        private static extern void Request85();
        private static extern void Request86();
        private static extern void Request87();
        private static extern void Request88();
        private static extern void Request89();
        private static extern void Request90();
        private static extern void Request91();
        private static extern void Request92();
        private static extern void Request93();
        private static extern void Request94();
        private static extern void Request97();
        private static extern void Request98();
        private static extern void Request99();
        private static extern void Request100();
        private static extern void Request101();
        private static extern void Request102();
        private static extern void Request103();
        private static extern void Request104();
        private static extern void Request105();
        private static extern void Request106();
        private static extern void Request107();
        private static extern void Request108();
        private static extern void Request109();
        private static extern void Request110();
        private static extern void Request111();
        private static extern void Request112();
        private static extern void Request113();
        private static extern void Request114();
        private static extern void Request115();
        private static extern void Request116();
        private static extern void Request117();
        private static extern void Request118();
        private static extern void Request119();
        private static extern void Request120();
        private static extern void Request121();
        private static extern void Request122();
        private static extern void Request123();
        private static extern void Request124();
        private static extern void Request125();
        private static extern void Request126();
        private static extern void Request127();
        private static extern void Request128();
        private static extern void Request129();
        private static extern void Request130();
        private static extern void Request131();
        private static extern void Request132();
        private static extern void Request133();
        private static extern void Request134();
        private static extern void Request135();
        private static extern void Request136();
        private static extern void Request137();
        private static extern void Request138();
        private static extern void Request139();
        private static extern void Request140();
        private static extern void Request141();
        private static extern void Request142();
        private static extern void Request143();
        private static extern void Request144();
        private static extern void Request145();
        private static extern void Request146();
        private static extern void Request147();
        private static extern void Request148();
        private static extern void Request149();
        private static extern void Request150();
        private static extern void Request151();
        private static extern void Request152();
        private static extern void Request153();
        private static extern void Request154();
        private static extern void Request155();
        private static extern void Request156();
        private static extern void Request157();
        private static extern void Request158();
        private static extern void Request159();
        private static extern void Request160();
        private static extern void Request161();
        private static extern void Request162();
        private static extern void Request163();
        private static extern void Request164();
        private static extern void Request165();
        private static extern void Request166();
        private static extern void Request167();
        private static extern void Request168();
        private static extern void Request169();
        private static extern void Request170();
        private static extern void Request171();
        private static extern void Request172();
        private static extern void Request173();
        private static extern void Request174();
        private static extern void Request175();
        private static extern void Request176();
        private static extern void Request177();
        private static extern void Request178();
        private static extern void Request179();
        private static extern void Request180();
        private static extern void Request181();
        private static extern void Request182();
        private static extern void Request183();
        private static extern void Request184();
        private static extern void Request185();
        private static extern void Request186();
        private static extern void Request187();
        private static extern void Request188();
        private static extern void Request189();
        private static extern void Request190();
        private static extern void Request191();
        private static extern void Request192();
        private static extern void Request193();
        private static extern void Request194();
        private static extern void Request195();
        private static extern void Request196();
        private static extern void Request197();
        private static extern void Request198();
        private static extern void Request199();
        private static extern void Request200();
        private static extern void Request201();
        private static extern void Request202();
        private static extern void Request203();
        private static extern void Request204();
        private static extern void Request205();
        private static extern void Request206();
        private static extern void Request207();
        private static extern void Request208();
        private static extern void Request209();
        private static extern void Request210();
        private static extern void Request211();
        private static extern void Request212();
        private static extern void Request213();
        private static extern void Request214();
        private static extern void Request215();
        private static extern void Request216();
        private static extern void Request217();
        private static extern void Request218();
        private static extern void Request219();
        private static extern void Request220();
        private static extern void Request221();
        private static extern void Request222();
        private static extern void Request223();

        #endregion

        #region Syscall routines

        private static extern void Syscall();
        private static extern void Yield();

        #endregion
    }
}
