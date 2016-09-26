using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    unsafe class Fat16
    {

        private const int FirstPartitonEntry = 0x1BE;

        private const int ENTRYACTIVE = 0;
        private const int ENTRYBEGINHEAD = 0x01;
        private const int ENTRYBEGINCYLSEC = 0x02;
        private const int ENTRYTYPE = 0x04;
        private const int ENTRYENDHEAD = 0x05;
        private const int ENTRYENDCYLSEC = 0x06;
        private const int ENTRYNUMSECTORSBETWEEN = 0x08;
        private const int ENTRYNUMSECTORS = 0x0C;

        private const int FAT_FREE = 0x00;
        private const int FAT_EOF = 0xFFF8;

        private static Node m_dev;
        private static int m_bytespersector;
        private static int m_beginLBA;
        private static int m_clusterBeginLBA;
        private static int m_beginDataLBA;


        private static Fat16BPB* m_bpb;
        private static FatDirEntry* m_dirEntries;
        private static uint m_numDirEntries;

        private static uint m_sectorOffset;

        private const byte LFN = 0x0F;

        private const int ATTRIB_READONLY = 0x1;
        private const int ATTRIB_HIDDEN = 0x2;
        private const int ATTRIB_SYSFILE = 0x4;
        private const int ATTRIB_VOLUMELABEL = 0x8;
        private const int ATTRIB_SUBDIR = 0x10;
        private const int ATTRIB_ARCHIVE = 0x20;


        private static unsafe void initFAT(Node dev)
        {
            // Read first sector
            byte[] firstSector = new byte[512];
            firstSector[0x00] = 0xFF;
            dev.Read(dev, 0, 512, firstSector);


            // Get partition type from first entry
            // Detect if FAT16
            if (firstSector[FirstPartitonEntry + ENTRYTYPE] != 0x06)
                return;

            /*
             * 
             * LBA = (C × HPC + H) × SPT + (S - 1)
             * C, H and S are the cylinder number, the head number, and the sector number
             * LBA is the logical block address
             * HPC is the maximum number of heads per cylinder (reported by disk drive, typically 16 for 28-bit LBA)
             * SPT is the maximum number of sectors per track (reported by disk drive, typically 63 for 28-bit LBA)
             * 
             */
            //m_beginLBA = (cylinder * hpc + BeginHead) * spt + (Sector - 1);
            int off = FirstPartitonEntry + ENTRYNUMSECTORSBETWEEN;
            m_beginLBA = firstSector[off + 3] << 24 | firstSector[off + 2] << 16 | firstSector[off + 1] << 8 | firstSector[off];


            byte[] bootSector = new byte[512];
            dev.Read(dev, (uint)m_beginLBA, 512, bootSector);

            byte* bootPtr = (byte*)Util.ObjectToVoidPtr(bootSector);

            // Avoid that the GC cleans this update (for in the future) and convert to struct
            byte* bpb = (byte*)Heap.Alloc(90);
            Memory.Memcpy(bpb, bootPtr, 90);
            m_bpb = (Fat16BPB*)bpb;

            parseBoot();
        }

        private static unsafe void parseBoot()
        {
            m_clusterBeginLBA = m_beginLBA + m_bpb->ReservedSectors + (m_bpb->NumFats * (int)m_bpb->SectorsPerFat16);

            byte[] buffer = new byte[512];
            m_dev.Read(m_dev, (uint)(m_clusterBeginLBA), 512, buffer);

            m_dirEntries = (FatDirEntry*)Heap.Alloc(m_bpb->NumDirEntries * sizeof(FatDirEntry));


            FatDirEntry* curBufPtr = (FatDirEntry*)Util.ObjectToVoidPtr(buffer);
            int sectorOffset = 0;
            int offset = 0;
            for (int i = 0; i < m_bpb->NumDirEntries; i++)
            {
                // 512 / sizeof fatentry == 16
                if (offset == 16)
                {
                    sectorOffset++;
                    m_dev.Read(m_dev, (uint)(m_clusterBeginLBA + sectorOffset), 512, buffer);

                    offset = 0;
                }

                // TODO: I think this can be overriden 
                m_dirEntries[i] = curBufPtr[offset];

                offset++;
            }

            m_numDirEntries = m_bpb->NumDirEntries;
            m_beginDataLBA = m_clusterBeginLBA + ((m_bpb->NumDirEntries * 32) / m_bpb->BytesPerSector);
        }

        public static uint Data_clust_to_lba(uint cluster)
        {
            return (uint)(m_beginDataLBA + (cluster - 2) * m_bpb->SectorsPerCluster);
        }


        public static unsafe void Init(Node dev, string name)
        {
            m_dev = dev;

            initFAT(dev);

            MountPoint p = new MountPoint();
            p.Name = name;
            p.Node = new Node();
            p.Node.ReadDir = readDirImpl;
            p.Node.FindDir = findDirImpl;
            p.Node.Cookie = 0xFFFFFFFF;
            p.Node.Flags = NodeFlags.DIRECTORY;

            VFS.AddMountPoint(p);
        }

        public static Node CreateNode(FatDirEntry* dirEntry)
        {
            Node node = new Node();
            node.Size = dirEntry->Size;
            node.Cookie = (uint)dirEntry;

            if ((dirEntry->Attribs & ATTRIB_SUBDIR) == 0)
            {
                node.Read = readImpl;
                node.Write = writeImpl;
                node.Flags = NodeFlags.FILE;
            }
            else
            {
                node.ReadDir = readDirImpl;
                node.FindDir = findDirImpl;
                node.Flags = NodeFlags.DIRECTORY;
            }

            return node;
        }

        private static Node findDirImpl(Node node, string name)
        {
            int length = String.Length(name);
            if (length > 12)
                return null;
            int dot = String.IndexOf(name, ".");
            if (dot > 8)
                return null;


            char* testFor = (char*)Heap.Alloc(11);
            Memory.Memset(testFor, ' ', 11);

            for (int i = 0; i < 8; i++)
            {
                if ((dot > 0 && i < length && i < dot) || (dot == -1 && i < length))
                    testFor[i] = String.ToUpper(name[i]);
                else
                    testFor[i] = ' ';

            }

            if (dot != -1)
            {
                int lengthExt = length - dot - 1;

                for (int i = 0; i < 3; i++)
                {
                    if (i < lengthExt)
                        testFor[i + 8] = String.ToUpper(name[i + dot + 1]);
                    else
                        testFor[i + 8] = ' ';
                }
            }

            uint cluster = 0xFFFFFFFF;

            if (node.Cookie != 0xFFFFFFFF)
            {
                FatDirEntry* entry = (FatDirEntry*)node.Cookie;

                cluster = entry->ClusterNumberLo;
            }

            SubDirectory dir = readDirectory(cluster);
            Node nd = FindFileInDirectory(dir, testFor);

            Heap.Free(testFor);
            return nd;
        }


        public static Node FindFileInDirectory(SubDirectory dir, char* testFor)
        {
            for (int i = 0; i < dir.Length; i++)
            {
                FatDirEntry entry = dir.DirEntries[i];

                if (entry.Name[0] == 0 || entry.Name[0] == 0xE5 || entry.Attribs == 0xF || (entry.Attribs & 0x08) > 0)
                    continue;

                FatDirEntry* entr = (FatDirEntry*)Heap.Alloc(sizeof(FatDirEntry));
                Memory.Memcpy(entr, dir.DirEntries + i, sizeof(FatDirEntry));

                if (Memory.Compare(testFor, entry.Name, 11))
                {

                    return CreateNode(entr);
                }
            }

            return null;
        }

        private static DirEntry* readDirImpl(Node node, uint index)
        {
            /*if (index > m_numDirEntries)
                return null;
                */


            int j = 0;

            uint cluster = 0xFFFFFFFF;

            if (node.Cookie != 0xFFFFFFFF)
            {
                FatDirEntry* entry = (FatDirEntry*)node.Cookie;

                cluster = entry->ClusterNumberLo;
            }

            SubDirectory dir = readDirectory(cluster);

            for (int i = 0; i < dir.Length; i++)
            {
                FatDirEntry entry = dir.DirEntries[i];

                if (entry.Name[0] == 0 || entry.Name[0] == (char)0xE5 || entry.Attribs == 0xF || (entry.Attribs & 0x08) > 0)
                    continue;

                if (j >= index)
                {
                    DirEntry* outDir = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
                    outDir->Reclen = (ushort)sizeof(DirEntry);

                    int fnLength = String.IndexOf(Util.CharPtrToString(entry.Name), " ");

                    if (fnLength > 8 || fnLength == -1)
                        fnLength = 8;

                    int offset = 0;
                    for (int z = 0; z < fnLength; z++)
                        outDir->Name[offset++] = entry.Name[z];

                    if ((dir.DirEntries[i].Attribs & ATTRIB_SUBDIR) == 0)
                    {

                        int extLength = String.IndexOf(Util.CharPtrToString(entry.Name + 8), " ");
                        if (extLength == -1)
                            extLength = 3;

                        if (extLength != 0)
                        {
                            outDir->Name[offset++] = '.';


                            for (int z = 0; z < extLength; z++)
                                outDir->Name[offset++] = entry.Name[z + 8];
                        }

                        outDir->Type = (byte)DT_Type.DT_REG;
                    }
                    else
                    {
                        outDir->Type = (byte)DT_Type.DT_DIR;
                    }



                    outDir->Name[offset] = '\0';

                    for (int z = 0; z < offset; z++)
                        outDir->Name[z] = String.ToLower(outDir->Name[z]);

                    return outDir;
                }

                j++;
            }

            return null;
        }

        /// <summary>
        /// Find next cluster in file
        /// </summary>
        /// <param name="cluster">Cluster number</param>
        /// <returns></returns>
        private static unsafe uint FindNextCluster(uint cluster)
        {
            int beginFat = m_beginLBA + m_bpb->ReservedSectors;
            uint clusters = (cluster / 256);
            uint adr = (uint)(beginFat + clusters);
            uint offset = (cluster * 2) - (clusters * 512);


            byte[] fatBuffer = new byte[512];
            m_dev.Read(m_dev, adr, 512, fatBuffer);

            byte* ptr = (byte*)Util.ObjectToVoidPtr(fatBuffer);
            ushort* pointer = (ushort*)(ptr + offset);


            ushort nextCluster = *pointer;

            if (nextCluster >= FAT_EOF)
                return 0xFFFF;

            return nextCluster;
        }

        /// <summary>
        /// Find next free sector in FAT
        /// </summary>
        /// <returns></returns>
        private static uint FirstFirstFreeSector()
        {
            //for (uint i = 0; i < m_bpb->SectorsPerFat16; i++)
            //{
            //    ushort nextCluster = m_fat[i];

            //    if (nextCluster == FAT_FREE)
            //        return i;
            //}

            // FULL!!! :O
            return 0;
        }

        private static uint writeFile(uint startCluster, uint offset, uint size, byte[] buffer)
        {
            // Calculate starting cluster
            uint dataPerCluster = m_bpb->SectorsPerCluster;
            uint sectorsOffset = (uint)((int)offset / 512);

            uint clusterOffset = sectorsOffset / dataPerCluster;
            

            return 0;
        }

        private static uint readFile(uint startCluster, uint offset, uint size, byte[] buffer)
        {
            // Calculate starting cluster
            uint dataPerCluster = m_bpb->SectorsPerCluster;
            uint sectorsOffset = (uint)((int)offset / 512);

            uint clusterOffset = sectorsOffset / dataPerCluster;

            if (clusterOffset > 0)
            {
                for (int i = 0; i < clusterOffset; i++)
                {
                    startCluster = FindNextCluster(startCluster);

                    if (startCluster == 0xFFFF)
                        return 0;
                }
            }

            uint StartOffset = offset - (sectorsOffset * 512);
            sectorsOffset = sectorsOffset - (clusterOffset * m_bpb->SectorsPerCluster);

            // Read starting cluster
            byte[] buf = new byte[512];
            m_dev.Read(m_dev, Data_clust_to_lba(startCluster), 512, buf);

            // Calculate size in sectors
            uint sizeInSectors = size / 512;
            if (sizeInSectors == 0)
                sizeInSectors++;

            uint offsetInCluster = sectorsOffset;
            uint offsetInSector = StartOffset;
            uint currentCluster = startCluster;
            uint currentOffset = 0;
            int sizeLeft = (int)size;
            
            for (int i = 0; i < sizeInSectors; i++)
            {
                if (offsetInCluster == m_bpb->SectorsPerCluster)
                {
                    currentCluster = FindNextCluster(currentCluster);

                    if (currentCluster == 0xFFFF)
                        return currentOffset;
                    
                    offsetInCluster = 0;
                }
                
                m_dev.Read(m_dev, Data_clust_to_lba(currentCluster) + offsetInCluster, 512, buf);

                int sizeTemp = (sizeLeft > 512) ? 512 : sizeLeft;
                int sizeLeftinSector = 512;
                sizeLeftinSector -= (int)offsetInSector;
                if (sizeLeft > sizeLeftinSector)
                {
                    sizeTemp = sizeLeftinSector;
                    sizeInSectors++;
                }
                
                Memory.Memcpy((void*)((int)Util.ObjectToVoidPtr(buffer) + currentOffset), (void*)((int)Util.ObjectToVoidPtr(buf) + offsetInSector), sizeTemp);
                currentOffset += (uint)sizeTemp;
                sizeLeft -= sizeTemp;
                offsetInCluster++;
                offsetInSector = 0;
            }
            
            return size;
        }

        public static SubDirectory readDirectory(uint cluster)
        {
            SubDirectory outDir = new SubDirectory();

            if (cluster == 0xFFFFFFFF)
            {
                outDir.Length = m_numDirEntries;
                outDir.DirEntries = m_dirEntries;
            }
            else
            {
                byte[] buffer = new byte[m_bpb->NumDirEntries * sizeof(FatDirEntry)];
                readFile(cluster, 0, (uint)(m_bpb->NumDirEntries * sizeof(FatDirEntry)), buffer);

                FatDirEntry* entries = (FatDirEntry*)Heap.Alloc(m_bpb->NumDirEntries * sizeof(FatDirEntry));

                FatDirEntry* curBufPtr = (FatDirEntry*)Util.ObjectToVoidPtr(buffer);

                int length = 0;
                for (int i = 0; i < m_bpb->NumDirEntries; i++)
                {
                    entries[i] = curBufPtr[i];

                    if (curBufPtr[i].Name[0] == 0x00)
                    {
                        break;

                    }

                    length++;
                }

                outDir.DirEntries = entries;
                outDir.Length = (uint)length;
            }

            return outDir;
        }

        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            FatDirEntry* entry = (FatDirEntry*)node.Cookie;
            
            // If the offset is behind the size, stop here
            if (offset > entry->Size)
                return 0;

            // If bytes to read is bigger than the file size, set the size to the file size minus offset
            if (offset + size > entry->Size)
                size = entry->Size - offset;

            return readFile(entry->ClusterNumberLo, offset, size, buffer);
        }

        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            FatDirEntry* entry = (FatDirEntry*)node.Cookie;
            
            return writeFile(entry->ClusterNumberLo, offset, size, buffer);
        }
    }
}
