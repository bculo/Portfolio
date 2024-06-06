using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQuery : IRequest<List<GetMostPopularResponse>>
    {
        public int Take { get; set; }
    }
}
