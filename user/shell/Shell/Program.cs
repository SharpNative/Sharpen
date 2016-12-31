using Sharpen;
using Sharpen.IO;
using Sharpen.Memory;
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
        /// <returns></returns>
        private unsafe static int TryRunFromExecDir(string name, string[] argv, int argc)
        {
            string total_string = String.Merge("C://exec/", name);

            int ret = Process.Run(total_string, argv, argc);

            Heap.Free(Util.ObjectToVoidPtr(total_string));

            return ret;
        }

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Arguments</param>
        unsafe static void Main(string[] args)
        {
            Console.WriteLine("Project Sharpen");
            Console.WriteLine("(c) 2016 SharpNative\n");

            while (true)
            {
                Console.Write(Directory.GetCurrentDirectory());
                Console.Write("> ");

                string read = Console.ReadLine();

                int offsetToSpace = String.IndexOf(read, " ");
                if (offsetToSpace == -1)
                    offsetToSpace = String.Length(read);

                string command = String.SubString(read, 0, offsetToSpace);
                if (command == null)
                    continue;

                string[] argv = null;
                int argc = 1;

                // It has no arguments
                if (read[offsetToSpace] == '\0')
                {
                    argv = new string[2];
                    argv[0] = command;
                    argv[1] = null;
                }
                // It has arguments
                else
                {
                    // Fetch arguments
                    string argumentStart = String.SubString(read, offsetToSpace + 1, String.Length(read) - offsetToSpace - 1);
                    argc = 1 + (String.Count(argumentStart, ' ') + 1);
                    argv = new string[argc + 1];
                    argv[0] = command;

                    // Add arguments
                    int i = 0;
                    int offset = 0;
                    for (; i < argc; i++)
                    {
                        // Find argument end
                        int nextOffset = offset;
                        for (; argumentStart[nextOffset] != ' ' && argumentStart[nextOffset] != '\0'; nextOffset++) ;

                        // Grab argument
                        string arg = String.SubString(argumentStart, offset, nextOffset - offset);
                        offset = nextOffset + 1;
                        argv[i + 1] = arg;
                    }

                    // Add null to end arguments
                    argv[i] = null;
                }

                if (String.Equals(command, "cd"))
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
                    }
                }
                else if (String.Equals(command, "dir"))
                {
                    Directory dir = Directory.Open(Directory.GetCurrentDirectory());
                    
                    uint i = 0;
                    while (true)
                    {
                        Directory.DirEntry entry = dir.Readdir(i);
                        if (entry.Name[0] == '\0')
                            break;

                        string str = Util.CharPtrToString(entry.Name);

                        Console.WriteLine(str);

                        i++;
                    }

                    dir.Close();
                }
                else if (String.Equals(command, "exit"))
                {
                    Process.Exit(0);
                }
                else
                {
                    // Try to start a process
                    int ret = Process.Run(command, argv, argc);
                    if (ret < 0)
                    {
                        ret = TryRunFromExecDir(command, argv, argc);

                        if (ret < 0)
                        {
                            Console.Write(command);
                            Console.WriteLine(": Bad command or filename");
                        }
                    }

                    // Wait until exit to return to prompt
                    Process.WaitForExit(ret);
                }
            }
        }
    }
}
