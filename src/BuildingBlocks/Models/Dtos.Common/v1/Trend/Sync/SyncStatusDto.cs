using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend.Sync
{
    public class SyncStatusDto
    {
        public string Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public int TotalRequests { get; set; }
        public int SucceddedRequests { get; set; }
        public List<SyncStatusWordDto> SearchWords { get; set; }
    }
}
