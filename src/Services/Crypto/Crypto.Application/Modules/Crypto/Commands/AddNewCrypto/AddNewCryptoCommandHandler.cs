using AutoMapper;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandHandler : IRequestHandler<AddNewCryptoCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public AddNewCryptoCommandHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddNewCryptoCommand request, CancellationToken cancellationToken)
        {
            var items = await _work.CryptoRepository.Find(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if(items.Any())
            {
                throw new Exception("Item with given symbol already exists");
            }

            var newInstance = _mapper.Map<Core.Entities.Crypto>(request);

            await _work.CryptoRepository.Add(newInstance);
            await _work.Commit();

            return Unit.Value;
        }
    }
}
