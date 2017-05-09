using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DumpSymbols
{
    class Program
    {
        /// <summary>
        /// Uses a symbol dump file to generate an assembly file containing the symbols
        /// Note: doing this in batch is terribly slow
        /// </summary>
        /// <param name="args">Arguments</param>
        static void Main(string[] args)
        {
            // Usage
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: DumpSymbols [symbolsfile] [outputfile]");
                return;
            }

            string input = args[0];
            string output = args[1];
            if (!File.Exists(input))
            {
                Console.WriteLine(input + ": No such file");
                return;
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder lineSb = new StringBuilder();
            sb.AppendLine("section .kernelsymbols");
            sb.AppendLine("global kernelsymbols");
            sb.AppendLine("kernelsymbols:");

            // Each line is a symbol entry
            IEnumerable<string> lines = File.ReadLines(input);

            // We use a seperate StringBuilder because we still need to know the amount of entries
            // Note: the amount of entries is not the amount of lines!
            int entries = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                if (parts.Length != 3)
                    continue;

                // Must start with "Sharpen."
                string symbolName = parts[2].Replace('_', '.');
                if (!symbolName.StartsWith("Sharpen."))
                    continue;

                // It is pointless keeping "Sharpen." since everything is in the "Sharpen" namespace
                symbolName = symbolName.Substring("Sharpen.".Length);

                // A symbol cannot start with .<number>, so using that we know where the possible arguments in the name start
                for (int i = 0; i <= 9; i++)
                {
                    int index = symbolName.IndexOf("." + i);
                    if (index != -1)
                    {
                        symbolName = symbolName.Substring(0, index);
                        break;
                    }
                }

                lineSb.AppendLine(string.Format("dd 0x{0}", parts[0]));
                lineSb.AppendLine(string.Format("db \"{0}\", 0", symbolName));
                entries++;
            }

            sb.AppendLine(string.Format("dd {0}", entries));
            sb.Append(lineSb.ToString());
            File.WriteAllText(output, sb.ToString());
        }
    }
}
