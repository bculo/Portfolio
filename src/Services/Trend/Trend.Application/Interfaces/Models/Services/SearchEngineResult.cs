using Trend.Domain.Entities;

namespace Trend.Application.Interfaces.Models.Services;

public class SearchEngineResult
{
    public SyncStatus SyncIteration { get; set; }
    public List<Article> Articles { get; set; }

    public SearchEngineResult(SyncStatus iteration, List<Article> articles)
    {
        SyncIteration = iteration;
        Articles = articles;
    }
}