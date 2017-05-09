using Sharpen.Arch;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    class PciFS
    {
        struct PciFSInfo
        {
            public ushort Bus;
            public ushort Slot;
            public ushort Function;

            public byte ClassCode;
            public byte SubClass;
            public byte ProgIntf;

            public ushort Vendor;
            public ushort Device;
        }

        private static ContainerFS m_container;

        /// <summary>
        /// Initializes PciFS
        /// </summary>
        public static unsafe void Init()
        {
            m_container = new ContainerFS();
            RootPoint dev = new RootPoint("pci", m_container.Node);
            VFS.MountPointDevFS.AddEntry(dev);
        }
        
        /// <summary>
        /// Load devices
        /// </summary>
        public static unsafe void LoadDevices()
        {
            for (int i = 0; i < Pci.DeviceNum; i++)
            {
                PciDevice dev = Pci.Devices[i];
                string name = GenerateNodeName(dev.Bus, dev.Slot, dev.Function);

                Node node = new Node();
                node.Size = (uint)sizeof(PciFSInfo);
                node.Read = readImpl;
                node.Flags = NodeFlags.FILE | NodeFlags.DEVICE;
                node.Cookie = new PciDeviceCookie(dev);

                RootPoint point = new RootPoint(name, node);
                m_container.AddEntry(point);
            }
        }

        /// <summary>
        /// Generate node name from bus/slot/function
        /// </summary>
        /// <param name="bus">The bus</param>
        /// <param name="slot">The slot</param>
        /// <param name="function">The function</param>
        /// <returns>The node name</returns>
        public static unsafe string GenerateNodeName(int bus, int slot, int function)
        {
            string part1 = bus.ToString();
            string part2 = slot.ToString();
            string part3 = function.ToString();

            char* ptr = (char*)Heap.Alloc(10);
            int x = 0;
            for (int j = 0; j < part1.Length; j++)
                ptr[x++] = part1[j];
            ptr[x++] = ':';
            for (int j = 0; j < part2.Length; j++)
                ptr[x++] = part2[j];
            ptr[x++] = ':';
            for (int j = 0; j < part3.Length; j++)
                ptr[x++] = part3[j];
            ptr[x] = '\0';

            Heap.Free(part1);
            Heap.Free(part2);
            Heap.Free(part3);

            return Util.CharPtrToString(ptr);
        }
        
        /// <summary>
        /// Read function info
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            PciDevice dev = ((PciDeviceCookie)node.Cookie).Device;
            if (dev == null)
                return 0;

            /**
             * Generate info copy struct
             */
            PciFSInfo info = new PciFSInfo();
            info.Bus = dev.Bus;
            info.Slot = dev.Slot;
            info.Function = dev.Function;

            info.ClassCode = dev.ClassCode;
            info.SubClass = dev.SubClass;
            info.ProgIntf = dev.ProgIntf;

            info.Vendor = dev.Vendor;
            info.Device = dev.Device;
            
            if (size > sizeof(PciFSInfo))
                size = (uint)sizeof(PciFSInfo);

            Memory.Memcpy(Util.ObjectToVoidPtr(buffer), &info, (int)size);

            return size;
        }
    }
}
