using Events.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class CryptoInfoUpdated
    {
        public long Id { get; set; }
        public string Symbol { get; set; }  = default!;
        public string Name { get; set; }  = default!;
        public string Website { get; set; }  = default!;
        public string Description { get; set; }  = default!;
    }
}
