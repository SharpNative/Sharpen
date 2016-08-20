using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Other
{
    class VboxDevFSDriver
    {
        private const int m_numCommands = 3;
        public static readonly string[] m_commands =
        {
            "sessionid",
            "powerstate",
            "hosttime"
        };

        /// <summary>
        /// Initializes the Filesystem node for VboxDev
        /// </summary>
        public static unsafe void Init()
        {
            Device device = new Device();
            device.Name = "VMMDEV";
            device.node = new Node();
            device.node.Flags = NodeFlags.DIRECTORY | NodeFlags.DEVICE;
            device.node.FindDir = findDirImpl;
            device.node.ReadDir = readDirImpl;

            DevFS.RegisterDevice(device);

            Console.WriteLine("[VMMDev] FsDevice registered under VMMDEV");
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
            uint functionID = 0;
            if (String.Equals(name, "sessionid"))
            {
                functionID = (uint)VboxDevRequestTypes.VMMDevReq_GetSessionId;
            }
            else if(String.Equals(name, "powerstate"))
            {
                functionID = (uint)VboxDevRequestTypes.VMMDevReq_SetPowerStatus;
            }
            else if (String.Equals(name, "hosttime"))
            {
                functionID = (uint)VboxDevRequestTypes.VMMDevReq_GetHostTime;
            }

            if (functionID == 0)
                return null;

            Node outNode = new Node();
            outNode.Cookie = functionID;
            outNode.Read = readImpl;
            outNode.Write = writeImpl;
            outNode.Flags = NodeFlags.FILE;

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
            VboxDevRequestTypes function = (VboxDevRequestTypes)node.Cookie;

            switch (function)
            {
                case VboxDevRequestTypes.VMMDevReq_SetPowerStatus:
                    if (size < 4)
                        return 0;

                    int state = ByteUtil.ToInt(buffer);
                    VboxDevPowerState stateConverted = (VboxDevPowerState)state;

                    VboxDev.ChangePowerState(stateConverted);

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
            VboxDevRequestTypes function = (VboxDevRequestTypes)node.Cookie;

            switch (function)
            {
                case VboxDevRequestTypes.VMMDevReq_GetSessionId:
                    if (size != 8)
                        return 0;

                    ulong sessionID = VboxDev.GetSessionID();

                    ByteUtil.ToBytes((long)sessionID, buffer);

                    return 8;

                case VboxDevRequestTypes.VMMDevReq_GetHostTime:
                    if (size != 8)
                        return 0;

                    ulong time = VboxDev.GetHostTime();

                    ByteUtil.ToBytes((long)time, buffer);

                    return 8;
            }

            return 0;
        }
    }
}
