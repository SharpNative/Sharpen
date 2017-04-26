namespace Sharpen.Utilities
{
    public sealed unsafe class SymbolTable
    {
        unsafe struct Symbol
        {
            public void* Address;
            // Note: actually the string itself, but it doesn't really work that way
            //       that's why we take it as a char
            public char Name;
        }

        /// <summary>
        /// Gets a pointer to the symbol table
        /// </summary>
        /// <returns>The symbol table</returns>
        private static unsafe extern void* getSymbolTable();

        /// <summary>
        /// Finds the name of the closest symbol to an address
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="addressOffset">A pointer to where to store the offset from this symbol</param>
        /// <returns>The symbol name</returns>
        public static string FindSymbolName(void* address, void** addressOffset)
        {
            void* table = getSymbolTable();

            // Before the first entry, there's an int stored for the amount of entries
            int entries = *(int*)table;
            int tableOffset = sizeof(int);

            // Find closest match
            int distance = 0x7FFFFFFF;
            string candidateName = null;
            void* candidateOffset = null;
            for (int i = 0; i < entries; i++)
            {
                // Get symbol info
                Symbol* sym = (Symbol*)((int)table + tableOffset);
                string symbolName = Util.CharPtrToString(&sym->Name);
                int size = sizeof(void*) + symbolName.Length + 1;

                // Distance from current symbol address to the address we search for
                int currentDistance = (int)address - (int)sym->Address;
                if (currentDistance < 0 || currentDistance > distance)
                {
                    tableOffset += size;
                    continue;
                }

                candidateName = symbolName;
                candidateOffset = (void*)currentDistance;
                distance = currentDistance;

                tableOffset += size;
            }

            *addressOffset = candidateOffset;
            return candidateName;
        }
    }
}
