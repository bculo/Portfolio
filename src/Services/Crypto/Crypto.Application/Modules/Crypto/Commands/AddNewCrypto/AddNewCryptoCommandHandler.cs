using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandHandler : IRequestHandler<AddNewCryptoCommand>
    {
        public AddNewCryptoCommandHandler()
        {

        }

        public async Task<Unit> Handle(AddNewCryptoCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
