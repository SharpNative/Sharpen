using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    unsafe class Fat16
    {

        private static readonly int FirstPartitonEntry = 0x1BE;

        private static readonly int ENTRYACTIVE = 0;
        private static readonly int ENTRYBEGINHEAD = 0x01;
        private static readonly int ENTRYBEGINCYLSEC = 0x02;
        private static readonly int ENTRYTYPE = 0x04;
        private static readonly int ENTRYENDHEAD = 0x05;
        private static readonly int ENTRYENDCYLSEC = 0x06;
        private static readonly int ENTRYNUMSECTORSBETWEEN = 0x08;
        private static readonly int ENTRYNUMSECTORS = 0x0C;

        private static readonly int FAT_FREE = 0x00;
        private static readonly int FAT_EOF = 0xFF8;

        private static Node m_dev;
        private static int m_bytespersector;
        private static int m_beginLBA;
        private static int m_clusterBeginLBA;
        private static int m_beginDataLBA;


        private static Fat16BPB* m_bpb;
        private static FatDirEntry[] m_dirEntries;
        private static uint m_numDirEntries;
        private static ushort[] m_fat;

        private static readonly byte LFN = 0x0F;
        

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
            
            byte BeginHead = firstSector[FirstPartitonEntry + ENTRYBEGINHEAD];
            byte Sector = (byte)(firstSector[FirstPartitonEntry + ENTRYBEGINCYLSEC] & 0x3F);

            int tmp = firstSector[FirstPartitonEntry + ENTRYBEGINCYLSEC] >> 8;
            byte cylinderHi = (byte)(tmp & 0x3);
            byte cylinderLo = firstSector[FirstPartitonEntry + ENTRYBEGINCYLSEC + 1];

            short cylinder = (short)(cylinderHi << 10 | cylinderLo);


            // TODO: Get this from the drive
            int hpc = 16;
            int spt = 63;

            /*
             * 
             * LBA = (C × HPC + H) × SPT + (S - 1)
             * C, H and S are the cylinder number, the head number, and the sector number
             * LBA is the logical block address
             * HPC is the maximum number of heads per cylinder (reported by disk drive, typically 16 for 28-bit LBA)
             * SPT is the maximum number of sectors per track (reported by disk drive, typically 63 for 28-bit LBA)
             * 
             */
            m_beginLBA = (cylinder * hpc + BeginHead) * spt + (Sector - 1);

            byte[] bootSector = new byte[512];
            dev.Read(dev, (uint)m_beginLBA, 512, bootSector);

            byte* bootPtr = (byte*) Util.ObjectToVoidPtr(bootSector);

            // Avoid that the GC cleans this update (for in the future) and convert to struct
            byte* bpb = (byte *)Heap.Alloc(90);
            Memory.Memcpy(bpb, bootPtr, 90);
            m_bpb = (Fat16BPB*)bpb;
            
            parseBoot();
        }

        private static unsafe void parseBoot()
        {
            m_clusterBeginLBA = m_beginLBA + m_bpb->ReservedSectors + (m_bpb->NumFats * (int)m_bpb->SectorsPerFat16);

            byte[] buffer = new byte[512];
            m_dev.Read(m_dev, (uint)(m_clusterBeginLBA), 512, buffer);

            m_dirEntries = new FatDirEntry[m_bpb->NumDirEntries];
            

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

            // Read FAT in memory
            int beginFat = m_beginLBA + m_bpb->ReservedSectors;
            uint size = m_bpb->SectorsPerFat16;
            
            m_fat = new ushort[size];

            byte[] fatBuffer = new byte[512];
            m_dev.Read(m_dev, (uint)(beginFat), 512, fatBuffer);


            ushort* fatBufPtr = (ushort*)Util.ObjectToVoidPtr(fatBuffer);

            sectorOffset = 0;
            offset = 0;
            for (int i = 0; i < size; i++)
            {
                // 512 / sizeof fatentry == 16
                if (offset == 256)
                {
                    sectorOffset++;
                    m_dev.Read(m_dev, (uint)(beginFat + sectorOffset), 1, fatBuffer);

                    offset = 0;
                }
                
                m_fat[i] = fatBufPtr[offset];

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
            p.Name = "C";
            p.Node = new Node();
            p.Node.ReadDir = readDirImpl;
            p.Node.FindDir = findDirImpl;

            VFS.AddMountPoint(p);
        }

        public static Node CreateNode(int dirEntry)
        {
            Node node = new Node();
            node.Cookie = (uint)dirEntry;
            node.Read = readImpl;
            node.Write = writeImpl;

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
            
            for(int i = 0; i < 8; i++)
            {
                if ((dot > 0 && i < length && i < dot) || (dot == -1 && i < length))
                    testFor[i] = String.ToUpper(name[i]);
                else
                    testFor[i] = ' ';
               
            }

            if(dot != -1)
            {
                int lengthExt = length - dot - 1;

                for(int i = 0; i < 3; i++)
                {
                    if (i < lengthExt)
                        testFor[i + 8] = String.ToUpper(name[i + dot + 1]);
                    else
                        testFor[i + 8] = ' '; 
                }
            }

            SubDirectory dir = new SubDirectory();
            dir.Length = m_numDirEntries;
            dir.DirEntries = m_dirEntries;

            Node nd = FindFileInDirectory(dir, testFor);
            
            Heap.Free(testFor);
            return nd;
        }


        public static Node FindFileInDirectory(SubDirectory dir, char *testFor)
        {
            for (int i = 0; i < dir.Length; i++)
            {
                FatDirEntry entry = dir.DirEntries[i];

                if (entry.Name[0] == 0 || entry.Name[0] == 0xE5 || entry.Attribs == 0xF || (entry.Attribs & 0x08) > 0)
                    continue;

                if (Memory.Compare(testFor, entry.Name, 11))
                {
                    return CreateNode(i);
                }
            }

            return null;
        }

        private static DirEntry* readDirImpl(Node node, uint index)
        {
            if (index > m_numDirEntries)
                return null;

            int j = 0;

            for (int i = 0; i < m_numDirEntries; i++)
            {
                FatDirEntry entry = m_dirEntries[i];

                if (entry.Name[0] == 0 || entry.Name[0] == 0xE5 || entry.Attribs == 0xF || (entry.Attribs & 0x08) > 0)
                    continue;

                if (j >= index)
                {
                    DirEntry* dir = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
                    Memory.Memcpy(dir->Name, entry.Name, 11);
                    dir->Name[11] = '\0';


                    return dir;
                }

                j++;
            }

            return null;
        }

        /// <summary>
        /// Find next clust
        /// </summary>
        /// <param name="cluster">Cluster number</param>
        /// <returns></returns>
        private static uint FindNextCluster(uint cluster)
        {
            ushort nextCluster = m_fat[cluster];
            
            // End of file?
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
            for(uint i = 0; i < m_bpb->SectorsPerFat16; i++)
            {
                ushort nextCluster = m_fat[i];

                if (nextCluster == FAT_FREE)
                    return i;
            }

            // FULL!!! :O
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

            sectorsOffset = sectorsOffset - (clusterOffset * m_bpb->SectorsPerCluster);
            uint StartOffset = offset - (sectorsOffset * 512);

            // Read starting cluster
            byte[] buf = new byte[512];
            m_dev.Read(m_dev, Data_clust_to_lba(startCluster), 512, buf);

            // Calculate size in sectors
            uint sizeInSectors = size / 512;
            if (sizeInSectors == 0)
                sizeInSectors++;

            uint offsetInCluser = sectorsOffset;
            uint offsetInSector = StartOffset;
            uint currentCluster = startCluster;
            uint currentOffset = 0;
            int sizeLeft = (int)size;

            for (int i = 0; i < sizeInSectors; i++)
            {
                if (offsetInCluser == m_bpb->SectorsPerCluster)
                {
                    currentCluster = FindNextCluster(currentCluster);

                    if (currentCluster == 0xFFFF)
                        return currentOffset;

                    offsetInCluser = 0;
                }

                m_dev.Read(m_dev, Data_clust_to_lba(currentCluster) + offsetInCluser, 512, buf);

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
                offsetInCluser++;
                offsetInSector = 0;
            }


            return size;
        }

        public static SubDirectory readDirectory(uint cluster)
        {

            byte[] buffer = new byte[m_bpb->NumDirEntries];
            readFile(cluster, 0, m_bpb->NumDirEntries, buffer);

            FatDirEntry[] entries = new FatDirEntry[m_bpb->NumDirEntries];

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

            SubDirectory subDir = new SubDirectory();
            subDir.DirEntries = entries;
            subDir.Length = (uint)length;

            return subDir;
        }

        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            if (node.Cookie > m_numDirEntries)
                return 0;

            FatDirEntry entry = m_dirEntries[node.Cookie];

            // If bytes to read is bigger than the file size, set the size to the file size minus offset
            if (offset + size > entry.Size)
                size = entry.Size - offset;

            uint startCluster = entry.ClusterNumberLo;

            return readFile(entry.ClusterNumberLo, offset, size, buffer);
        }

        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            return 0;
        }
    }
}
