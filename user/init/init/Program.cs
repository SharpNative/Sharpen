using LibCS2C.Attributes;
using Sharpen;
using Sharpen.IO;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace init
{
    class Program
    {
        private const int WNOHANG = 1;

        [Plug("EntryPoint")]
        static unsafe void Main(string[] args)
        {
            File fdin = new File("devices://keyboard");
            File fdout = new File("devices://stdout");
            File fderr = new File("devices://stdout");

            // 0 is read end, 1 is write end
            int[] fd = new int[2];
            File.Pipe(fd);

            // Child process
            var pid = Process.Fork();
            
            if (pid == 0)
            {
                // Child, replace STDIO descriptors
                // Close write end
                File.Close(fdin.FileDescriptorID);
                File.Close(fd[1]);
                File.Dup2(0, fd[0]);

                string[] argv = new string[2];
                argv[0] = args[1];
                argv[1] = null;

                Process.Execve(argv[0], argv, null);
            }
            else
            {
                File.Close(fd[0]);

                char[] input = new char[1024];
                int index = 0;

                while (true)
                {
                    uint size = fdin.GetSize();
                    if (size > 0)
                    {
                        byte[] buffer = new byte[size];
                        fdin.Read(buffer, (int)size);
                        byte[] outb = new byte[3];

                        for (int i = 0; i < size; i++)
                        {
                            char ch = (char)buffer[i];

                            if (ch == '\b')
                            {
                                if (index > 0)
                                {
                                    input[--index] = '\0';

                                    outb[0] = (byte)'\b';
                                    outb[1] = (byte)' ';
                                    outb[2] = (byte)'\b';
                                    fdout.Write(outb, 3 / sizeof(char));
                                }
                            }
                            else if (ch == '\n')
                            {
                                input[index++] = '\n';
                                File.Write(fd[1], Util.ObjectToVoidPtr(input), index);
                                index = 0;

                                outb[0] = (byte)ch;
                                fdout.Write(outb, 1 / sizeof(char));
                            }
                            else
                            {
                                input[index++] = ch;

                                outb[0] = (byte)ch;
                                fdout.Write(outb, 1 / sizeof(char));
                            }

                            // Buffer full? Send it
                            if (index == 1024)
                            {
                                File.Write(fd[1], Util.ObjectToVoidPtr(input), index);
                                index = 0;
                            }
                        }

                        Heap.Free(buffer);
                    }

                    // No input, yield
                    Process.Yield();

                    // Wait for child to exit
                    if (Process.WaitPID(pid, null, WNOHANG) != 0)
                        break;
                }

                Console.WriteHex(0xFf);
            }
        }
    }
}
