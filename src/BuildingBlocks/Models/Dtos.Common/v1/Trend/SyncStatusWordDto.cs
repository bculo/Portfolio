using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend
{
    public class SyncStatusWordDto
    {
        public string ContextTypeName { get; set; }
        public int ContextTypeId { get; set; }
        public string Word { get; set; }
    }
}
