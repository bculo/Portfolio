using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class AddCryptoItemWithDelayTimeoutExpired
    {
        public Guid TemporaryId { get; set; }
        public string Symbol { get; set; } = default!;
    }
}
