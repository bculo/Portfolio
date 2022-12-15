using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Commands.Add
{
    public class AddPorftolioCommandValidator : AbstractValidator<AddPorftolioCommand>
    {
        public AddPorftolioCommandValidator()
        {
            RuleFor(c => c.Name).MinimumLength(3).NotEmpty();
        }
    }
}
