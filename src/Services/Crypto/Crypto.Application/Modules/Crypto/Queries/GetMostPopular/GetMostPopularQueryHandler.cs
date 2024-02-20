using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryHandler : IRequestHandler<GetMostPopularQuery, List<GetMostPopularResponse>>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public GetMostPopularQueryHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<List<GetMostPopularResponse>> Handle(GetMostPopularQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var response = await _work.CryptoRepository.GetMostPopular(request.Take);
            //
            // if(!response.Any())
            // {
            //     return new List<GetMostPopularResponse>();
            // }
            //
            // return _mapper.Map<List<GetMostPopularResponse>>(response);
        }
    }
}
