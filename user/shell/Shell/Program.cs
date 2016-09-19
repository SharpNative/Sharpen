using Sharpen;
using Sharpen.IO;
using Sharpen.Utilities;

namespace Shell
{
    class Program
    {
        unsafe static void Main(string[] args)
        {
            Console.WriteLine("Welcome");

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
                    Directory dir = Directory.Open(".");

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
                        Console.Write(command);
                        Console.WriteLine(": Bad command or filename");
                    }

                    // Wait until exit to return to prompt
                    Process.WaitForExit(ret);
                }
            }
        }
    }
}
