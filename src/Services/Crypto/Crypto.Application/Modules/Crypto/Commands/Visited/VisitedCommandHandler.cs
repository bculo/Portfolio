using Crypto.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.Visited
{
    public class VisitedCommandHandler : IRequestHandler<VisitedCommand>
    {
        private readonly ICryptoDbContext _work;
        private readonly ILogger<VisitedCommandHandler> _logger;

        public VisitedCommandHandler(ICryptoDbContext work, ILogger<VisitedCommandHandler> logger)
        {
            _work = work;
            _logger = logger;
        }

        public async Task<Unit> Handle(VisitedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Crypto with {0} visited", request.CryptoId);

            await _work.Visits.AddAsync(new Core.Entities.Visit
            {
                CryptoID = request.CryptoId
            });

            await _work.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
