using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;

namespace mount
{
    class Program
    {
        [Extern("umount", true)]
        public static extern int umount(string mountName);

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
            if (argc != 2)
            {
                Console.WriteLine("mount: Unmount a mountpoint");
                Console.WriteLine("Usage: umount [targetMount]");
                return;
            }

            int result = umount(args[1]);

            if (result == 0)
                Console.WriteLine("Target unmounted.");
            else
                Console.WriteLine("Could not unmount target.");
        }
    }
}
