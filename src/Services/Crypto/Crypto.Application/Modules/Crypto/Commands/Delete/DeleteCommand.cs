using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.Delete
{
    public class DeleteCommand : IRequest
    {
        public string? Symbol { get; set; }
    }
}
