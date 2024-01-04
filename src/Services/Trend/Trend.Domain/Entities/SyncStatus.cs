
namespace Trend.Domain.Entities
{
    public class SyncStatus : RootDocument
    {
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public int TotalRequests { get; set; }
        public int SucceddedRequests { get; set; }
        public int BadRequests => TotalRequests - SucceddedRequests;
        public List<SyncStatusWord> UsedSyncWords { get; set; } = new();
    }
}
