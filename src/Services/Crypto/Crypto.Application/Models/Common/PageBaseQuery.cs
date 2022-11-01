using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Models.Common
{
    public class PageBaseQuery
    {
        public int Page { get; set; }
        public int Take { get; set; }
    }

    public class PageBaseQueryValidator<T> : AbstractValidator<T> where T : PageBaseQuery
    {
        public PageBaseQueryValidator()
        {
            RuleFor(i => i.Page).GreaterThan(0);
            RuleFor(i => i.Take).GreaterThan(0).LessThan(50);
        }
    }
}
