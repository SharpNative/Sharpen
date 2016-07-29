using Sharpen.Collections;

namespace Sharpen.FileSystem
{
    class VFS
    {
        private MountDictionary m_dictionary = new MountDictionary();

        public VFS()
        {

        }

        /// <summary>
        /// Generate hash from string
        /// </summary>
        /// <param name="inVal">Name</param>
        /// <returns></returns>
        private long GenerateHash(string inVal)
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
        public void AddMountPoint(MountPoint mountPoint)
        {

            long key = GenerateHash(mountPoint.Name);

            Console.Write("Put in index: ");
            Console.Write(mountPoint.Name);
            Console.Write(" ");
            Console.WriteHex(key);
            Console.PutChar('\n');
            m_dictionary.Add(key, mountPoint);
        }

        /// <summary>
        /// Find mount in VFS
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public unsafe MountPoint FindMountByName(string name)
        {
            long key = GenerateHash(name);

            Console.Write("Looking for ");
            Console.WriteHex(key);
            Console.PutChar('\n');

            return m_dictionary.GetByKey(key);
        }
    }
}
