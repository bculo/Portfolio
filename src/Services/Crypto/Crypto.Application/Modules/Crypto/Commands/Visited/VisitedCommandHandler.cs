using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.Visited
{
    public class VisitedCommandHandler(IUnitOfWork work)
        : IRequestHandler<VisitedCommand>
    {
        public async Task Handle(VisitedCommand request, CancellationToken ct)
        {
            await work.VisitRepo.Add(new VisitEntity
            {
                CryptoId = request.CryptoId
            }, ct);

            await work.Commit(ct);
        }
    }
}
