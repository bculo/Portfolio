using Crypto.Application.Interfaces.Persistence;
using Crypto.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Modules.Crypto.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
    {
        private readonly ICryptoDbContext _work;

        public DeleteCommandHandler(ICryptoDbContext work)
        {
            _work = work;
        }

        public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.Cryptos.FirstOrDefaultAsync(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());

            if(item is null)
            {
                throw new CryptoCoreException("Item not found");
            }

            await _work.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
