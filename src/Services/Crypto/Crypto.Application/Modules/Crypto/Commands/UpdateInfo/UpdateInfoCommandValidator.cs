using Crypto.Application.Constants.Regex;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
    {
        public UpdateInfoCommandValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.SYMBOL)
                .NotEmpty();
        }
    }
}
