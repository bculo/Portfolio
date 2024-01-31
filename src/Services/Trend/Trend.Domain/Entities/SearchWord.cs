using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class SearchWord : AuditableDocument
    {
        public string Word { get; set; } = default!;
        public SearchEngine Engine { get; set; } = default!;
        public ContextType Type { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
