using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePrice
{
    public class UpdatePriceCommandValidator : AbstractValidator<UpdatePriceCommand>
    {
        public UpdatePriceCommandValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.Symbol)
                .NotEmpty();
        }
    }
}
