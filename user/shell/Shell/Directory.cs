using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Directory
    {
        public struct DirEntry
        {
            public uint Ino;
            public int Offset;
            public byte Type;
            public ushort Reclen;

            public unsafe fixed char Name[256];
        }

        private unsafe void* m_instance;

        private static unsafe extern void* OpenInternal(string path);
        private static unsafe extern void ReaddirInternal(void* instance, DirEntry* entry, uint index);
        private static unsafe extern void CloseInternal(void* instance);

        public static unsafe Directory Open(string path)
        {
            Directory instance = new Directory();
            instance.m_instance = OpenInternal(path);
            return instance;
        }

        public unsafe DirEntry Readdir(uint index)
        {
            DirEntry entry = new DirEntry();
            ReaddirInternal(m_instance, &entry, index);
            return entry;
        }

        public unsafe void Close()
        {
            CloseInternal(m_instance);
        }
    }
}
