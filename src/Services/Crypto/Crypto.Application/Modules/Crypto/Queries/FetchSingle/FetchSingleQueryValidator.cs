using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryValidator : AbstractValidator<FetchSingleQuery>
    {
        public FetchSingleQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches("^[a-zA-z]{1,15}$")
                .NotEmpty();
        }
    }
}
