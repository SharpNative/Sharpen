using Sharpen.FileSystem;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Other
{
    class VboxDevFSDriver
    {
        public const int NumCommands = 3;
        public static readonly string[] CommandNames =
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
            device.Node = new Node();
            device.Node.Flags = NodeFlags.DIRECTORY | NodeFlags.DEVICE;
            device.Node.FindDir = findDirImpl;
            device.Node.ReadDir = readDirImpl;

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
            if (index >= NumCommands)
                return null;
            
            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int i = 0;
            for (; CommandNames[index][i] != '\0'; i++)
                entry->Name[i] = CommandNames[index][i];
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
            VboxDevRequestTypes function = VboxDevRequestTypes.VMMDevReq_InvalidRequest;
            if (name.Equals("sessionid"))
            {
                function = VboxDevRequestTypes.VMMDevReq_GetSessionId;
            }
            else if(name.Equals("powerstate"))
            {
                function = VboxDevRequestTypes.VMMDevReq_SetPowerStatus;
            }
            else if (name.Equals("hosttime"))
            {
                function = VboxDevRequestTypes.VMMDevReq_GetHostTime;
            }

            if (function == VboxDevRequestTypes.VMMDevReq_InvalidRequest)
                return null;

            Node outNode = new Node();
            outNode.Read = readImpl;
            outNode.Write = writeImpl;
            outNode.Flags = NodeFlags.FILE;

            IDCookie cookie = new IDCookie((int)function);
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
            VboxDevRequestTypes request = (VboxDevRequestTypes)cookie.ID;

            switch (request)
            {
                case VboxDevRequestTypes.VMMDevReq_SetPowerStatus:
                    if (size < 4)
                        return 0;

                    int state = Byte.ToInt(buffer);
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
            IDCookie cookie = (IDCookie)node.Cookie;
            VboxDevRequestTypes request = (VboxDevRequestTypes)cookie.ID;

            switch (request)
            {
                case VboxDevRequestTypes.VMMDevReq_GetSessionId:
                    if (size != 8)
                        return 0;

                    ulong sessionID = VboxDev.GetSessionID();

                    Byte.ToBytes((long)sessionID, buffer);

                    return 8;

                case VboxDevRequestTypes.VMMDevReq_GetHostTime:
                    if (size != 8)
                        return 0;

                    ulong time = VboxDev.GetHostTime();

                    Byte.ToBytes((long)time, buffer);

                    return 8;
            }

            return 0;
        }
    }
}
