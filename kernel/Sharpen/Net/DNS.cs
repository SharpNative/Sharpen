using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class DNS
    {

        const ushort FLAG_QR_RESPONSE = (1 << 0);
        const ushort FLAG_AA_AUTH = (1 << 5);
        const ushort FLAG_TC_TRUNC = (1 << 6);
        const ushort FLAG_RD__DES = (1 << 7);
        const ushort FLAG_RA_AVAI = (1 << 8);
        const ushort FLAG_AD_AUTH = (1 << 10);
        const ushort FLAG_CD_CHECK = (1 << 11);

        const ushort FLAG_RCODE = (1 << 12);
    }
}
