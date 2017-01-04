using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    class PCIFS
    {
        private static Dictionary m_dictionary;
        private static List m_direntries;
        private static Node m_currentNode;

        public static unsafe void Init()
        {
            LoadDevices();


            MountPoint mp = new MountPoint();
            mp.Name = "pci";
            m_currentNode = new Node();
            m_currentNode.FindDir = findDirImpl;
            m_currentNode.ReadDir = readDirImpl;
            m_currentNode.Flags = NodeFlags.DIRECTORY;

            mp.Node = m_currentNode;

            VFS.AddMountPoint(mp);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            return null;
        }


        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= m_direntries.Count)
                return null;

            string devname = (string)m_direntries.Item[(int)index];
            if (devname == null)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int strlen = String.Length(devname);
            int i = 0;
            for (; i < strlen; i++)
                entry->Name[i] = devname[i];
            entry->Name[i] = '\0';

            return entry;
        }

        public static unsafe void LoadDevices()
        {
            m_dictionary = new Dictionary();
            m_direntries = new List();

            for (int i = 0; i < PCI.DeviceNum; i++)
            {
                PciDevice dev = PCI.GetDevices()[i];

                if (dev == null)
                    continue;

                long key = GenerateKey((byte)dev.Bus, (byte)dev.Slot, (byte)dev.Function);
                
                m_dictionary.Add(key, dev);
                m_direntries.Add(GenerateNoteName(dev.Bus, dev.Slot, dev.Function));
            }
        }

        public static unsafe string GenerateNoteName(int bus, int slot, int function)
        {
            string part1 = Int.ToString(bus);
            string part2 = Int.ToString(slot);
            string part3 = Int.ToString(function);
            
            char* ptr = (char*)Heap.Alloc(10);
            int x = 0;
            for (int j = 0; j < String.Length(part1); j++)
                ptr[x++] = part1[j];
            ptr[x++] = ':';
            for (int j = 0; j < String.Length(part2); j++)
                ptr[x++] = part2[j];
            ptr[x++] = ':';
            for (int j = 0; j < String.Length(part3); j++)
                ptr[x++] = part3[j];
            ptr[x] = (char)0x00;

            // NOTE: MEMLEAK

            return Util.CharPtrToString(ptr);
        }

        public static long GetKey(string str)
        {
            int count = String.Count(str, ':');

            if (count != 2)
                return -1;

            return 0;
        }

        public static long GenerateKey(byte bus, byte device, byte function)
        {
            return (bus << 16) | (device << 8) | (function);
        } 

    }
}
