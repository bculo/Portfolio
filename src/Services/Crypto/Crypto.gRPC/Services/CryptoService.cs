using Crypto.Application.Modules.Crypto.Queries;
using Crypto.gRPC.Protos.v1;
using Grpc.Core;
using MediatR;

namespace Crypto.gRPC.Services
{
    public class CryptoService(IMediator mediator) : Protos.v1.Crypto.CryptoBase
    {
        public override async Task<FetchCryptoItemRequestResponse> FetchCryptoItem(FetchCryptoItemRequest request, ServerCallContext context)
        {
            var result = await mediator.Send(new FetchSingleQuery(request.Symbol));

            return new FetchCryptoItemRequestResponse
            {
                Name = result.Name,
                Price = (float)result.Price,
                Symbol = result.Symbol,
            };
        }
    }
}
