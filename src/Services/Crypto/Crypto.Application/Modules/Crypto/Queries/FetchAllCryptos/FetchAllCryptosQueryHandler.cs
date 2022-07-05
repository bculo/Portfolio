using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos
{
    public class FetchAllCryptosQueryHandler : IRequestHandler<FetchAllCryptosQuery, List<FetchAllCryptosDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchAllCryptosQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<List<FetchAllCryptosDto>> Handle(FetchAllCryptosQuery request, CancellationToken cancellationToken)
        {
            var items = await _work.CryptoRepository.GetAll();

            var dtos = _mapper.Map<List<FetchAllCryptosDto>>(items);

            return dtos;
        }
    }
}
