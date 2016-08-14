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
