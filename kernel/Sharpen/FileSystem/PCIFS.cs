using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    class PCIFS
    {
        struct PCIFSInfo
        {
            public ushort Bus;
            public ushort Slot;
            public ushort Function;

            public byte classCode;
            public byte SubClass;
            public byte ProgIntf;

            public ushort Vendor;
            public ushort Device;
        }

        private static Dictionary m_dictionary;
        private static List m_direntries;
        private static Node m_currentNode;

        /// <summary>
        /// Initializes PCI FS
        /// </summary>
        public static unsafe void Init()
        {
            LoadDevices();
            
            MountPoint mp = new MountPoint();
            mp.Name = "pci";
            m_currentNode = new Node();
            m_currentNode.FindDir = rootFindDir;
            m_currentNode.ReadDir = rootReadDir;
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
        private static unsafe Node rootFindDir(Node node, string name)
        {
            uint key = (uint)GetKey(name);

            object obj = m_dictionary.GetByKey(key);
            if (obj == null)
                return null;

            IDCookie cookie = new IDCookie((int)key);

            Node outNode = new Node();
            outNode.Cookie = cookie;
            outNode.Flags = NodeFlags.DIRECTORY;
            outNode.ReadDir = deviceReadDir;
            outNode.FindDir = deviceFindDir;

            return outNode;
        }
        
        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* rootReadDir(Node node, uint index)
        {
            if (index >= m_direntries.Count)
                return null;

            string devname = (string)m_direntries.Item[(int)index];
            if (devname == null)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int strlen = devname.Length;
            int i = 0;
            for (; i < strlen; i++)
                entry->Name[i] = devname[i];
            entry->Name[i] = '\0';

            return entry;
        }

        /// <summary>
        /// Load devices
        /// </summary>
        public static unsafe void LoadDevices()
        {
            m_dictionary = new Dictionary();
            m_direntries = new List();

            for (int i = 0; i < PCI.DeviceNum; i++)
            {
                PciDevice dev = PCI.Devices[i];
                if (dev == null)
                    continue;

                long key = GenerateKey(dev.Bus, dev.Slot, dev.Function);
                
                m_dictionary.Add(key, dev);
                m_direntries.Add(GenerateNodeName(dev.Bus, dev.Slot, dev.Function));
            }
        }

        /// <summary>
        /// Generate note name from bus/slot/function
        /// </summary>
        /// <param name="bus">The bus</param>
        /// <param name="slot">The slot</param>
        /// <param name="function">The function</param>
        /// <returns>The node name</returns>
        public static unsafe string GenerateNodeName(int bus, int slot, int function)
        {
            string part1 = bus.ToString();
            string part2 = slot.ToString();
            string part3 = function.ToString();

            char* ptr = (char*)Heap.Alloc(10);
            int x = 0;
            for (int j = 0; j < part1.Length; j++)
                ptr[x++] = part1[j];
            ptr[x++] = ':';
            for (int j = 0; j < part2.Length; j++)
                ptr[x++] = part2[j];
            ptr[x++] = ':';
            for (int j = 0; j < part3.Length; j++)
                ptr[x++] = part3[j];
            ptr[x] = '\0';

            Heap.Free(part1);
            Heap.Free(part2);
            Heap.Free(part3);

            return Util.CharPtrToString(ptr);
        }

        /// <summary>
        /// Convert FS key to long key
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long GetKey(string str)
        {
            int count = String.Count(str, ':');
            if (count != 2)
                return -1;

            long l = 0;

            int shiftOffset = 16;
            int offset = 0;
            for (int j = 0; j < 3; j++)
            {
                int index = str.IndexOf(':', offset);
                if (index == -1)
                    index = str.Length - offset;
                else
                    index -= offset;
                
                string part = str.Substring(offset, index);
                
                uint num = (uint)int.Parse(part);

                l |= num << shiftOffset;

                shiftOffset -= 8;

                Heap.Free(part);

                offset += index + 1;
            }

            return l;
        }

        /// <summary>
        /// Generate key from bus/device/function
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="device"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static uint GenerateKey(byte bus, byte device, byte function)
        {
            return (uint)((bus << 16) | (device << 8) | (function));
        }

        /// <summary>
        /// PCI device find dir
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static unsafe DirEntry* deviceReadDir(Node node, uint index)
        {
            if (index == 0)
                return makeByName("info");

            return null;
        }

        /// <summary>
        /// PCI device read dir
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static unsafe Node deviceFindDir(Node node, string name)
        {
            if (name.Equals("info"))
            {
                IDCookie cookie = (IDCookie)node.Cookie;
                return byKey(0, (uint)cookie.ID);
            }

            return null;
        }

        /// <summary>
        /// Generate note from key and type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static unsafe Node byKey(int type, uint key)
        {
            Node node = new Node();

            IDCookie cookie = new IDCookie((int)key);
            node.Cookie = cookie;

            if (type == 0)
            {
                node.Size = (uint)sizeof(PCIFSInfo);
                node.Read = infoReadImpl;
            }

            return node;
        }

        /// <summary>
        /// Make direntry by name
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static unsafe DirEntry* makeByName(string str)
        {
            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            Memory.Memcpy(entry->Name, Util.ObjectToVoidPtr(str), str.Length + 1);

            return entry;
        }

        /// <summary>
        /// Read function info
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static unsafe uint infoReadImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            IDCookie cookie = (IDCookie)node.Cookie;
            PciDevice dev = (PciDevice)m_dictionary.GetByKey((uint)cookie.ID);
            if (dev == null)
                return 0;

            /**
             * Generate info copy struct
             */
            PCIFSInfo* info = (PCIFSInfo*)Heap.Alloc(sizeof(PCIFSInfo));
            info->Bus = dev.Bus;
            info->Slot = dev.Slot;
            info->Function = dev.Function;

            info->classCode = dev.classCode;
            info->SubClass = dev.SubClass;
            info->ProgIntf = dev.ProgIntf;

            info->Vendor = dev.Vendor;
            info->Device = dev.Device;

            if (size > sizeof(PCIFSInfo))
                size = (uint)sizeof(PCIFSInfo);

            Memory.Memcpy(Util.ObjectToVoidPtr(buffer), info, (int)size);

            Heap.Free(info);

            return size;
        }
    }
}
