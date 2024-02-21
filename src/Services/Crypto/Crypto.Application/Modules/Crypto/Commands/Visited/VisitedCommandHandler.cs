using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.Visited
{
    public class VisitedCommandHandler : IRequestHandler<VisitedCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ILogger<VisitedCommandHandler> _logger;

        public VisitedCommandHandler(IUnitOfWork work, ILogger<VisitedCommandHandler> logger)
        {
            _work = work;
            _logger = logger;
        }

        public async Task Handle(VisitedCommand request, CancellationToken ct)
        {
            await _work.VisitRepo.Add(new Visit
            {
                CryptoId = request.CryptoId
            }, ct);

            await _work.Commit(ct);
        }
    }
}
