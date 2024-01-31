using Dtos.Common;

namespace Trend.Application.Interfaces.Models;

public record FilterArticlesReqDto : PageRequestDto
{
    public int Context { get; set; } = default!;
    public int Activity { get; set; } = default!;
}

public record ActivateArticleReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record DeactivateArticleReqDto(string Id) : GetItemByStringIdReqDto(Id);

public record ArticleResDto
{
    public string Id { get; set; } = default!;
    public DateTime Created { get; set; } 
    public string Title { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string PageSource { get; set; } = default!;
    public string TypeName { get; set; } = default!;
    public int TypeId { get; set; }
    public string SearchWordId { get; set; } = default!;
    public string SearchWord { get; set; } = default!;
    public string SearchWordImage { get; set; } = default!;
}