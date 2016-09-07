using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Process
    {
        extern static int internalRun(string path, string[] args);
        public extern static void WaitForExit(int pid);
        public extern static void Exit(int status);

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
