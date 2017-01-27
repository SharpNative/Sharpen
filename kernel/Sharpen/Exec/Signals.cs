namespace Sharpen.Exec
{
    public enum Signal
    {
        SIGHUP = 1,     // Hangup
        SIGINT = 2,     // Interrupt
        SIGQUIT = 3,    // Quit
        SIGILL = 4,     // Illegal instruction (not reset when caught)
        SIGTRAP = 5,    // Trace trap (not reset when caught)
        SIGABRT = 6,    // Used by abort
        SIGEMT = 7,     // EMT instruction
        SIGFPE = 8,     // Floating point exception
        SIGKILL = 9,    // Kill (cannot be caught or ignored)
        SIGBUS = 10,    // Bus error
        SIGSEGV = 11,   // Segmentation violation
        SIGSYS = 12,    // Bad argument to system call
        SIGPIPE = 13,   // Write on a pipe with no one to read it
        SIGALRM = 14,   // Alarm clock
        SIGTERM = 15,   // Software termination signal from kill
        SIGURG = 16,    // Urgent condition on IO channel
        SIGSTOP = 17,   // Sendable stop signal not from tty
        SIGTSTP = 18,   // Stop signal from tty
        SIGCONT = 19,   // Continue a stopped process
        SIGCHLD = 20,   // To parent on child stop or exit
        SIGTTIN = 21,   // To readers pgrp upon background tty read
        SIGTTOU = 22,   // Like TTIN for output if (tp->t_local&LTOSTOP)
        SIGIO = 23,     // Input/output possible signal
        SIGXCPU = 24,   // Exceeded CPU time limit
        SIGXFSZ = 25,   // Exceeded file size limit
        SIGVTALRM = 26, // Virtual time alarm
        SIGPROF = 27,   // Profiling time alarm
        SIGWINCH = 28,  // Window changed
        SIGLOST = 29,   // Resource lost (eg, record-lock lost)
        SIGUSR1 = 30,   // User defined signal 1
        SIGUSR2 = 31    // User defined signal 2
    }

    public sealed class Signals
    {
        public const int SIG_DFL = 0;
        public const int SIG_IGN = 1;
        public const int SIG_ERR = -1;
        public const int NSIG = 32; // Signal 0 implied

        public static readonly string[] SignalNames =
        {
            null,
            "Hangup",
            "Interrupt",
            "Quit",
            "Illegal instruction",
            "Trace/breakpoint trap",
            "IOT trap",
            "EMT trap",
            "Floating point exception",
            "Killed",
            "Bus error",
            "Segmentation fault",
            "Bad system call",
            "Broken pipe",
            "Alarm clock",
            "Terminated",
            "Urgent I/O condition",
            "Stopped (signal)",
            "Stopped",
            "Continued",
            "Child exited",
            "Stopped (TTY input)",
            "Stopped (TTY output)",
            "I/O possible",
            "CPU time limit exceeded",
            "File size limit exceeded",
            "Virtual timer expired",
            "Profiling timer expired",
            "Window changed",
            "Resource lost",
            "User signal 1",
            "User signal 2"
        };

        // Possible default actions
        public enum DefaultAction
        {
            Terminate,
            Ignore,
            Core,
            Stop,
            Continue
        }

        // Default actions (corresponds to SignalNumbers)
        public static readonly DefaultAction[] DefaultActions =
        {
            DefaultAction.Terminate, // Signal 0 is unused
            DefaultAction.Terminate,
            DefaultAction.Terminate,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Terminate,
            DefaultAction.Core,
            DefaultAction.Terminate,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Terminate,
            DefaultAction.Terminate,
            DefaultAction.Terminate,
            DefaultAction.Ignore,
            DefaultAction.Stop,
            DefaultAction.Stop,
            DefaultAction.Continue,
            DefaultAction.Ignore,
            DefaultAction.Stop,
            DefaultAction.Stop,
            DefaultAction.Terminate,
            DefaultAction.Core,
            DefaultAction.Core,
            DefaultAction.Terminate,
            DefaultAction.Terminate,
            DefaultAction.Ignore,
            DefaultAction.Terminate,
            DefaultAction.Terminate,
            DefaultAction.Terminate
        };
    }
}
