namespace Trend.Domain.Errors;

public static class SyncErrors
{
    public static ValidationError ValidationError(IDictionary<string, string[]> errors) => new(
        "SearchWord.ValidationErrors", "Validation error occurred", errors);
    
    public static readonly CoreError NoSearchWords = new("Sync.NoWords", "Zero search for sync execution");
    public static readonly NotFoundError NotFound = new("Sync.NotFound", "Sync not found");
}