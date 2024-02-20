using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryHandler : IRequestHandler<FetchPriceHistoryQuery, IEnumerable<PriceHistoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchPriceHistoryQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<IEnumerable<PriceHistoryDto>> Handle(FetchPriceHistoryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
//             var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());
//
//             if (entity is null)
//             {
//                 throw new CryptoCoreException("Item with symbol {0} not found", request.Symbol);
//             }
//
//             throw new NotImplementedException();
//             
//             /*
//             var priceEntities = await _work.CryptoPriceRepository.Find(i => i.CryptoId == entity.Id);
//
//             return _mapper.Map<List<PriceHistoryDto>>(priceEntities);
//             */
        }
    }
}
