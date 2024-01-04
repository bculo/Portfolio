using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class SearchWord : AuditableDocument
    {
        public string Word { get; set; }
        public SearchEngine Engine { get; set; }
        public ContextType Type { get; set; }
        public string ImageUrl { get; set; }
    }
}
