using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    class NullFS
    {
        private static Node m_currentNode;

        /// <summary>
        /// Initializes Null device
        /// </summary>
        public unsafe static void Init()
        {
            Device device = new Device();
            device.Name = "null";
            device.Node = new Node();
            device.Node.Read = readImpl;
            device.Node.Write = writeImpl;
            device.Node.Size = 0xFFFFFFFF;

            DevFS.RegisterDevice(device);
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
            Memory.Memclear((char*)Util.ObjectToVoidPtr(buffer) + offset, (int)size);
            return size;
        }

        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            return size;
        }
    }
}
