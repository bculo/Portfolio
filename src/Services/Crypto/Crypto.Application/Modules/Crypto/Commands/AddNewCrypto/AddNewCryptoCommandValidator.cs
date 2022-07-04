using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandValidator : AbstractValidator<AddNewCryptoCommand>
    {
        public AddNewCryptoCommandValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches("^[a-zA-z]{1,15}$")
                .NotEmpty();
        }
    }
}
