﻿using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public class AddNewCommandValidator : AbstractValidator<AddNewCommand>
    {
        public AddNewCommandValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.Symbol)
                .NotEmpty();
        }
    }
}
