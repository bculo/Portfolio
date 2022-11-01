using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var items = await _work.CryptoRepository.GetPageWithPrices(request.Page, request.Take);

            var dtos = _mapper.Map<List<FetchPageResponseDto>>(items);

            return dtos;
        }
    }
}
