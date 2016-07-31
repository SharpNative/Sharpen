using Sharpen.Collections;

namespace Sharpen.FileSystem
{
    class VFS
    {
        private static MountDictionary m_dictionary = new MountDictionary();

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
        /// Add mount to VFS
        /// </summary>
        /// <param name="mountPoint"></param>
        public static void AddMountPoint(MountPoint mountPoint)
        {
            long key = GenerateHash(mountPoint.Name);
            m_dictionary.Add(key, mountPoint);
        }

        /// <summary>
        /// Find mount in VFS
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MountPoint FindMountByName(string name)
        {
            long key = GenerateHash(name);
            return m_dictionary.GetByKey(key);
        }

        /// <summary>
        /// Get node by path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns></returns>
        public static Node GetByPath(string path)
        {
            int index = String.indexOf(path, "://");
            if (index == -1)
                return null;

            if (path[String.Length(path) - 1] != '/')
                path = String.Merge(path, "/");

            string deviceName = String.Substring(path, 0, index);
            string AfterDeviceName = String.Substring(path, index + 3, String.Length(path) - (index + 3));
            int parts = String.Count(AfterDeviceName, '/');

            // Find first mount
            MountPoint mp = FindMountByName(deviceName);

            if (mp == null)
                return null;

            Node lastNode = mp.Node;
            // TODO: Optimize this process!
            string nodeName = AfterDeviceName;
            string afterNodeName = AfterDeviceName;
            while (parts > 0)
            {
                index = String.indexOf(afterNodeName, "/");
                nodeName = String.Substring(afterNodeName, 0, index);
                afterNodeName = String.Substring(afterNodeName, index + 1, String.Length(path) - 1);

                lastNode = lastNode.FindDir(lastNode, nodeName);
                if (lastNode == null)
                    return null;

                parts--;
            }

            return lastNode;
        }
    }
}
