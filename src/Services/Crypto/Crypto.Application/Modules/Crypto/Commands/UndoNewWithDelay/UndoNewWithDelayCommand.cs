using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay
{
    public class UndoNewWithDelayCommand : IRequest
    {
        public Guid TemporaryId { get; set; }
    }
}
