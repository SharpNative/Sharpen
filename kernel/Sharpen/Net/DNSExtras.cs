using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    struct DNSHeader
    {
        public ushort Identification;
        public ushort Flags;
        public ushort TotalQuestions;
        public ushort TotalAnsswersRR;
        public ushort TotalAuthorityRR;
        public ushort TotalAdditionalRR;
    }

    enum DNSQueryResponse: byte
    {
        Query,
        Response
    }

    enum DNSOpcode
    {
        QUERY,
        IQUERY,
        STATUS,
        BLANK,
        NOTIFY,
        UPDATE
    }

    enum DDAA: byte
    {
        NotAuthoritative,
        Authoritative
    }
}
