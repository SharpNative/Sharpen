using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;

namespace type
{
    class Program
    {
        private const int BufferSize = 4096;

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
            if (argc <= 1 || argc > 3)
            {
                Console.WriteLine("type:  Prints the contents of a file.");
                Console.WriteLine("Usage: type <filename> [optional: maxlength]");
                return;
            }
            
            // Open file
            File file = new File(args[1], File.FileMode.ReadOnly);
            if (!file.IsOpen)
            {
                Console.WriteLine("type: Could not open the file.");
                Heap.Free(file);
                return;
            }

            // Maximum length
            int maxLength = (argc == 2) ? (int)file.GetSize() : int.Parse(args[2]);

            // Read
            byte[] buffer = new byte[BufferSize];

            int totalRead = 0;
            int read = 0;
            do
            {
                read = file.Read(buffer, BufferSize);
                for (int i = 0; i < read && i < maxLength - totalRead; i++)
                    Console.Write((char)buffer[i]);
                totalRead += read;
                Console.Flush();
            }
            while (read == 4096 && totalRead < maxLength);

            // End
            file.Close();
            Heap.Free(file);
            Heap.Free(buffer);
        }
    }
}
