using Events.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class CryptoInfoUpdated : IntegrationEvent
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string? Website { get; set; }
        public string Description { get; set; }
    }
}
