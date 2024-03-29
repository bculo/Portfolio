﻿using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly IPublishEndpoint _publish;

        public DeleteCommandHandler(IUnitOfWork work, IPublishEndpoint publish)
        {
            _work = work;
            _publish = publish;
        }

        public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var item = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());
            //
            // CryptoCoreException.ThrowIfNull(item, "Item not found");
            //
            // await _work.CryptoRepository.Remove(item);
            // await _work.Commit();
            //
            // await _publish.Publish(new CryptoItemDeleted
            // {
            //     Symbol = request.Symbol
            // });
            
        }
    }
}
