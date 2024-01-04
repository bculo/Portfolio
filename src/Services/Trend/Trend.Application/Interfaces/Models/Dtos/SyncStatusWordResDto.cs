using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public record SyncStatusWordResDto
    {
        public string ContextTypeName { get; init; }
        public int ContextTypeId { get; init; }
        public string Word { get; init; }
    }
}
