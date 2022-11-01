using Crypto.Application.Modules.Crypto.Queries.FetchAll;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using MediatR;

namespace Crypto.GraphQL
{
    public class Query
    {      
        public async Task<IEnumerable<FetchAllResponseDto>> GetAllResponseDto([Service] IMediator mediator)
        {
            return await mediator.Send(new FetchAllQuery { });
        }
        
        public async Task<FetchSingleResponseDto> GetSingleResponseDto(string symbol, [Service] IMediator mediator)
        {
            return await mediator.Send(new FetchSingleQuery { Symbol = symbol });
        }
    }
}
