using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public class AddNewCommand : IRequest
    {
        public string Symbol { get; set; }
    }
}
