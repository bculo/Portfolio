using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryHandler : IRequestHandler<FetchPageQuery, IEnumerable<FetchPageResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchPageQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<IEnumerable<FetchPageResponseDto>> Handle(FetchPageQuery request, CancellationToken ct)
        {
            var items = await _work.CryptoPriceRepo.GetPage(new PageQuery(request.Page, request.Take), ct);
            return _mapper.Map<List<FetchPageResponseDto>>(items);
        }
    }
}
