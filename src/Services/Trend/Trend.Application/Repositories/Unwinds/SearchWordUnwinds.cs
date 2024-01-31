using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Unwinds;

public class SearchWordSyncStatusUnwind
{
    public string Id { get; set; } = default!;
    public SearchWord SearchWords { get; set; } = default!;
}