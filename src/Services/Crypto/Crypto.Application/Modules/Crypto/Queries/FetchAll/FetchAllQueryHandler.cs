using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAll
{
    public class FetchAllQueryHandler : IRequestHandler<FetchAllQuery, IEnumerable<FetchAllResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchAllQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<IEnumerable<FetchAllResponseDto>> Handle(FetchAllQuery request, CancellationToken cancellationToken)
        {
            var items = await _work.CryptoRepository.GetAllWithPrice();

            var dtos = _mapper.Map<List<FetchAllResponseDto>>(items);

            return dtos;
        }
    }
}
