using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class Article : IDocumentRoot
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PageSource { get; set; }
        public string ArticleUrl { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public string SyncStatusId { get; set; }
        public DateTime Created { get; set; }
        public ContextType Type { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
