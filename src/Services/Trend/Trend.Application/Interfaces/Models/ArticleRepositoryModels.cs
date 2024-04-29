using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models;

public class FilterArticlesReqQuery : PageReqQuery
{
    public ContextType Context { get; set; } = default!;
    public ActiveFilter Activity { get; set; } = default!;
    
    public string? Query { get; set; }
}

public class ArticleDetailResQuery
{
    public string Id { get; set; } = default!;
    public DateTime Created { get; set; }
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; }
    public string Content { get; set; } = default!;
    public string PageSource { get; set; } = default!;
    public string ArticleUrl { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string SearchWordId { get; set; } = default!;
    public string SearchWord { get; set; } = default!;
    public string SearchWordImage { get; set; } = default!;
    public ContextType ContextType { get; set; } = default!;
}