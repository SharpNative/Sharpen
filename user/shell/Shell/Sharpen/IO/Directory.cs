namespace Sharpen.IO
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
        
        /// <summary>
        /// Open directory
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static unsafe Directory Open(string path)
        {
            Directory instance = new Directory();
            instance.m_instance = OpenInternal(path);
            return instance;
        }

        /// <summary>
        /// Read directory item
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
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

        public static extern bool SetCurrentDirectory(string path);
        public static extern string GetCurrentDirectory();
    }
}
