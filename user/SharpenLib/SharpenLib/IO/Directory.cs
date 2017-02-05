using LibCS2C.Attributes;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.IO
{
    public sealed unsafe class Directory
    {
        public struct DirEntry
        {
            public uint Ino;
            public int Offset;
            public byte Type;
            public ushort Reclen;

            public unsafe fixed char Name[256];
        }

        private struct Dir
        {
            public int Descriptor;
            public uint Last;
            DirEntry __Current;
        }

        private Dir* m_instance;

        /// <summary>
        /// Open directory
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static Directory Open(string path)
        {
            Directory instance = new Directory();
            instance.m_instance = openInternal(path);
            return instance;
        }

        /// <summary>
        /// Read next directory entry
        /// </summary>
        /// <returns>Next directory entry</returns>
        public DirEntry Readdir()
        {
            DirEntry* read = readdir(m_instance);

            DirEntry entry = new DirEntry();
            if (read != null)
                Memory.Memcpy(&entry, read, sizeof(DirEntry));

            return entry;
        }

        /// <summary>
        /// Closes a directory
        /// </summary>
        public void Close()
        {
            closeInternal(m_instance);
        }
        
        /// <summary>
        /// Gets the current working directory
        /// </summary>
        /// <param name="destination">The destination buffer</param>
        /// <param name="size">The size of the buffer</param>
        /// <returns>The buffer</returns>
        [Extern("getcwd")]
        public static extern unsafe char* GetCWD(char* destination, int size);

        /// <summary>
        /// Changes the current working directory
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>0 if success, -1 if failed</returns>
        [Extern("chdir")]
        private static extern int CHDir(string path);

        /// <summary>
        /// Changes the current working directory
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>If the directory change was successful</returns>
        public static bool SetCurrentDirectory(string path)
        {
            return (CHDir(path) == 0);
        }

        /// <summary>
        /// Gets the current directory
        /// </summary>
        /// <returns>The current directory</returns>
        public static string GetCurrentDirectory()
        {
            char[] str = new char[4096];
            GetCWD((char*)Util.ObjectToVoidPtr(str), 4096);
            return Util.CharArrayToString(str);
        }
        
        /// <summary>
        /// Internal open directory method
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The pointer to the directory</returns>
        [Extern("opendir")]
        private static extern Dir* openInternal(string path);

        /// <summary>
        /// Internal read directory method
        /// </summary>
        /// <param name="instance">The pointer to the directory structure</param>
        /// <param name="entry">The directory entry destination</param>
        /// <param name="index">The index</param>
        [Extern("readdir")]
        private static extern DirEntry* readdir(void* instance);

        /// <summary>
        /// Internal close directory method
        /// </summary>
        /// <param name="instance">The pointer to the directory structure</param>
        [Extern("closedir")]
        private static extern int closeInternal(void* instance);
    }
}
