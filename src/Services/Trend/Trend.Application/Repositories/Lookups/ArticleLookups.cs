using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Lookups;

public class ArticleSearchWordLookup : Article
{
    public List<SearchWord> SearchWords { get; set; } = default!;
}