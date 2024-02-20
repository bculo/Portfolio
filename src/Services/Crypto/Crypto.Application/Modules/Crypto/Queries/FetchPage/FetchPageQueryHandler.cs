using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
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

        public async Task<IEnumerable<FetchPageResponseDto>> Handle(FetchPageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var items = await _work.CryptoRepository.GetPageWithPrices(request.Page, request.Take);
            //
            // var dtos = _mapper.Map<List<FetchPageResponseDto>>(items);
            //
            // return dtos;
        }
    }
}
