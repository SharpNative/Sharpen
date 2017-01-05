using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PCIFSInfo
    {
        public ushort Bus;
        public ushort Slot;
        public ushort Function;

        public byte classCode;
        public byte SubClass;

        public ushort Vendor;
        public ushort Device;
    }
}
