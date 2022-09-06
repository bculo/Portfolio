using Crypto.Application.Modules.Crypto.Queries.FetchAll;
using MediatR;

namespace Crypto.GraphQL
{
    public class Query
    {
        public async Task<List<FetchAllResponseDto>> GetFetchAllResponseDto([Service] IMediator mediator)
        {
            return await mediator.Send(new FetchAllQuery { });
        }
    }
}
