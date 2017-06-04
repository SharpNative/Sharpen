using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem.PartitionTables
{
    interface IPartitionTable
    {

        bool isType(Node node);
    }
}
