using AutoMapper;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryHandler : IRequestHandler<FetchPriceHistoryQuery, List<PriceHistoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchPriceHistoryQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<List<PriceHistoryDto>> Handle(FetchPriceHistoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if (entity is null)
            {
                throw new CryptoCoreException("Item with symbol {0} not found", request.Symbol);
            }

            var priceEntities = await _work.CryptoPriceRepository.Find(i => i.CryptoId == entity.Id);

            return _mapper.Map<List<PriceHistoryDto>>(priceEntities);
        }
    }
}
