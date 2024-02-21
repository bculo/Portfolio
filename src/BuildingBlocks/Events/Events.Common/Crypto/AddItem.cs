using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class AddItem
    {
        public string Symbol { get; set; }  = default!;
        public Guid TemporaryId { get; set; }
    }
}
