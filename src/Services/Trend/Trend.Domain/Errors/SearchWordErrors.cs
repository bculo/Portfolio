namespace Trend.Domain.Errors;

public static class SearchWordErrors
{
    public static ValidationError ValidationError(IDictionary<string, string[]> errors) => new(
        "SearchWord.ValidationErrors", "Validation error occurred", errors);
    
    public static readonly NotFoundError NotFound = new("SearchWord.NotFound", "Search word not found");
}