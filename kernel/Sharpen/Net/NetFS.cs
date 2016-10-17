using Sharpen.Collections;
using Sharpen.FileSystem;
using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class NetFS
    {

        private static Dictionary m_devices;
        private static Node m_currentNode;

        /// <summary>
        /// Initializes DevFS
        /// </summary>
        public unsafe static void Init()
        {
            m_devices = new Dictionary();

            MountPoint mp = new MountPoint();
            mp.Name = "net";
            m_currentNode = new Node();
            m_currentNode.FindDir = findDirImpl;
            m_currentNode.ReadDir = readDirImpl;
            m_currentNode.Flags = NodeFlags.DIRECTORY;

            mp.Node = m_currentNode;

            VFS.AddMountPoint(mp);

            NetworkInfoFS.Init();
            ARPFS.Init();
        }

        /// <summary>
        /// Generate hash from string
        /// </summary>
        /// <param name="inVal">Name to get hash from</param>
        /// <returns>The hash</returns>
        public static long GenerateHash(string inVal)
        {
            long hash = 0;

            // There can be 8 chars before the NULL-character
            for (int i = 0; i <= 8; i++)
            {
                char c = inVal[i];
                if (c == '\0')
                    break;

                hash <<= 3;
                hash |= c;
            }

            return hash;
        }

        /// <summary>
        /// Register device in devices
        /// </summary>
        /// <param name="dev">The device</param>
        public unsafe static void RegisterDevice(Device dev)
        {
            m_devices.Add(GenerateHash(dev.Name), dev);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            long hash = GenerateHash(name);

            Device dev = (Device)m_devices.GetByKey(hash);
            if (dev == null)
                return null;
            
            return dev.node;
        }

        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= m_devices.Count())
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
