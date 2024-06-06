using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay
{
    public class UndoNewWithDelayCommand : IRequest
    {
        public Guid TemporaryId { get; set; }
    }
}
