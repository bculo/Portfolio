using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Unwinds;

public class ArticleSearchWordUnwind : Article
{
    public SearchWord SearchWords { get; set; }
}