using LibCS2C.Attributes;
using Sharpen.IO;
using Sharpen.Mem;

namespace Play
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
            if (argc != 2)
            {
                Console.WriteLine("play:  Plays an audio file.");
                Console.WriteLine("Usage: play <filename>");
                return;
            }

            // Open file
            File file = new File(args[1], File.FileMode.ReadOnly);
            if (!file.IsOpen)
            {
                Console.WriteLine("play: Could not open the file.");
                Heap.Free(file);
                return;
            }

            // Open audio device
            File audioDev = new File("audio://default", File.FileMode.WriteOnly);
            if(!audioDev.IsOpen)
            {
                Console.WriteLine("play: Could not open the audio device.");
                Heap.Free(file);
                Heap.Free(audioDev);
                return;
            }

            // Maximum length
            uint maxLength = file.GetSize();

            // Read
            byte[] buffer = new byte[BufferSize];
            for(uint i = 0; i < maxLength; i += BufferSize)
            {
                file.Read(buffer, BufferSize);
                audioDev.Write(buffer, BufferSize);
            }

            // End
            file.Close();
            Heap.Free(file);
            Heap.Free(audioDev);
            Heap.Free(buffer);

            Console.WriteLine("");
        }
    }
}
