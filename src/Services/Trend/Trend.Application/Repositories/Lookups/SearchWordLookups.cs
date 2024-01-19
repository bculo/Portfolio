using Trend.Application.Repositories.Unwinds;
using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Lookups;

public class SearchWordSyncStatusLookup
{
    public string Id { get; set; }
    public SyncStatusWord UsedSyncWords { get; set; }
    public List<SearchWord> SearchWords { get; set; }
}