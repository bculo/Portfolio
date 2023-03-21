using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _work;

        public DeleteCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());

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
