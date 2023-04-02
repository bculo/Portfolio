using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayCommand : IRequest<Guid>
    {
        public string Symbol { get; set; }
    }
}
