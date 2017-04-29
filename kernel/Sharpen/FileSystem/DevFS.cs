using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.FileSystem
{
    public class DevFS
    {
        private static StringDictionary m_devices;
        private static Node m_currentNode;

        /// <summary>
        /// Initializes DevFS
        /// </summary>
        public unsafe static void Init()
        {
            m_devices = new StringDictionary(8);
            
            MountPoint mp = new MountPoint();
            mp.Name = "devices";
            m_currentNode = new Node();
            m_currentNode.FindDir = findDirImpl;
            m_currentNode.ReadDir = readDirImpl;
            m_currentNode.Flags = NodeFlags.DIRECTORY;

            mp.Node = m_currentNode;

            VFS.AddMountPoint(mp);
        }

        /// <summary>
        /// Register device in devices
        /// </summary>
        /// <param name="dev">The device</param>
        public unsafe static void RegisterDevice(Device dev)
        {
            m_devices.Add(dev.Name, dev);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            Device dev = (Device)m_devices.Get(name);
            if (dev == null)
                return null;

            return dev.Node;
        }

        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= m_devices.Count)
                return null;

            Device dev = (Device)m_devices.GetAt((int)index);
            if (dev == null)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int i = 0;
            for (; dev.Name[i] != '\0'; i++)
                entry->Name[i] = dev.Name[i];
            entry->Name[i] = '\0';

            return entry;
        }
    }
}
