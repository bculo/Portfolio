using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQuery : IRequest<FetchSingleResponseDto>
    {
        public string Symbol { get; set; } = default!;
    }
}
