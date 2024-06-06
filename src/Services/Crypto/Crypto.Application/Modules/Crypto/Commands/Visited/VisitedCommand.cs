using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.Visited
{
    public class VisitedCommand : IRequest
    {
        public Guid CryptoId { get; set; }
        public string Symbol { get; set; } = default!;
    }
}
