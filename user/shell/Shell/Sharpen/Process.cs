using LibCS2C.Attributes;

namespace Sharpen
{
    sealed class Process
    {
        /// <summary>
        /// Exits the current process
        /// </summary>
        /// <param name="status">The errorcode status</param>
        [Extern("_exit")]
        public extern static void Exit(int status);

        /// <summary>
        /// Runs a new process
        /// </summary>
        /// <param name="path">The path of the executable</param>
        /// <param name="argv">The arguments</param>
        /// <param name="envp">The environment</param>
        /// <returns>The errorcode</returns>
        [Extern("run")]
        public static extern int Run(string path, string[] argv, string[] envp);

        /// <summary>
        /// Waits until a process exits
        /// </summary>
        /// <param name="pid">The process PID</param>
        public static unsafe void WaitForExit(int pid)
        {
            WaitPID(pid, null, 0);
        }

        /// <summary>
        /// Wait for process to change state
        /// </summary>
        /// <param name="pid">The PID</param>
        /// <param name="status">The status</param>
        /// <param name="options">The options</param>
        /// <returns>On success, returns the PID of the child whose state changed, If WNOHANG then zero is returned, on error -1</returns>
        [Extern("waitpid")]
        public static unsafe extern int WaitPID(int pid, int* status, int options);
    }
}
