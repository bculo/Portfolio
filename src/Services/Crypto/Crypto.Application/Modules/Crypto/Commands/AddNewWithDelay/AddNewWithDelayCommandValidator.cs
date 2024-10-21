using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayCommandValidator : AbstractValidator<AddNewWithDelayCommand>
    {
        public AddNewWithDelayCommandValidator()
        { 
            RuleFor(i => i.Symbol).WithSymbolRule();
        }
    }
}
