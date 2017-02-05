using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace pcii
{
    unsafe class Program
    {
        struct PCIFSInfo
        {
            public ushort Bus;
            public ushort Slot;
            public ushort Function;

            public byte ClassCode;
            public byte SubClass;
            public byte ProgIntf;

            public ushort Vendor;
            public ushort Device;
        }

        enum BoxChar
        {
            NONE,
            TL = '\xDA',
            TM = '\xC2',
            TR = '\xBF',
            ML = '\xB3',
            MM = '\xC5',
            MR = '\xB4',
            BL = '\xC0',
            BM = '\xC1',
            BR = '\xD9'
        }

        /// <summary>
        /// Prints information about a device
        /// </summary>
        /// <param name="name">The directory name</param>
        private static void printDevice(string name)
        {
            string tmp1 = String.Merge("pci://", name);
            string tmp2 = String.Merge(tmp1, "/info");

            File file = new File(tmp2, File.FileMode.ReadOnly);
            if (file.IsOpen)
            {
                PCIFSInfo info = new PCIFSInfo();
                file.Read(&info, sizeof(PCIFSInfo));
                
                Console.Write("\xB3\t");
                Console.Write(info.Bus);
                Console.Write("\t\xB3\t");
                Console.Write(info.Slot);
                Console.Write("\t\xB3\t");
                Console.Write(info.Function);
                Console.Write("\t\xB3\t");
                Console.WriteHex(info.Vendor);
                Console.Write("\t\xB3\t");
                Console.WriteHex(info.Device);
                Console.Write("\t\xB3\t");
                Console.WriteHex(info.ClassCode);
                Console.Write("\t\xB3\t");
                Console.WriteHex(info.SubClass);
                Console.Write("\t\xB3\t");
                Console.WriteHex(info.ProgIntf);
                Console.Write("\t\xB3\n");
            }

            file.Close();
            Heap.Free(file);

            Heap.Free(tmp1);
            Heap.Free(tmp2);
        }

        /// <summary>
        /// Prints a line
        /// </summary>
        /// <param name="count">The amount of characters in the line</param>
        /// <param name="stop">What character to put at the end</param>
        private static void printLine(int count, BoxChar stop)
        {
            for (int i = 0; i < count; i++)
                Console.Write('\xC4');

            if (stop != BoxChar.NONE)
                Console.Write((char)stop);
        }

        /// <summary>
        /// Prints the top line
        /// </summary>
        private static void printTopLine()
        {
            Console.Write((char)BoxChar.TL);
            printLine(7, BoxChar.TM);
            printLine(7, BoxChar.TM);
            printLine(7, BoxChar.TM);
            printLine(11, BoxChar.TM);
            printLine(11, BoxChar.TM);
            printLine(7, BoxChar.TM);
            printLine(7, BoxChar.TM);
            printLine(7, BoxChar.TR);
            Console.Write('\n');
        }

        /// <summary>
        /// Prints a split line
        /// </summary>
        private static void printSplitLine()
        {
            Console.Write((char)BoxChar.ML);
            printLine(7, BoxChar.MM);
            printLine(7, BoxChar.MM);
            printLine(7, BoxChar.MM);
            printLine(11, BoxChar.MM);
            printLine(11, BoxChar.MM);
            printLine(7, BoxChar.MM);
            printLine(7, BoxChar.MM);
            printLine(7, BoxChar.MR);
            Console.Write('\n');
        }

        /// <summary>
        /// Prints the bottom line
        /// </summary>
        private static void printBottomLine()
        {
            Console.Write((char)BoxChar.BL);
            printLine(7, BoxChar.BM);
            printLine(7, BoxChar.BM);
            printLine(7, BoxChar.BM);
            printLine(11, BoxChar.BM);
            printLine(11, BoxChar.BM);
            printLine(7, BoxChar.BM);
            printLine(7, BoxChar.BM);
            printLine(7, BoxChar.BR);
            Console.Write('\n');
        }

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Arguments</param>
        [Plug("EntryPoint")]
        static void Main(string[] args)
        {
            // Header
            printTopLine();
            Console.WriteLine("\xB3  BUS  \xB3  SLOT \xB3  FUNC \xB3   VENDOR  \xB3   DEVICE  \xB3 CLASS \xB3  SUB  \xB3 INTF  \xB3");
            printSplitLine();
            
            Directory dir = Directory.Open("pci://");
            
            Directory.DirEntry entry = dir.Readdir();
            while (entry.Name[0] != '\0')
            {
                string str = Util.CharPtrToString(entry.Name);

                printDevice(str);
                
                entry = dir.Readdir();
                if (entry.Name[0] != '\0')
                    printSplitLine();
                else
                    printBottomLine();
            }

            dir.Close();
            Heap.Free(dir);
        }
    }
}
