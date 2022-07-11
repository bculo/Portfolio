using Crypto.Application.Constants.Regex;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryValidator : AbstractValidator<FetchPriceHistoryQuery>
    {
        public FetchPriceHistoryQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.SYMBOL)
                .NotEmpty();
        }   
    }
}
