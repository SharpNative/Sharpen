namespace Sharpen.FileSystem
{
    public struct DirEntry
    {
        public uint Ino;
        public int Offset;
        public ushort Reclen;
        public byte Type;

        public unsafe fixed char Name[256];
    }
}
