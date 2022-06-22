using Dtos.Common.v1.Trend.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend.Sync
{
    public class GoogleSyncResultDto
    {
        public SyncStatusDto Status { get; set; }
        public List<ArticleGroupDto> SearchResult { get; set; }
    }
}
