using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Commands.Add
{
    public class AddCommand : IRequest<string>
    {
        public string? Name { get; set; }
    }
}
