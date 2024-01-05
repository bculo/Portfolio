using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models.Repositories;

public class SearchWordFilterReqQuery : PageReqQuery
{
    public ActiveFilter Active { get; set; }
    public ContextType ContextType { get; set; }
    public SearchEngine SearchEngine { get; set; }
    public string Query { get; set; }
    public SortType Sort { get; set; }
}