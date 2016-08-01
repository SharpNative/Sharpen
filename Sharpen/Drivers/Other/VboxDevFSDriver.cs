using Sharpen.FileSystem;
using Sharpen.Utilities;

namespace Sharpen.Drivers.Other
{
    class VboxDevFSDriver
    {
        private static readonly int num_commands = 3;
        public static readonly string[] commands =
        {
            "sessionid",
            "powerstate",
            "hosttime"
        };

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

        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            if (index >= num_commands)
                return null;


            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));

            int i = 0;
            for (; commands[index][i] != '\0'; i++)
                entry->Name[i] = commands[index][i];
            entry->Name[i] = '\0';

            return entry;
        }

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
