using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Stock
{
    public class UpdateBatchPrepared
    {
        public int IterationId { get; set; }
        public List<string> Symbols { get; set; } = default!;
    }
}
