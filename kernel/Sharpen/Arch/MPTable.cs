using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Arch
{
    unsafe class MPTable
    {
        /// <summary>
        /// Finds the MP table
        /// </summary>
        /// <returns>The MP table</returns>
        public static MPFloatingPoint* FindMPTable()
        {
            char* checkPtr = (char*)Util.ObjectToVoidPtr("_MP_");
            ushort* adr = (ushort*)0x040E;
            byte* ebdap = (byte*)((*adr) << 4);

            // First search in the first kb of EBDA
            MPFloatingPoint* mp = (MPFloatingPoint*)Util.FindStructure(checkPtr, 4, ebdap, (void*)((uint)ebdap + 1024), sizeof(MPFloatingPoint));
            if (mp == null)
            {
                // Not found? Check in last kb of system base memory
                mp = (MPFloatingPoint*)Util.FindStructure(checkPtr, 4, (void*)0x9FC00, (void*)0xA0000, sizeof(MPFloatingPoint));
                if (mp == null)
                {
                    // Not found? Check in the BIOS ROM between 0xF0000 and 0xFFFFF
                    mp = (MPFloatingPoint*)Util.FindStructure(checkPtr, 4, (void*)0xF0000, (void*)0xFFFFF, sizeof(MPFloatingPoint));
                }
            }

            return mp;
        }

        /// <summary>
        /// Checks a bus type
        /// </summary>
        /// <param name="type">The type string (6 chars)</param>
        /// <param name="check">The type to check for</param>
        /// <returns>If the bus is the type we wanted to check for</returns>
        private static bool checkBusType(char* type, string check)
        {
            for (int i = 0; i < 6; i++)
            {
                if (type[i] != check[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the MP table
        /// </summary>
        public static void Init()
        {
            // TODO: get rid of this and use ACPI AML interpreter instead

            // Note: the MP table is guaranteed to be mapped since it is under 1MB
            MPFloatingPoint* mp = FindMPTable();
            if (mp == null)
            {
                Console.WriteLine("[MPTABLE] MPFloatingPoint table not found");
                return;
            }

            MPConfigTable* cfg = (MPConfigTable*)mp->ConfigTable;

            // Check if valid
            if (cfg->Signature[0] != 'P' ||
                cfg->Signature[1] != 'C' ||
                cfg->Signature[2] != 'M' ||
                cfg->Signature[3] != 'P' ||
                !Util.ZeroCheckSum(cfg, cfg->Length))
            {
                Console.WriteLine("[MPTABLE] MPConfigTable checksum or signature incorrect");
                return;
            }

            bool[] isBusPCI = new bool[32];
            
            uint address = (uint)cfg + (uint)sizeof(MPConfigTable);
            for (int i = 0; i < cfg->EntryCount; i++)
            {
                MPEntryType type = (MPEntryType)(*(byte*)address);

                switch (type)
                {
                    case MPEntryType.Processor:
                        address += 20;
                        break;

                    case MPEntryType.IOApic:
                    case MPEntryType.LocalInterruptAssignment:
                        address += 8;
                        break;

                    case MPEntryType.Bus:
                        MPBusEntry* bus = (MPBusEntry*)address;

                        // We are only really interested in the PCI busses
                        if (checkBusType(bus->Type, "PCI   "))
                        {
                            if(bus->BusID>=32)
                            {
                                Console.WriteLine("[MPTABLE] PCI bus with ID >= 32");
                            }
                            else
                            {
                                isBusPCI[bus->BusID] = true;
                            }
                        }

                        address += 8;
                        break;
                        
                    case MPEntryType.IOInterruptAssignment:
                        //MPIOInterruptEntry* io = (MPIOInterruptEntry*)address;

                        //if(isBusPCI[io->SourceBusID])
                        {
                            /*int pin = io->SourceBusIRQ & 0x3;
                            int slot = (io->SourceBusIRQ >> 2)&0x0F;

                            Console.Write("PCI[");
                            Console.WriteNum(pin);
                            Console.Write(' ');
                            Console.WriteNum(slot);
                            Console.Write(' ');
                            Console.WriteNum(io->DestinationApicINTNO);
                            Console.Write(' ');
                            Console.WriteNum(io->DestinationApicID);
                            Console.Write("]\t\t");*/

                            //IOApicManager.blar(io->SourceBusIRQ, io->DestinationApicINTNO);
                        }

                        address += 8;
                        break;
                }
            }
            
            Heap.Free(isBusPCI);
        }
    }
}
