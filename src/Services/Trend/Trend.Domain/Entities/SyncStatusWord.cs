using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class SyncStatusWord
    {
        public ContextType Type { get; set; }
        public string WordId { get; set; }
    }
}
