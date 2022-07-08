using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.DeleteCrypto
{
    public class DeleteCryptoCommandHandler : IRequestHandler<DeleteCryptoCommand>
    {
        private readonly IUnitOfWork _work;

        public DeleteCryptoCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<Unit> Handle(DeleteCryptoCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if(item is null)
            {
                throw new CryptoCoreException("Item not found");
            }

            await _work.CryptoRepository.Remove(item);
            await _work.Commit();

            return Unit.Value;
        }
    }
}
