using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class Article : AuditableDocument
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string PageSource { get; set; } = default!;
        public string ArticleUrl { get; set; } = default!;
        public string Text { get; set; } = default!;
        public string SyncStatusId { get; set; } = default!;
        public string SearchWordId { get; set; } = default!;
    }
}
