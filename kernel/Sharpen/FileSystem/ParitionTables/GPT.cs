using System;
using Sharpen.FileSystem.PartitionTables;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem.ParitionTables
{
    class GPT : IPartitionTable
    {

        /// <summary>
        /// Register GPT
        /// </summary>
        public static void Register()
        {
            Disk.RegisterPatitionTable(new GPT());
        }


        /// <summary>
        /// Detect if type is gpt
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public unsafe bool isType(Node node)
        {
            byte[] buf = new byte[512];

            node.Read(node, 1, 512, buf);

            bool same = Memory.Compare((char*)Util.ObjectToVoidPtr(buf), (char*)Util.ObjectToVoidPtr("EFI PART"), 8);

            Heap.Free(buf);

            return same;
        }

        public void ReadPartitions(Node node, string name)
        {

            Console.WriteLine("[GPT] Reading of GPT not implemented.");

            // TODO: Implement this
        }
    }
}
