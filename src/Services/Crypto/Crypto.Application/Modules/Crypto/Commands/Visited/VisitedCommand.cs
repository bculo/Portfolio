using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.Visited
{
    public class VisitedCommand : IRequest
    {
        public Guid CryptoId { get; set; }
        public string Symbol { get; set; } = default!;
    }
}
