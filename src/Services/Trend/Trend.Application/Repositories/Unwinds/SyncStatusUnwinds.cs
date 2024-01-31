using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Unwinds;

public class SyncStatusUnwind
{
    public string Id { get; set; } = default!;
    public SyncStatusWord UsedSyncWords { get; set; } = default!;
}