using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class SyncStatusResDto
    {
        public string Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public int TotalRequests { get; set; }
        public int SucceddedRequests { get; set; }
        public List<SyncStatusWordResDto> SearchWords { get; set; }
    }
}
