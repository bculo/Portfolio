using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayCommand : IRequest<Guid>
    {
        public string Symbol { get; set; } = default!;
    }
}
