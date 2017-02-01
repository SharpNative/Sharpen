using Sharpen;
using Sharpen.IO;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Shell
{
    class Program
    {
        /// <summary>
        /// Try find program in C://exec and run it
        /// </summary>
        /// <param name="name">The program name</param>
        /// <param name="argv">Arguments</param>
        /// <param name="argc">Argument length</param>
        /// <returns>PID on success, -1 on fail</returns>
        private unsafe static int tryRunFromExecDir(string name, string[] argv)
        {
            string totalString = String.Merge("C://exec/", name);
            int ret = Process.Run(totalString, argv, null);
            Heap.Free(totalString);
            return ret;
        }

        /// <summary>
        /// Tries to start a process
        /// </summary>
        /// <param name="command">The program name</param>
        /// <param name="name">The program name</param>
        /// <param name="argv">Arguments</param>
        /// <returns>PID on success, -1 on fail</returns>
        private static int tryStartProcess(string command, string[] argv)
        {
            int ret = Process.Run(command, argv, null);
            if (ret < 0)
                ret = tryRunFromExecDir(command, argv);

            return ret;
        }

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Arguments</param>
        unsafe static void Main(string[] args)
        {
            Console.WriteLine("\nProject Sharpen");
            Console.WriteLine("(c) 2016-2017 SharpNative\n");

            string currentDir = Directory.GetCurrentDirectory();
            while (true)
            {
                // Prompt
                Console.Write(currentDir);
                Console.Write("> ");

                // Read line
                string read = Console.ReadLine();
                if (read.Length == 0)
                {
                    Heap.Free(read);
                    continue;
                }

                // Split command line
                int argc = String.Count(read, ' ') + 1;
                string[] argv = read.Split(' ');
                string command = argv[0];

                // Remove the empty arguments
                for (int i = 0; i < argc; i++)
                {
                    if (argv[i].Length == 0)
                    {
                        Heap.Free(argv[i]);
                        argv[i] = null;
                        argc--;
                    }
                }

                // Process commands
                if (command.Equals("cd"))
                {
                    if (argc != 2)
                    {
                        Console.WriteLine("Invalid usage of cd: cd [dirname]");
                    }
                    else
                    {
                        if (!Directory.SetCurrentDirectory(argv[1]))
                        {
                            Console.WriteLine("cd: Couldn't change the directory");
                        }
                        else
                        {
                            string old = currentDir;
                            currentDir = Directory.GetCurrentDirectory();
                            Heap.Free(old);
                        }
                    }
                }
                else if (command.Equals("dir"))
                {
                    Directory dir = Directory.Open(currentDir);

                    while (true)
                    {
                        Directory.DirEntry entry = dir.Readdir();
                        if (entry.Name[0] == '\0')
                            break;

                        string str = Util.CharPtrToString(entry.Name);
                        Console.WriteLine(str);
                    }

                    dir.Close();
                    Heap.Free(dir);
                }
                else if (command.Equals("exit"))
                {
                    Process.Exit(0);
                }
                else if (command.Equals("background"))
                {
                    // Try to start a process without waiting until exit
                    string[] offsetArgv = (string[])Array.CreateSubArray((object[])argv, 1, argc - 1);

                    int ret = tryStartProcess(offsetArgv[0], offsetArgv);
                    if (ret > 0)
                    {
                        Console.Write("Process started in background with PID ");
                        Console.Write(ret);
                    }
                    Console.Write('\n');
                    Heap.Free(offsetArgv);
                }
                else
                {
                    // Try to start a process and wait until exit to return to prompt
                    int ret = tryStartProcess(command, argv);
                    if (ret > 0)
                    {
                        Process.WaitForExit(ret);
                    }
                    else
                    {
                        Console.Write(command);
                        Console.WriteLine(": Bad command or filename");
                    }
                }

                // Note: command is in the first entry of argv
                for (int i = 0; i < argc; i++)
                {
                    if (argv[i] != null)
                        Heap.Free(argv[i]);
                }
                Heap.Free(read);
                Heap.Free(argv);
            }
        }
    }
}
