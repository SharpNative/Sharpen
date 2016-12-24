using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct V8086Regs
    {
        public ushort di, si, bp, sp, bx, dx, cx, ax;
        public ushort gs, fs, es, ds, eflags;
    }

    unsafe class V8086
    {
        /// <summary>
        /// Do a BIOS interrupt
        /// </summary>
        /// <param name="intnum">Interrupt number</param>
        /// <param name="regs">Register struct</param>
        public static void Interrupt(byte intnum, V8086Regs* regs)
        {
            Paging.Disable();

            Int(intnum, regs);

            Paging.Enable();
        }

        private extern static void Int(byte intnum, V8086Regs *regs);
    }
}
