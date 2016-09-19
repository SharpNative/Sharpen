namespace Sharpen
{
    class Process
    {
        extern static int internalRun(string path, string[] args);
        public extern static void WaitForExit(int pid);
        public extern static void Exit(int status);

        /// <summary>
        /// Runs a new process
        /// </summary>
        /// <param name="path">The path of the executable</param>
        /// <param name="argv">The arguments</param>
        /// <param name="argc">The argument count</param>
        /// <returns>The errorcode</returns>
        public static int Run(string path, string[] argv, int argc)
        {
            string[] args = new string[argc + 1];

            if (argc > 0)
                for (int i = 0; i < argc; i++)
                    args[i] = argv[i];

            args[argc] = null;

            return internalRun(path, args);
        }
    }
}
