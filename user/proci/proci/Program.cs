using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace proci
{
    unsafe class Program
    {
        struct ProcFSInfo
        {
            public fixed char Name[64];
            public fixed char CMDLine[256];
            public int Pid;
            public uint Uptime;
            public int Priority;
            public int ThreadCount;
        }

        /// <summary>
        /// Gets the priority name of a priority number
        /// </summary>
        /// <param name="prio">The priority number</param>
        /// <returns>The priority name</returns>
        private static string getPriority(int prio)
        {
            switch (prio)
            {
                case 1:
                    return "Very low";

                case 3:
                    return "Low";

                case 6:
                    return "Normal";

                case 9:
                    return "High";

                case 12:
                    return "Very high";
            }

            return "?";
        }

        /// <summary>
        /// Prints information about a process
        /// </summary>
        /// <param name="name">The directory name</param>
        private static void printProcess(string name)
        {
            string tmp1 = String.Merge("proc://", name);
            string tmp2 = String.Merge(tmp1, "/info");
            
            File file = new File(tmp2, File.FileMode.ReadOnly);
            if (file.IsOpen)
            {
                ProcFSInfo info = new ProcFSInfo();
                file.Read(&info, sizeof(ProcFSInfo));

                Console.Write("Process ");
                Console.Write(info.Pid);
                Console.Write("\t\t");
                Console.WriteLine(Util.CharPtrToString(info.Name));
                Console.Write("\tCMDLine\t\t");
                Console.WriteLine(Util.CharPtrToString(info.CMDLine));
                Console.Write("\tPriority\t");
                Console.WriteLine(getPriority(info.Priority));
                Console.Write("\tThreads\t\t");
                Console.WriteLine(info.ThreadCount);
                Console.Write("\tUptime\t\t");
                Console.WriteLine((int)info.Uptime);
            }

            file.Close();
            Heap.Free(file);

            Heap.Free(tmp1);
            Heap.Free(tmp2);
        }

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Arguments</param>
        [Plug("EntryPoint")]
        static void Main(string[] args)
        {
            Directory dir = Directory.Open("proc://");

            while (true)
            {
                Directory.DirEntry entry = dir.Readdir();
                if (entry.Name[0] == '\0')
                    break;

                string str = Util.CharPtrToString(entry.Name);
                printProcess(str);
            }

            dir.Close();
            Heap.Free(dir);
        }
    }
}
