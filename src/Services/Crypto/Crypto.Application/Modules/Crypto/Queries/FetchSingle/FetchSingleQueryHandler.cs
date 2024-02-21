using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryHandler : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work; 
        private readonly IPublishEndpoint _publish;

        public FetchSingleQueryHandler(IMapper mapper, 
            IUnitOfWork work, 
            IPublishEndpoint publish)
        {
            _mapper = mapper;
            _work = work;
            _publish = publish;
        }

        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // FetchSingleResponseDto response = default;
            //
            // var entity = await _work.CryptoRepository.GetWithPrice(request.Symbol);
            // CryptoCoreException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");
            // response = _mapper.Map<FetchSingleResponseDto>(entity);
            // await PublishVisitedEvent(entity.Id, entity.Symbol);
            //
            // return response;
        }

        private async Task PublishVisitedEvent(long cryptoId, string symbol)
        {
            await _publish.Publish(new Visited
            {
                CryptoId = cryptoId,
                Symbol = symbol
            });
        }
    }
}
