namespace Sharpen.FileSystem
{
    public struct DirEntry
    {
        public uint Ino;
        public int Offset;
        public byte Type;
        public ushort Reclen;
        public byte Flags; // 0x01 = FILE // 0x02 == DIRECTORY

        public unsafe fixed char Name[256];
    }
}
