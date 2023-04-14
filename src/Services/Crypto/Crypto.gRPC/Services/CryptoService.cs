using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.gRPC.Protos;
using Grpc.Core;
using MediatR;

namespace Crypto.gRPC.Services
{  
    public class CryptoService : Protos.Crypto.CryptoBase
    {
        private readonly IMediator _mediator;

        public CryptoService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<FetchCryptoItemRequestResponse> FetchCryptoItem(FetchCryptoItemRequest request, ServerCallContext context)
        {
            var result = await _mediator.Send(new FetchSingleQuery { Symbol = request.Symbol });

            return new FetchCryptoItemRequestResponse
            {
                Name = result.Name,
                Price = (float)result.Price,
                Symbol = result.Symbol,
            };
        }
    }
}
