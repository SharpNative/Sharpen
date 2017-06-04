using Sharpen.FileSystem.Filesystems;

namespace Sharpen.FileSystem.Cookie
{
    public class Fat16Cookie : ICookie
    {
        public unsafe FatDirEntry* DirEntry;
        public uint Cluster;
        public uint Num;

        public Fat16 FAT16;
        
        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
