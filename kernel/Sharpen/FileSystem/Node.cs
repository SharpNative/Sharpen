using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    public class Node
    {
        public uint Size;
        public uint Cookie;
        public FileMode FileMode;
        public NodeFlags Flags;

        public FSRead Read;
        public FSWrite Write;
        public FSOpen Open;
        public FSClose Close;
        public FSFindDir FindDir;
        public FSReaddir ReadDir;
        
        public unsafe delegate uint FSRead(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate uint FSWrite(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate void FSOpen(Node node);
        public unsafe delegate void FSClose(Node node);
        public unsafe delegate Node FSFindDir(Node node, string name);
        public unsafe delegate DirEntry* FSReaddir(Node node, uint index);

        /// <summary>
        /// Creates the stat structure
        /// </summary>
        /// <param name="st"></param>
        public unsafe void Stat(Stat* st)
        {
            st->st_dev = 0;
            st->st_rdev = 0;
            st->st_ino = 0;
            st->st_size = Size;

            st->st_mode = 0;
            st->st_uid = 0;
            st->st_gid = 0;

            st->st_atime = 0;
            st->st_mtime = 0;
            st->st_ctime = 0;

            st->st_nlink = 1;
            st->st_blksize = 0;
            st->st_blkcnt = 0;
        }
    }

    public struct Stat
    {
        public ushort st_dev;
        public ushort st_ino;
        public uint st_mode;
        public ushort st_nlink;
        public ushort st_uid;
        public ushort st_gid;
        public ushort st_rdev;
        public uint st_size;
        public ulong st_atime;
        public ulong st_mtime;
        public ulong st_ctime;
        public uint st_blksize;
        public uint st_blkcnt;
    }

    public enum NodeFlags
    {
        DIRECTORY = (1 << 0),
        FILE = (1 << 1),
        DEVICE = (1 << 2)
    }

    public enum FileMode
    {
        O_RDONLY = 0,
        O_WRONLY = 1,
        O_RDWR = 2,
        O_NONE = 3
    }
}
