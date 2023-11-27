using Dtos.Common.v1.Trend.Article;


namespace Dtos.Common.v1.Trend.Sync
{
    public class SyncResultDto
    {
        public SyncStatusDto Status { get; set; }
        public List<ArticleGroupDto> SearchResult { get; set; }
    }
}
