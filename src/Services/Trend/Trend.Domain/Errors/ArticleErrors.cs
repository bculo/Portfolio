namespace Trend.Domain.Errors;

public static class ArticleErrors
{
    public static readonly CoreError EmptyId = new("Article.Empty", "Article ID is empty");
    public static readonly NotFoundError NotFound = new("Article.NotFound", "Article not found");
}