using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    public class Node
    {
        public uint Flags;
        public uint Cookie;

        public FSRead Read;
        public FSWrite Write;
        public FSOpen Open;
        public FSClose Close;
        public FSFindDir FindDir;
        public FSReaddir ReadDir;
        
        public unsafe delegate uint FSRead(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate uint FSWrite(Node node, uint offset, uint size, byte[] buffer);
        public unsafe delegate void FSOpen(Node node, uint read, uint write);
        public unsafe delegate void FSClose(Node node);
        public unsafe delegate Node FSFindDir(Node node, string name);
        public unsafe delegate DirEntry *FSReaddir(Node node, uint index);
    }

    public class NodeFlags
    {
        public static uint DIRECTORY = (1 << 0);
        public static uint FILE = (1 << 1);
        public static uint DEVICE = (1 << 2);
    }
}
