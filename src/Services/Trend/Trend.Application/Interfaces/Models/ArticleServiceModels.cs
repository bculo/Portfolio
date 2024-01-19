using Dtos.Common;

namespace Trend.Application.Interfaces.Models;

public record FilterArticlesReqDto : PageRequestDto
{
    public int Context { get; set; }
    public int Activity { get; set; }
}

public record ActivateArticleReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record DeactivateArticleReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record ArticleResDto
{
    public string Id { get; set; }
    public DateTime Created { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Url { get; set; }
    public string PageSource { get; set; }
    public string TypeName { get; set; }
    public int TypeId { get; set; }
    public string SearchWordId { get; set; }
    public string SearchWord { get; set; }
    public string SearchWordImage { get; set; }
}