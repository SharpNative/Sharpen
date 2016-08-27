using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Process
    {
        extern static void internalRun(string path, string[] args); 

        public static void Run(string path, string[] argv = null, int argc = 0)
        {
            string[] args = new string[argc + 1];

            if (argc > 0)
                for (int i = 0; i < argc; i++)
                    args[i] = argv[i];

            args[argc] = null;

            internalRun(path, args);
        }
    }
}
