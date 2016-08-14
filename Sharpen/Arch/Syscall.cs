using Sharpen.Exec;
using Sharpen.Utilities;
using Sharpen.FileSystem;

namespace Sharpen.Arch
{
    public sealed class Syscall
    {
        /// <summary>
        /// Syscall handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(Regs* regsPtr)
        {
            int function = regsPtr->EAX;
            if (function > 8)
                return;

            int ret = 0;
            switch (function)
            {
                case Syscalls.SYS_EXIT:
                    ret = Syscalls.Exit(regsPtr->EBX);
                    break;

                case Syscalls.SYS_GETPID:
                    ret = Syscalls.GetPID();
                    break;

                case Syscalls.SYS_SBRK:
                    ret = Syscalls.Sbrk(regsPtr->EBX);
                    break;

                case Syscalls.SYS_FORK:
                    ret = Syscalls.Fork();
                    break;

                case Syscalls.SYS_WRITE:
                    ret = Syscalls.Write(regsPtr->EBX, Util.BytePtrToByteArray((byte*)regsPtr->ECX), (uint)regsPtr->EDX);
                    break;

                case Syscalls.SYS_READ:
                    ret = Syscalls.Read(regsPtr->EBX, Util.BytePtrToByteArray((byte*)regsPtr->ECX), (uint)regsPtr->EDX);
                    break;

                case Syscalls.SYS_OPEN:
                    ret = Syscalls.Open(Util.CharPtrToString((char*)regsPtr->EBX), regsPtr->ECX);
                    break;

                case Syscalls.SYS_CLOSE:
                    ret = Syscalls.Close(regsPtr->EBX);
                    break;

                case Syscalls.SYS_SEEK:
                    ret = Syscalls.Seek(regsPtr->EBX, (uint)regsPtr->ECX, (FileWhence)regsPtr->EDX);
                    break;

                default:
                    Console.Write("Unhandled syscall ");
                    Console.WriteHex(function);
                    Console.WriteLine("");
                    break;
            }
            
            regsPtr->EAX = ret;
        }
    }
}
