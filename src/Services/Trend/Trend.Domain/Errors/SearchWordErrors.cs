namespace Trend.Domain.Errors;

public static class SearchWordErrorCodes
{
    public const string FilterError = "SearchWord.ValidationErrors";
    public const string NotFound = "SearchWord.NotFound";
}


public static class SearchWordErrors
{
    public static ValidationError ValidationError(IDictionary<string, string[]> errors) => new(
        SearchWordErrorCodes.FilterError, "Validation error occurred", errors);
    
    public static readonly NotFoundError NotFound = new(SearchWordErrorCodes.NotFound, "Search word not found");
}