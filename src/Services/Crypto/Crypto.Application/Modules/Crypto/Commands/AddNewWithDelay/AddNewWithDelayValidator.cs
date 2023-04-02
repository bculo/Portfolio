using Crypto.Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayValidator : AbstractValidator<AddNewWithDelayCommand>
    {
        public AddNewWithDelayValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.SYMBOL)
                .NotEmpty();
        }
    }
}
