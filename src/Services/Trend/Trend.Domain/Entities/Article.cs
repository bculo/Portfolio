using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class Article : AuditableDocument
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PageSource { get; set; }
        public string ArticleUrl { get; set; }
        public string Text { get; set; }
        public string SyncStatusId { get; set; }
        public string SearchWordId { get; set; }
    }
}
