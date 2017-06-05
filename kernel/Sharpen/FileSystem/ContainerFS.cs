using Sharpen.Collections;
using Sharpen.FileSystem.Cookie;
using Sharpen.Lib;
using Sharpen.Mem;

namespace Sharpen.FileSystem
{
    public class ContainerFS
    {
        private StringDictionary m_children;
        public Node Node { get; private set; }

        /// <summary>
        /// Initializes a filesystem that is a container
        /// </summary>
        public unsafe ContainerFS()
        {
            m_children = new StringDictionary(8);

            Node = new Node();
            Node.FindDir = findDirImpl;
            Node.ReadDir = readDirImpl;
            Node.Flags = NodeFlags.DIRECTORY;
            Node.Cookie = new ContainerCookie(this);
        }

        /// <summary>
        /// Remove a entry
        /// </summary>
        /// <param name="name">Name of entry</param>
        /// <returns>Success?</returns>
        public bool RemoveEntry(string name)
        {
            if (m_children.Get(name) == null)
                return false;

            m_children.Remove(name);

            return true;
        }

        /// <summary>
        /// Adds an entry
        /// </summary>
        /// <param name="entry">The entry</param>
        public void AddEntry(RootPoint entry)
        {
            m_children.Add(entry.Name, entry);
        }

        /// <summary>
        /// Gets an entry
        /// </summary>
        /// <param name="name">The entry name</param>
        /// <returns>The node associated with the entry</returns>
        public RootPoint GetEntry(string name)
        {
            return (RootPoint)m_children.Get(name);
        }
        
        /// <summary>
        /// Gets the entry at a given index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The entry</returns>
        public RootPoint GetAt(uint index)
        {
            if (index >= m_children.Count)
                return null;

            return (RootPoint)m_children.GetAt(index);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            ContainerCookie cookie = (ContainerCookie)node.Cookie;
            RootPoint root = cookie.FS.GetEntry(name);
            if (root == null)
                return null;

            return root.Node;
        }

        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            ContainerCookie cookie = (ContainerCookie)node.Cookie;
            RootPoint dev = cookie.FS.GetAt(index);
            if (dev == null)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
            String.CopyTo(entry->Name, dev.Name);

            return entry;
        }
    }
}
