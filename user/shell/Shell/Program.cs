using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Program
    {
        static string m_folder = "C://";

        static void Main(string[] args)
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
                

                if(String.Equals(command, "udm"))
                {
                    Console.WriteLine("udm mode activated");
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
