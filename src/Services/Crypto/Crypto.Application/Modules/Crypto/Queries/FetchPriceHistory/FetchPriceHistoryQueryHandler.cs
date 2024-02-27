using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Exceptions;
using Crypto.Core.ReadModels;
using MediatR;

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

        public async Task<List<PriceHistoryDto>> Handle(FetchPriceHistoryQuery request, CancellationToken ct)
        {
            var items = await _work.TimeFrameRepo.GetSingle(request.CryptoId, 
                new TimeFrameQuery(43200, 30), 
                ct);

            return _mapper.Map<List<PriceHistoryDto>>(items);
        }
    }
}
