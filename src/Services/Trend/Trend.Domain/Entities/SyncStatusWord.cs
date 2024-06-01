using Trend.Domain.Enums;

namespace Trend.Domain.Entities
{
    public class SyncStatusWord
    {
        public ContextType Type { get; set; } = default!;
        public string WordId { get; set; } = default!;
    }
}
