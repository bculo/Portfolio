using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
    {
        public UpdateInfoCommandValidator()
        {
            RuleFor(i => i.Symbol).WithSymbolRule();
        }
    }
}
