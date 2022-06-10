using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class Info : IDocument
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
    }
}
