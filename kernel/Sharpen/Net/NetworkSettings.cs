namespace Sharpen.Net
{
    public unsafe struct NetworkSettings
    {
        public fixed byte IP[4];
        public fixed byte Subnet[4];
        public fixed byte Gateway[4];
        public fixed byte DNS1[4];
        public fixed byte DNS2[4];
        public fixed byte ServerID[4];
    }
}
