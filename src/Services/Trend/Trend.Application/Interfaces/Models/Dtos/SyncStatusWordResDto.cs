using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class SyncStatusWordResDto
    {
        public string ContextTypeName { get; set; }
        public int ContextTypeId { get; set; }
        public string Word { get; set; }
    }
}
