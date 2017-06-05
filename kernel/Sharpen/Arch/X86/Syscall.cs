// #define PRINT_SYSCALLS

using Sharpen.Exec;
using Sharpen.Utilities;
using Sharpen.FileSystem;
using Sharpen.MultiTasking;

namespace Sharpen.Arch
{
    public sealed class Syscall
    {
        /// <summary>
        /// Syscall handler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        public static unsafe void Handler(RegsDirect* regsPtr)
        {
            int function = regsPtr->EAX;
            if (function < 0 || function > Syscalls.SYSCALL_MAX)
            {
                Console.Write("[SYSCALL] Invalid syscall requested (");
                Console.WriteNum(function);
                Console.Write(" > ");
                Console.WriteNum(Syscalls.SYSCALL_MAX);
                Console.Write(")\n");
                return;
            }

#if PRINT_SYSCALLS
            if (function != 0x0D && function != 0x09 && function != 0x16 && function != 0x4 && function != 0x5)
            {
                Console.Write("[SYSCALL] PID: ");
                Console.WriteNum(Tasking.CurrentTask.PID);
                Console.Write(" function: ");
                Console.WriteNum(function);
                Console.WriteLine("");
            }
#endif

            Tasking.CurrentTask.CurrentThread.Context.UpdateSyscallRegs(regsPtr);

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
                    ret = Syscalls.Seek(regsPtr->EBX, regsPtr->ECX, (FileWhence)regsPtr->EDX);
                    break;

                case Syscalls.SYS_FSTAT:
                    ret = -(int)Syscalls.FStat(regsPtr->EBX, (Stat*)regsPtr->ECX);
                    break;

                case Syscalls.SYS_STAT:
                    ret = -(int)Syscalls.Stat(Util.CharPtrToString((char*)regsPtr->EBX), (Stat*)regsPtr->ECX);
                    break;

                case Syscalls.SYS_EXECVE:
                    ret = Syscalls.Execve(Util.CharPtrToString((char*)regsPtr->EBX), Util.PtrToArray((char**)regsPtr->ECX), Util.PtrToArray((char**)regsPtr->EDX));
                    break;

                case Syscalls.SYS_RUN:
                    ret = Syscalls.Run(Util.CharPtrToString((char*)regsPtr->EBX), Util.PtrToArray((char**)regsPtr->ECX), Util.PtrToArray((char**)regsPtr->EDX));
                    break;

                case Syscalls.SYS_WAITPID:
                    ret = -(int)Syscalls.WaitPID(regsPtr->EBX, (int*)regsPtr->ECX, regsPtr->EDX);
                    break;

                case Syscalls.SYS_READDIR:
                    ret = -(int)Syscalls.Readdir(regsPtr->EBX, (DirEntry*)regsPtr->ECX, (uint)regsPtr->EDX);
                    break;

                case Syscalls.SYS_SHUTDOWN:
                    ret = -(int)Syscalls.Shutdown();
                    break;

                case Syscalls.SYS_REBOOT:
                    ret = -(int)Syscalls.Reboot();
                    break;

                case Syscalls.SYS_GETTIMEOFDAY:
                    ret = -(int)Syscalls.GetTimeOfDay((Time.Timeval*)regsPtr->EBX);
                    break;

                case Syscalls.SYS_PIPE:
                    ret = -(int)Syscalls.Pipe((int*)regsPtr->EBX);
                    break;

                case Syscalls.SYS_DUP2:
                    ret = Syscalls.Dup2(regsPtr->EBX, regsPtr->ECX);
                    break;

                case Syscalls.SYS_SIG_SEND:
                    ret = -(int)Syscalls.SigSend(regsPtr->EBX, (Signal)regsPtr->ECX);
                    break;

                case Syscalls.SYS_SETSIGHANDLER:
                    ret = -(int)Syscalls.SetSigHandler((Signal)regsPtr->EBX, (SignalAction.SigAction*)regsPtr->ECX, (SignalAction.SigAction*)regsPtr->EDX);
                    break;

                case Syscalls.SYS_RETURN_FROM_SIGNAL:
                    Syscalls.ReturnFromSignal();
                    ret = 0;
                    break;

                case Syscalls.SYS_GETCWD:
                    ret = -(int)Syscalls.GetCWD((char*)regsPtr->EBX, regsPtr->ECX);
                    break;

                case Syscalls.SYS_CHDIR:
                    ret = -(int)Syscalls.ChDir(Util.CharPtrToString((char*)regsPtr->EBX));
                    break;

                case Syscalls.SYS_SLEEP:
                    ret = (int)Syscalls.Sleep((uint)regsPtr->EBX, (uint)regsPtr->ECX);
                    break;

                case Syscalls.SYS_TRUNCATE:
                    ret = Syscalls.Truncate(Util.CharPtrToString((char*)regsPtr->EBX), (uint)regsPtr->ECX);
                    break;

                case Syscalls.SYS_FTRUNCATE:
                    ret = Syscalls.FTruncate(regsPtr->EBX, (uint)regsPtr->ECX);
                    break;

                case Syscalls.SYS_DUP:
                    ret = Syscalls.Dup(regsPtr->EBX);
                    break;

                case Syscalls.SYS_IOCTL:
                    ret = Syscalls.IOCtl(regsPtr->EBX, regsPtr->ECX, (void*)regsPtr->EDX);
                    break;

                case Syscalls.SYS_MKDIR:
                    ret = Syscalls.MkDir(Util.CharPtrToString((char*)regsPtr->EBX), regsPtr->ECX);
                    break;

                case Syscalls.SYS_RMDIR:
                    ret = Syscalls.RmDir(Util.CharPtrToString((char*)regsPtr->EBX));
                    break;

                case Syscalls.SYS_UNLINK:
                    ret = Syscalls.Unlink(Util.CharPtrToString((char*)regsPtr->EBX));
                    break;

                case Syscalls.SYS_MOUNT:
                    ret = Syscalls.Mount(Util.CharPtrToString((char*)regsPtr->EBX), Util.CharPtrToString((char*)regsPtr->ECX), Util.CharPtrToString((char*)regsPtr->EDX));
                    break;

                case Syscalls.SYS_UMOUNT:
                    ret = Syscalls.Umount(Util.CharPtrToString((char*)regsPtr->EBX));
                    break;

                default:
                    Console.Write("Unhandled syscall ");
                    Console.WriteNum(function);
                    Console.WriteLine("");
                    break;
            }

            Tasking.CurrentTask.CurrentThread.Context.SetSysReturnValue(ret);
        }
    }
}
