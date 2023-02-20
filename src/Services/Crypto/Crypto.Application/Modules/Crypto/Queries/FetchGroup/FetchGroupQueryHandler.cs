using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchGroup
{
    public class FetchGroupQueryHandler : IRequestHandler<FetchGroupQuery, IEnumerable<FetchGroupResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchGroupQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<IEnumerable<FetchGroupResponseDto>> Handle(FetchGroupQuery request, CancellationToken cancellationToken)
        {
            if(request.Symbols == null || !request.Symbols.Any())
            {
                return Enumerable.Empty<FetchGroupResponseDto>();
            }

            var items = await _work.CryptoRepository.GetGroupWithPrices(request.Symbols, request.Page, request.Take);

            var dtos = _mapper.Map<List<FetchGroupResponseDto>>(items);

            return dtos;
        }
    }
}
