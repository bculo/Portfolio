using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Unwinds;

public class SearchWordSyncStatusUnwind
{
    public string Id { get; set; }
    public SearchWord SearchWords { get; set; }
}