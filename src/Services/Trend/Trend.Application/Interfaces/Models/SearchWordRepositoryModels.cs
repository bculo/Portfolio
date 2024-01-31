using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models;

public class SearchArticleReqQuery
{
    public ContextType Type { get; set; } = default!;
}

public class SearchWordFilterReqQuery : PageReqQuery
{
    public ActiveFilter Active { get; set; } = default!;
    public ContextType ContextType { get; set; } = default!;
    public SearchEngine SearchEngine { get; set; } = default!;
    public string Query { get; set; } = default!;
    public SortType Sort { get; set; } = default!;
}

public class SearchWordSyncDetailResQuery
{
    public string WordId { get; set; } = default!;
    public int Count { get; set; }
}