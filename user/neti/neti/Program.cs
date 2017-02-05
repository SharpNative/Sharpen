using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;
using SharpenLib.IO;

namespace Neti
{
    class Program
    {
        /// <summary>
        /// Prints an IP address
        /// </summary>
        /// <param name="ip">The IP</param>
        private static void printIP(byte[] ip)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Write(ip[i]);
                Console.Write('.');
            }
            Console.Write(ip[3]);
        }

        /// <summary>
        /// Prints the info of an entry
        /// </summary>
        /// <param name="filename">The filename of the entry</param>
        /// <param name="infoname">The info name</param>
        private static void printInfo(string filename, string infoname)
        {
            byte[] ip = new byte[4];

            File file = new File(filename);
            file.Read(ip, 4);
            file.Close();

            Console.Write(infoname);
            Console.Write(": ");
            printIP(ip);
            Console.Write('\n');

            Heap.Free(file);
            Heap.Free(ip);
        }

        /// <summary>
        /// The entrypoint
        /// </summary>
        /// <param name="args">The arguments</param>
        [Plug("EntryPoint")]
        static void Main(string[] args)
        {
            printInfo("net://info/ip", "IP");
            printInfo("net://info/gateway", "Gateway");
            printInfo("net://info/subnet", "Netmask");
            printInfo("net://info/ns1", "DNS1");
            printInfo("net://info/ns2", "DNS2");
        }
    }
}
