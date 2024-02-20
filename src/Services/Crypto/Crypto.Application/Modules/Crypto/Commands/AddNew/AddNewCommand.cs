using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public record AddNewCommand : IRequest
    {
        public string Symbol { get; init; } = default!;
    }
}
