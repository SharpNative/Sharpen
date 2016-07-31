using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    public class DevFS
    {
        private static DevDictionary m_devices = new DevDictionary();
        private static Node m_currentNode;

        public unsafe static void Init()
        {
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
        /// Generate hash from string
        /// </summary>
        /// <param name="inVal">Name</param>
        /// <returns></returns>
        private static long GenerateHash(string inVal)
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
        /// <param name="dev"></param>
        public unsafe static void RegisterDevice(Device dev)
        {
            m_devices.Add(GenerateHash(dev.Name), dev);
        }

        private static unsafe Node findDirImpl(Node node, string name)
        {
            long hash = GenerateHash(name);
            
            Device dev = m_devices.GetByKey(hash);

            if (dev == null)
                return null;

            return dev.node;
        }

        private static void readDirImpl(Node node, uint index)
        {
            // TODO: List all devices here :)
        }
    }
}
