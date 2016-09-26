namespace Sharpen.FileSystem
{
    public class STDOUT
    {
        /// <summary>
        /// Initialize STDOUT
        /// </summary>
        public static unsafe void Init()
        {
            Device device = new Device();
            device.Name = "stdout";
            device.node = new Node();
            device.node.Write = writeImpl;

            DevFS.RegisterDevice(device);
        }

        /// <summary>
        /// Write to stdout
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            for (int i = 0; i < size; i++)
                Console.Write((char)buffer[i]);
            
            return size;
        }
    }
}
