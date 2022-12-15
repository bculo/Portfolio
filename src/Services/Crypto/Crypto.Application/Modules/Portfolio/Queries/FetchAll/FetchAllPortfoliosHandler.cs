using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Queries.FetchAll
{
    public class FetchAllPortfoliosHandler : IRequestHandler<FetchAllPortfoliosQuery, IEnumerable<FetchAllPortfoliosDto>>
    {
        private readonly ILogger<FetchAllPortfoliosHandler> _logger;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public FetchAllPortfoliosHandler(ILogger<FetchAllPortfoliosHandler> logger, 
            IUnitOfWork work,
            IMapper mapper)
        {
            _logger = logger;
            _work = work;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FetchAllPortfoliosDto>> Handle(FetchAllPortfoliosQuery request, CancellationToken cancellationToken)
        {
            //TODO get user ID from service
            string userId = string.Empty;

            var portfolios = await _work.PortfolioRepositry.GetPortfolios(userId);

            if (!portfolios.Any())
            {
                return Enumerable.Empty<FetchAllPortfoliosDto>();
            }

            var dtos = _mapper.Map<IEnumerable<FetchAllPortfoliosDto>>(portfolios);

            return dtos;
        }
    }
}
