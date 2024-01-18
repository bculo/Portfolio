namespace Trend.Domain.Errors;

public static class SearchWordErrors
{
    public static readonly CoreError EmptyId = new("SearchWord.Empty", "Search word ID is empty");
    public static readonly CoreError Exists = new("SearchWord.Exists", "Search word with given engine exists");
    public static readonly NotFoundError NotFound = new("SearchWord.NotFound", "Search word not found");
}