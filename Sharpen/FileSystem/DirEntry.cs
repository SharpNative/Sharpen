using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    public struct DirEntry
    {
        public uint Ino;
        public int Offset;
        public ushort Reclen;
        public byte Type;

        public unsafe fixed char Name[256];
    }
}
