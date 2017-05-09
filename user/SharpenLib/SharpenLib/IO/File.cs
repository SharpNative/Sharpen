using LibCS2C.Attributes;
using Sharpen.Utilities;

namespace Sharpen.IO
{
    public class File
    {
        public unsafe struct Stat
        {
            public short st_dev;
            public ushort st_ino;
            public uint st_mode;
            public ushort st_nlink;
            public ushort st_uid;
            public ushort st_gid;
            public short st_rdev;
            public uint st_size;
            public ulong st_atime;
            public ulong st_mtime;
            public ulong st_ctime;
            public uint st_blksize;
            public uint st_blkcnt;
        }

        public enum FileMode
        {
            ReadOnly,
            WriteOnly,
            ReadWrite
        }

        private int m_fd;

        public bool IsOpen
        {
            get
            {
                return (m_fd >= 0);
            }
        }

        public int FileDescriptorID { get { return m_fd; } }

        /// <summary>
        /// Creates a new File object
        /// </summary>
        /// <param name="path">The path of the file</param>
        public File(string path)
        {
            m_fd = Open(path, (int)FileMode.ReadWrite);
        }

        /// <summary>
        /// Creates a new File object
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <param name="mode">The file opening mode</param>
        public File(string path, FileMode mode)
        {
            m_fd = Open(path, (int)mode);
        }

        /// <summary>
        /// Closes this file
        /// </summary>
        public void Close()
        {
            if (m_fd < 0)
                return;

            Close(m_fd);
            m_fd = -1;
        }

        /// <summary>
        /// Gets the file status
        /// </summary>
        /// <returns>The stat struct</returns>
        public unsafe Stat GetStatus()
        {
            Stat st = new Stat();
            if (m_fd >= 0)
            {
                fstatInternal(m_fd, &st);
            }
            return st;
        }

        /// <summary>
        /// Gets the size of the file
        /// </summary>
        /// <returns>The file size</returns>
        public uint GetSize()
        {
            return GetStatus().st_size;
        }

        /// <summary>
        /// Reads file contents into a buffer
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The amount of bytes to read</param>
        /// <returns>How many bytes were read</returns>
        public unsafe int Read(byte[] buffer, int size)
        {
            if (m_fd < 0)
                return 0;
            
            return Read(m_fd, Util.ObjectToVoidPtr(buffer), size);
        }

        /// <summary>
        /// Reads file contents into a buffer
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The amount of bytes to read</param>
        /// <returns>How many bytes were read</returns>
        public unsafe int Read(void* buffer, int size)
        {
            if (m_fd < 0)
                return 0;

            return Read(m_fd, buffer, size);
        }

        /// <summary>
        /// Writes a buffer to a file
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The amount of bytes to write</param>
        /// <returns>How many bytes were written</returns>
        public unsafe int Write(byte[] buffer, int size)
        {
            if (m_fd < 0)
                return 0;

            return Write(m_fd, Util.ObjectToVoidPtr(buffer), size);
        }

        /// <summary>
        /// Writes a buffer to a file
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The amount of bytes to write</param>
        /// <returns>How many bytes were written</returns>
        public unsafe int Write(void* buffer, int size)
        {
            if (m_fd < 0)
                return 0;

            return Write(m_fd, buffer, size);
        }

        /// <summary>
        /// Internal open method
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="flags">The flags</param>
        [Extern("open")]
        public static extern int Open(string path, int flags);

        /// <summary>
        /// Internal read method
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <param name="buf">The buffer</param>
        /// <param name="nbytes">The amount of bytes to read</param>
        /// <returns>The amount of bytes read</returns>
        [Extern("read")]
        public static extern unsafe int Read(int fd, void* buf, int nbytes);

        /// <summary>
        /// Internal write method
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <param name="buf">The buffer</param>
        /// <param name="nbytes">The amount of bytes to write</param>
        /// <returns>The amount of bytes written</returns>
        [Extern("write")]
        public static extern unsafe int Write(int fd, void* buf, int nbytes);

        /// <summary>
        /// Internal stat method
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <param name="st">The stat struct</param>
        [Extern("fstat")]
        private static extern unsafe int fstatInternal(int fd, Stat* st);

        /// <summary>
        /// Internal close method
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        [Extern("close")]
        public static extern int Close(int fd);

        /// <summary>
        /// Create pipe
        /// </summary>
        /// <param name="fd">File descriptors</param>
        /// <returns>Status</returns>
        [Extern("pipe")]
        public static extern int Pipe(int[] fd);

        /// <summary>
        /// Duplicate a file descriptor
        /// </summary>
        /// <param name="oldfd">Old file descriptors</param>
        /// <returns>Status</returns>
        [Extern("dup")]
        public static extern int Dup(int oldfd);

        /// <summary>
        /// Duplicate a file descriptor
        /// </summary>
        /// <param name="oldfd">Old file descriptor</param>
        /// <param name="newfd">New file descriptor</param>
        /// <returns>status</returns>
        [Extern("dup2")]
        public static extern int Dup2(int oldfd, int newfd);
    }
}
