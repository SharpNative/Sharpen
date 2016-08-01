using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    unsafe class Fat16
    {

        private static readonly int FirstPartitonEntry = 0x1BE;

        private static readonly int EntryActive = 0;
        private static readonly int EntryBeginHead = 0x01;
        private static readonly int EntryBeginCylSec = 0x02;
        private static readonly int EntryType = 0x04;
        private static readonly int EntryEndHead = 0x05;
        private static readonly int EntryEndCylSec = 0x06;
        private static readonly int EntryNumSectorsBetween = 0x08;
        private static readonly int EntryNumSectors = 0x0C;

        private static Node m_dev;
        private static int m_bytespersector;
        private static int m_beginLBA;
        private static int m_clusterBeginLBA;
        private static int m_beginDataLBA;


        private static Fat16BPB* m_bpb;
        private static FatDirEntry[] m_dirEntries;
        

        private static unsafe void initFAT(Node dev)
        {
            // Read first sector
            byte[] firstSector = new byte[512];
            firstSector[0x00] = 0xFF;
            dev.Read(dev, 0, 512, firstSector);

            Console.WriteHex(firstSector[FirstPartitonEntry + EntryType]);

            // Get partition type from first entry
            // Detect if FAT16
            if (firstSector[FirstPartitonEntry + EntryType] != 0x06)
                return;
            
            byte BeginHead = firstSector[FirstPartitonEntry + EntryBeginHead];
            byte Sector = (byte)(firstSector[FirstPartitonEntry + EntryBeginCylSec] & 0x3F);

            int tmp = firstSector[FirstPartitonEntry + EntryBeginCylSec] >> 8;
            byte cylinderHi = (byte)(tmp & 0x3);
            byte cylinderLo = firstSector[FirstPartitonEntry + EntryBeginCylSec + 1];

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
                if (offset == 16)
                {
                    sectorOffset++;
                    m_dev.Read(m_dev, (uint)(m_clusterBeginLBA + sectorOffset), 512, buffer);
                    
                    offset = 0;
                }

                m_dirEntries[i] = curBufPtr[offset];

                offset++;
            }
            
            m_beginDataLBA = m_clusterBeginLBA + ((m_bpb->NumDirEntries * 32) / m_bpb->BytesPerSector);
        }

        public static uint Data_clust_to_lba(int cluster)
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

            VFS.AddMountPoint(p);
        }

        private static DirEntry* readDirImpl(Node node, uint index)
        {
            if (index > 5)
                return null;

            FatDirEntry entry = m_dirEntries[index];

            if (entry.Name[0] == 0 || entry.Name[0] == 0xE5)
                return (DirEntry *)Heap.Alloc(sizeof(DirEntry));

            DirEntry* dir = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
            Memory.Memcpy(dir->Name, entry.Name, 11);
            dir->Name[11] = '\0';

            return dir;
        }
    }
}
