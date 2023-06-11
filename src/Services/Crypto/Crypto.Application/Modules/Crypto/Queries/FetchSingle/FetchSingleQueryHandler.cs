using AutoMapper;
using Cache.Abstract.Contracts;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
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
        private readonly ICacheService _cache;

        public FetchSingleQueryHandler(IMapper mapper, 
            IUnitOfWork work, 
            IPublishEndpoint publish,
            ICacheService cache)
        {
            _mapper = mapper;
            _work = work;
            _publish = publish;
            _cache = cache;
        }

        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken cancellationToken)
        {
            FetchSingleResponseDto response = default;

            var cacheInstance = await _cache.Get<CryptoPriceUpdated>(request.Symbol);
            if(cacheInstance is not null)
            {
                response = _mapper.Map<FetchSingleResponseDto>(cacheInstance);
                await PublishVisitedEvent(cacheInstance.Id, cacheInstance.Symbol);
                return response;
            } 

            var entity = await _work.CryptoRepository.GetWithPrice(request.Symbol);
            CryptoCoreException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");
            response = _mapper.Map<FetchSingleResponseDto>(entity);
            await PublishVisitedEvent(entity.Id.Value, entity.Symbol);

            return response;
        }

        private async Task PublishVisitedEvent(long cryptoId, string symbol)
        {
            await _publish.Publish(new CryptoVisited
            {
                CryptoId = cryptoId,
                Symbol = symbol
            });
        }
    }
}
