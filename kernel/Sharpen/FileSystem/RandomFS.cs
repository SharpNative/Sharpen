using Sharpen.Lib;

namespace Sharpen.FileSystem
{
    class RandomFS
    {
        /// <summary>
        /// Initializes Null device
        /// </summary>
        public unsafe static void Init()
        {
            Node node = new Node();
            node.Read = readImpl;
            node.Size = 0xFFFFFFFF;

            RootPoint dev = new RootPoint("random", node);
            VFS.MountPointDevFS.AddEntry(dev);
        }

        /// <summary>
        /// Read from null device
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private unsafe static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            uint left = size;
            uint i = 0;
            
            int rand;

            /*
             * Fill buffer 4 bytes at a time
             */ 
            while (left >= 4)
            {
                rand = Random.Rand();

                buffer[i] = (byte)(rand & 0xFF);
                buffer[i + 1] = (byte)((rand >> 8) & 0xFF);
                buffer[i + 2] = (byte)((rand >> 16) & 0xFF);
                buffer[i + 3] = (byte)((rand >> 24) & 0xFF);

                left -= 4;
                i += 4;
            }
            
            /**
             * Fill leftout bytes
             */ 
            rand = Random.Rand();

            if (left >= 1)
                buffer[i] = (byte)(rand & 0xFF);

            if (left >= 2)
                buffer[i + 1] = (byte)((rand >> 8) & 0xFF);
            
            if (left >= 3)
                buffer[i + 2] = (byte)((rand >> 16) & 0xFF);
            
            if (left >= 4)
                buffer[i + 3] = (byte)((rand >> 24) & 0xFF);

            return size;
        }
    }
}
