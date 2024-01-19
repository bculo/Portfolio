using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models;

public class ArticleDetailResQuery
{
    public string Id { get; set; }
    public DateTime Created { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string PageSource { get; set; }
    public string ArticleUrl { get; set; }
    public string Text { get; set; }
    public string SearchWordId { get; set; }
    public string SearchWord { get; set; }
    public string SearchWordImage { get; set; }
    public ContextType ContextType { get; set; }
}