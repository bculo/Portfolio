using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models;

public class SearchArticleReqQuery
{
    public ContextType Type { get; set; }
}

public class SearchWordFilterReqQuery : PageReqQuery
{
    public ActiveFilter Active { get; set; }
    public ContextType ContextType { get; set; }
    public SearchEngine SearchEngine { get; set; }
    public string Query { get; set; }
    public SortType Sort { get; set; }
}

public class SearchWordSyncDetailResQuery
{
    public string WordId { get; set; }
    public int Count { get; set; }
}