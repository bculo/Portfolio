using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class SyncStatus : IDocumentRoot
    {
        public string Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public int TotalRequests { get; set; }
        public int SucceddedRequests { get; set; }
        public int BadRequests => TotalRequests - SucceddedRequests;
        public DateTime Created { get; set; }
        public List<SyncStatusWord> UsedSyncWords { get; set; }

        public SyncStatus()
        {
            UsedSyncWords = new List<SyncStatusWord>();
        }
    }
}
