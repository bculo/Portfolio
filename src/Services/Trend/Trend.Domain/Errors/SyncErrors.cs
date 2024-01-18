namespace Trend.Domain.Errors;

public static class SyncErrors
{
    public static readonly CoreError EmptyId = new("Sync.Empty", "Sync ID is empty");
    public static readonly CoreError NoSearchWords = new("Sync.NoWords", "Zero search for sync execution");
    public static readonly NotFoundError NotFound = new("Sync.NotFound", "Sync not found");
}