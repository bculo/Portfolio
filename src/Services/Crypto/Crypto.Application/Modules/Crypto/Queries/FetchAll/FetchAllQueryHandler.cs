using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAll
{
    public class FetchAllQueryHandler : IRequestHandler<FetchAllQuery, List<FetchAllResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchAllQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<List<FetchAllResponseDto>> Handle(FetchAllQuery request, CancellationToken cancellationToken)
        {
            var items = await _work.CryptoRepository.GetAll();

            var dtos = _mapper.Map<List<FetchAllResponseDto>>(items);

            return dtos;
        }
    }
}
