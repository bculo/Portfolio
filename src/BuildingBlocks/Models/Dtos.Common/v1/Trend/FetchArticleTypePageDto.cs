using Dtos.Common.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend
{
    public class FetchArticleTypePageDto : PageRequestDto
    {
        public int Type { get; set; }
    }
}
