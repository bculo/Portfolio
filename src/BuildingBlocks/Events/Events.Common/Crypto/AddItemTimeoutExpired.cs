using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class AddItemTimeoutExpired
    {
        public Guid TemporaryId { get; set; }
        public string Symbol { get; set; } = default!;
    }
}
