namespace Trend.Domain.Errors;

public static class SearchWordErrors
{
    public static readonly TrendError EmptyId = new("SearchWord.Empty", "Search word ID is empty");
    public static readonly TrendError Exists = new("SearchWord.Exists", "Search word with given engine exists");
    public static readonly TrendError NotFound = new("SearchWord.NotFound", "Search word not found");
}