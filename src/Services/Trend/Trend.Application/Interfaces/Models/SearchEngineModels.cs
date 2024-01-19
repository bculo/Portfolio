using Trend.Domain.Entities;

namespace Trend.Application.Interfaces.Models;

public class SearchEngineReq
{
    public string SearchWordId { get; set; }
    public string SearchWord { get; set; }
}

public class SearchEngineRes
{
    public SyncStatus SyncIteration { get; set; }
    public List<Article> Articles { get; set; }

    public SearchEngineRes(SyncStatus iteration, List<Article> articles)
    {
        SyncIteration = iteration;
        Articles = articles;
    }
}