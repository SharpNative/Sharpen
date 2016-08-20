namespace Sharpen.FileSystem
{
    public enum DT_Type
    {
        DT_UNKNOWN = 0,
        DT_FIFO = 1,
        DT_CHR = 2,
        DT_DIR = 4,
        DT_BLK = 6,
        DT_REG = 8,
        DT_LNK = 10,
        DT_SOCK = 12,
        DT_WHT = 14
    }

    public struct DirEntry
    {
        public uint Ino;
        public int Offset;
        public byte Type;
        public ushort Reclen;

        public unsafe fixed char Name[256];
    }
}
