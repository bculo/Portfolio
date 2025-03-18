using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands;

public record VisitedCommand(Guid CryptoId, string Symbol) : IRequest;

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