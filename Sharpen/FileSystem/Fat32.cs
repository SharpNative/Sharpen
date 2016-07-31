using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    class Fat32
    {
        private readonly int FirstPartitonEntry = 0x1BE;

        private readonly int EntryActive = 0;
        private readonly int EntryBeginHead = 0x01;
        private readonly int EntryBeginCylSec = 0x02;
        private readonly int EntryType = 0x04;
        private readonly int EntryEndHead = 0x05;
        private readonly int EntryEndCylSec = 0x06;
        private readonly int EntryNumSectorsBetween = 0x08;
        private readonly int EntryNumSectors = 0x0C;

        private Node m_dev;
        private int m_bytespersector;

        public unsafe Fat32(Node dev, string name)
        {
            m_dev = dev;

            // Read first sector
            byte[] firstSector = new byte[512];
            firstSector[0x00] = 0xFF;
            dev.Read(dev, 0, 512, firstSector);

            byte systemID = firstSector[0x1BE];

            // Get partition type from first entry
            // Detect if FAT32
            if (firstSector[FirstPartitonEntry + EntryType] != 0x0B)
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
            int lba = (cylinder * hpc + BeginHead) * spt + (Sector - 1);
            


            Console.WriteHex(systemID);

        }
    }
}
