using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePrice
{
    public class UpdatePriceCommand : IRequest
    {
        public string Symbol { get; set; } = default!;
    }
}
