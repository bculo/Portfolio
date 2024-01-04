using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public record SyncStatusResDto
    {
        public string Id { get; init; }
        public DateTime Started { get; init; }
        public DateTime Finished { get; init; }
        public int TotalRequests { get; init; }
        public int SucceddedRequests { get; init; }
        public List<SyncStatusWordResDto> SearchWords { get; init; }
    }
}
