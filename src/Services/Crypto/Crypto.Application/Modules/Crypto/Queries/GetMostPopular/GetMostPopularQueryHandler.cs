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

        public async Task<List<GetMostPopularResponse>> Handle(GetMostPopularQuery request, CancellationToken ct)
        {
            var response = await _work.VisitRepo.GetMostPopular(request.Take, ct);
            return response.Count == 0 
                ? new List<GetMostPopularResponse>() 
                : _mapper.Map<List<GetMostPopularResponse>>(response);
        }
    }
}
