using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;

namespace mount
{
    class Program
    {
        [Extern("mount", true)]
        public static extern int mount(string devicePath, string mountName, string fsType);

        /// <summary>
        /// Entrypoint
        /// </summary>
        /// <param name="args">The arguments</param>
        [Plug("EntryPoint")]
        static void Main(string[] args)
        {

            // Get argument count
            int argc = 0;
            for (; args[argc] != null; argc++) ;

            // Usage
            if (argc != 4)
            {
                Console.WriteLine("mount: Mount a device on a root point");
                Console.WriteLine("Usage: mount [sourceFile] [targetMount] [FsType]");
                return;
            }

            int result = mount(args[1], args[2], args[3]);

            if (result == 0)
            {
                Console.Write(args[1]);
                Console.Write(" mounted on ");
                Console.Write(args[2]);
                Console.WriteLine("://");
            }
            else if (result == -20)
                Console.WriteLine("Target not a mountpoint.");
            else if (result == -19)
                Console.WriteLine("Given filesystem type not right");
            else if (result == -2)
                Console.WriteLine("Target already used");
        }
    }
}
