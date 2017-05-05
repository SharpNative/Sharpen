using Sharpen.FileSystem.Cookie;

namespace Sharpen.FileSystem
{
    public class Node
    {
        // TODO: cookie cleaning?
        public uint Size;
        public ICookie Cookie;
        public FileMode FileMode;
        public int OpenFlags;
        public NodeFlags Flags;
        public bool IsOpen;

        public FSRead Read;
        public FSWrite Write;
        public FSTruncate Truncate;
        public FSOpen Open;
        public FSClose Close;
        public FSFindDir FindDir;
        public FSReaddir ReadDir;
        public FSGetSize GetSize;
        public FSIOCtl IOCtl;
        
        public unsafe delegate uint FSRead(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate uint FSWrite(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate uint FSTruncate(Node node, uint size);
        public unsafe delegate void FSOpen(Node node);
        public unsafe delegate void FSClose(Node node);
        public unsafe delegate Node FSFindDir(Node node, string name);
        public unsafe delegate DirEntry* FSReaddir(Node node, uint index);
        public unsafe delegate uint FSGetSize(Node node);
        public unsafe delegate int FSIOCtl(Node node, int request, void* arg);

        /// <summary>
        /// Creates the stat structure
        /// </summary>
        /// <param name="st"></param>
        public unsafe void Stat(Stat* st)
        {
            st->st_dev = 0;
            st->st_rdev = 0;
            st->st_ino = 0;
            st->st_size = VFS.GetSize(this);

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

        /// <summary>
        /// Clones a filesystem node
        /// </summary>
        /// <returns>The clone</returns>
        public Node Clone()
        {
            Node clone = new Node();

            // TODO: clone cookie? or is this the task of the filesystem implementation itself?
            clone.Size = Size;
            clone.Cookie = Cookie;
            clone.FileMode = FileMode;
            clone.OpenFlags = OpenFlags;
            clone.Flags = Flags;
            clone.IsOpen = IsOpen;

            clone.Open = Open;
            clone.Close = Close;
            clone.Read = Read;
            clone.Write = Write;
            clone.Truncate = Truncate;
            clone.FindDir = FindDir;
            clone.ReadDir = ReadDir;
            clone.GetSize = GetSize;
            clone.IOCtl = IOCtl;
            
            return clone;
        }
    }

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
