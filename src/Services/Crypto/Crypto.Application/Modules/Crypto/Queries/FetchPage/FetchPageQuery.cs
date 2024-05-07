using Crypto.Application.Common.Models;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQuery : PageBaseQuery, IRequest<PageBaseResult<FetchPageResponseDto>>
    {
        public string? Symbol { get; set; }
    }
}
