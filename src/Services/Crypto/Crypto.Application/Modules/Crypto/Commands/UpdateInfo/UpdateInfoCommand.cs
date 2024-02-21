using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommand : IRequest
    {
        public string Symbol { get; set; } = default!;
    }
}
