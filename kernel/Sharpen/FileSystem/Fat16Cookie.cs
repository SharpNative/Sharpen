namespace Sharpen.FileSystem
{
    public class Fat16Cookie : ICookie
    {
        public unsafe FatDirEntry* DirEntry;
        public uint Cluster;
        public uint Num;

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
