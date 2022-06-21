using Dtos.Common.Shared;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Validators.Common
{
    public class PageRequestDtoBaseValidator<T> : AbstractValidator<T> where T : PageRequestDto
    {
        public PageRequestDtoBaseValidator()
        {
            RuleFor(i => i.Page).GreaterThan(0);
            RuleFor(i => i.Take).GreaterThan(0);
        }
    }

    public class PageRequestDtoValidator : PageRequestDtoBaseValidator<PageRequestDto>
    {
        public PageRequestDtoValidator() : base()
        {

        }
    }
}
