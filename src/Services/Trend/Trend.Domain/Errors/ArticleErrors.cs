namespace Trend.Domain.Errors;

public static class ArticleErrors
{
    public static ValidationError ValidationError(IDictionary<string, string[]> errors) => new(
        "Article.ValidationErrors", "ValidationErrorOccured", errors);
    
    public static readonly NotFoundError NotFound = new("Article.NotFound", "Article not found");
}