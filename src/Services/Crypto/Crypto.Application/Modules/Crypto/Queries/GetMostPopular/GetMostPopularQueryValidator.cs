using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryValidator : AbstractValidator<GetMostPopularQuery>
    {
        public GetMostPopularQueryValidator()
        {
            RuleFor(i => i.Take).GreaterThan(0);
        }
    }
}
