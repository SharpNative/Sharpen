
namespace Shell
{
    class Program
    {
        static string m_folder = "C://";

        unsafe static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(m_folder);
                Console.Write("> ");

                string read = Console.ReadLine();
                Console.WriteLine("");

                int offsetToSpace = String.IndexOf(read, " ");

                if (offsetToSpace == -1)
                    offsetToSpace = String.Length(read);

                string command = String.SubString(read, 0, offsetToSpace);
                if (command == null)
                    continue;
                
                if(String.Equals(command, "cd"))
                {

                }
                else if(String.Equals(command, "dir"))
                {
                    Directory dir = Directory.Open(m_folder);

                    uint i = 0;
                    while (true)
                    {
                        Directory.DirEntry entry = dir.Readdir(i);
                        if (entry.Name[0] == (char)0x00)
                            break;

                        string str = Util.CharPtrToString(entry.Name);
                        
                        Console.WriteLine(str);

                        i++;
                    }
                    dir.Close();
                }
                else
                {
                    Console.Write("Unknown command '");
                    Console.Write(command);
                    Console.WriteLine("'");
                }
            }
        }
    }
}
