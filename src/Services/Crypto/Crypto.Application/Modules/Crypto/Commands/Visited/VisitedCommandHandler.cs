using Crypto.Core.Interfaces;
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

        public async Task Handle(VisitedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Crypto with {0} visited", request.CryptoId);

            await _work.VisitRepository.Add(new Core.Entities.Visit
            {
                CryptoId = request.CryptoId
            });

            await _work.Commit();
        }
    }
}
