using Dtos.Common;
namespace Trend.Application.Interfaces.Models.Dtos;

public record SearchWordFilterReqDto : PageRequestDto
{
    public int Active { get; init; }
    public int ContextType { get; init; }
    public int SearchEngine { get; init; }
    public string Query { get; init; }
    public int Sort { get; init; }
}