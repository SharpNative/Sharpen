namespace Sharpen.FileSystem
{
    public struct DirEntry
    {
        public uint Ino;
        public int Offset;
        public byte Type;
        public ushort Reclen;

        public unsafe fixed char Name[256];
    }
}
