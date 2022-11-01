using AutoMapper;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryHandler : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work; 
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;

        public FetchSingleQueryHandler(IMapper mapper, 
            IUnitOfWork work, 
            IPublishEndpoint publish,
            IDateTime time)
        {
            _mapper = mapper;
            _work = work;
            _publish = publish;
            _time = time;
        }

        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.GetWithPrice(request.Symbol);

            if(entity is null)
            {
                throw new CryptoCoreException("Item with symbol {0} not found", request.Symbol);
            }

            var dto = _mapper.Map<FetchSingleResponseDto>(entity);

            await _publish.Publish(new CryptoVisited
            {
                CreatedOn = _time.DateTime,
                CryptoId = entity.Id!.Value,
                Symbol = entity.Symbol
            });

            return dto;
        }
    }
}
