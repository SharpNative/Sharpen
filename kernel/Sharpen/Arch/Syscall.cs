using Sharpen.Exec;
using Sharpen.Utilities;
using Sharpen.FileSystem;
using Sharpen.Task;

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
            if (function < 0 || function > Syscalls.SYSCALL_MAX)
            {
                Console.Write("[SYSCALL] ");
                Console.WriteNum(function);
                Console.Write(" > ");
                Console.WriteNum(Syscalls.SYSCALL_MAX);
                Console.Write('\n');
                return;
            }

            //if (function != 0xD && function != 9 && function != 0x16)
            //{
            //    Console.Write("syscall func: ");
            //    Console.WriteNum(function);
            //    Console.WriteLine("");
            //}

            Tasking.CurrentTask.SysRegs = regsPtr;

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
                    ret = (int)Syscalls.Sbrk(regsPtr->EBX);
                    break;

                case Syscalls.SYS_FORK:
                    ret = Syscalls.Fork();
                    break;

                case Syscalls.SYS_WRITE:
                    ret = Syscalls.Write(regsPtr->EBX, Util.PtrToArray((byte*)regsPtr->ECX), (uint)regsPtr->EDX);
                    break;

                case Syscalls.SYS_READ:
                    ret = Syscalls.Read(regsPtr->EBX, Util.PtrToArray((byte*)regsPtr->ECX), (uint)regsPtr->EDX);
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

                case Syscalls.SYS_FSTAT:
                    ret = Syscalls.FStat(regsPtr->EBX, (Stat*)regsPtr->ECX);
                    break;

                case Syscalls.SYS_STAT:
                    ret = Syscalls.Stat(Util.CharPtrToString((char*)regsPtr->EBX), (Stat*)regsPtr->ECX);
                    break;

                case Syscalls.SYS_EXECVE:
                    ret = Syscalls.Execve(Util.CharPtrToString((char*)regsPtr->EBX), Util.PtrToArray((char**)regsPtr->ECX), Util.PtrToArray((char**)regsPtr->EDX));
                    break;

                case Syscalls.SYS_RUN:
                    ret = Syscalls.Run(Util.CharPtrToString((char*)regsPtr->EBX), Util.PtrToArray((char**)regsPtr->ECX), Util.PtrToArray((char**)regsPtr->EDX));
                    break;

                case Syscalls.SYS_WAITPID:
                    ret = Syscalls.WaitPID(regsPtr->EBX, (int*)regsPtr->ECX, regsPtr->EDX);
                    break;

                case Syscalls.SYS_READDIR:
                    ret = Syscalls.Readdir(regsPtr->EBX, (DirEntry*)regsPtr->ECX, (uint)regsPtr->EDX);
                    break;

                case Syscalls.SYS_SHUTDOWN:
                    ret = Syscalls.Shutdown();
                    break;

                case Syscalls.SYS_REBOOT:
                    ret = Syscalls.Reboot();
                    break;

                case Syscalls.SYS_GETTIMEOFDAY:
                    ret = Syscalls.GetTimeOfDay((Time.Timeval*)regsPtr->EBX);
                    break;

                case Syscalls.SYS_PIPE:
                    ret = Syscalls.Pipe((int*)regsPtr->EBX);
                    break;

                case Syscalls.SYS_DUP2:
                    ret = Syscalls.Dup2(regsPtr->EBX, regsPtr->ECX);
                    break;

                case Syscalls.SYS_SIG_SEND:
                    // TODO
                    break;

                case Syscalls.SYS_SIG_HANDLER:
                    // TODO
                    break;

                case Syscalls.SYS_YIELD:
                    ret = Syscalls.Yield();
                    break;

                case Syscalls.SYS_GETCWD:
                    ret = Syscalls.GetCWD((char*)regsPtr->EBX, regsPtr->ECX);
                    break;

                case Syscalls.SYS_CHDIR:
                    ret = Syscalls.ChDir(Util.CharPtrToString((char*)regsPtr->EBX));
                    break;

                default:
                    Console.Write("Unhandled syscall ");
                    Console.WriteNum(function);
                    Console.WriteLine("");
                    break;
            }

            Tasking.CurrentTask.SysRegs->EAX = ret;
        }
    }
}
