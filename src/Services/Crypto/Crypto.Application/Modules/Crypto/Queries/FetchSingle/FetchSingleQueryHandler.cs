﻿using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryHandler : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public FetchSingleQueryHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _work = work;
        }

        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken cancellationToken)
        {
            var items = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            var dto = _mapper.Map<FetchSingleResponseDto>(items);

            return dto;
        }
    }
}
