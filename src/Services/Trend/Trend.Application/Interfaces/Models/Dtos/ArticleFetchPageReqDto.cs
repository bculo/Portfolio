using Dtos.Common;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class ArticleFetchPageReqDto : PageRequestDto
    {
        public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}
