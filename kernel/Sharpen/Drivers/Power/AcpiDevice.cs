using Sharpen.FileSystem;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;

namespace Sharpen.Drivers.Power
{
    public sealed class AcpiDevice
    {
        private const int m_numCommands = 3;
        public static readonly string[] m_commands =
        {
            "info",
            "reboot",
            "shutdown"
        };

        /// <summary>
        /// Initializes the Filesystem node for VboxDev
        /// </summary>
        public static unsafe void Init()
        {
            Device device = new Device();
            device.Name = "ACPI";
            device.Node = new Node();
            device.Node.Flags = NodeFlags.DIRECTORY | NodeFlags.DEVICE;
            device.Node.FindDir = findDirImpl;
            device.Node.ReadDir = readDirImpl;

            DevFS.RegisterDevice(device);

            Console.WriteLine("[ACPI] Device registered under ACPI");
        }

        /// <summary>
        /// Reads directory entries from the FS
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The current index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= m_numCommands)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int i = 0;
            for (; m_commands[index][i] != '\0'; i++)
                entry->Name[i] = m_commands[index][i];
            entry->Name[i] = '\0';

            return entry;
        }

        /// <summary>
        /// Find dir function
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            int functionID = 0;
            
            if (name.Equals("info"))
            {
                functionID = 1;
            }
            else if (name.Equals("reboot"))
            {
                functionID = 2;
            }
            else if (name.Equals("shutdown"))
            {
                functionID = 3;
            }

            if (functionID == 0)
                return null;

            Node outNode = new Node();
            outNode.Read = readImpl;
            outNode.Write = writeImpl;
            outNode.Flags = NodeFlags.FILE;

            IDCookie cookie = new IDCookie(functionID);
            outNode.Cookie = (ICookie)cookie;

            return outNode;
        }

        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            IDCookie cookie = (IDCookie)node.Cookie;
            int function = cookie.ID;

            switch (function)
            {
                case 2:
                    Acpi.Reboot();

                    return 4;


                case 3:
                    Acpi.Shutdown();

                    return 4;
            }

            return 0;
        }

        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            IDCookie cookie = (IDCookie)node.Cookie;
            int function = cookie.ID;

            switch (function)
            {
                case 1:

                    break;
            }

            return 0;
        }
    }
}
