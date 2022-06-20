using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class SearchWord : IDocumentRoot
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool IsActive { get; set; }
        public string Word { get; set; }
        public SearchEngine Engine { get; set; }
        public ContextType Type { get; set; }
    }
}
