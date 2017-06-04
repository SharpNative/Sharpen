using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem.Filesystems
{
    // TODO: Extract the BPB from the extBS
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct Fat16BPB
    {

        // Standard BPB
        public fixed byte Jump[3];
        public fixed char OemID[8];
        public ushort BytesPerSector;
        public byte SectorsPerCluster;
        public ushort ReservedSectors;
        public byte NumFats;
        public ushort NumDirEntries;
        public ushort TotalSectorsLogical;
        public byte MediaDescriptorType;
        public ushort SectorsPerFat16; // Not used by FAT32
        public ushort SectorsPerTrack;
        public ushort NumHeadsOrSides;
        public uint HiddenSectors; // LBA
        public uint LargeAmountOfSectors;

        // Extended BS
        public byte DriveNumber;
        public byte Flags;
        public byte Signature;
        public fixed char VolumeID[4];
        public fixed char Label[11];
        public fixed char SysIdentifier[8];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FatDirEntry
    {
        public fixed char Name[11];
        public byte Attribs;
        public byte Reserved;
        public byte CreationTime;
        public ushort TimeCreated;
        public ushort DateCreated;
        public ushort LastAccessedDate;
        public ushort ClusterNumberHi;
        public ushort LastModTime;
        public ushort LastModDate;
        public ushort ClusterNumberLo;
        public uint Size;
    }

    public unsafe class SubDirectory
    {
        public uint Length;

        public FatDirEntry *DirEntries;
    }
}
