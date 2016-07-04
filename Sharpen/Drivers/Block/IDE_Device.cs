namespace Sharpen.Drivers.Block
{
    public unsafe struct IDE_Device
    {
        public bool Exists;
        public byte Channel;

        public ushort BasePort;
        public byte Drive;
        public ulong Size;

        public uint CmdSet;
        public ushort Type;
        public ushort Capabilities;

        public ushort Cylinders;
        public ushort Heads;
        public ushort Sectorspt;

        public char* Name;
    }
}
