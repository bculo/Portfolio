using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Common.Models
{
    public record PageRequest
    {
        public int Page { get; set; }
        public int Take { get; set; }
    }

    public class PageBaseValidator<T> : AbstractValidator<T> where T : PageRequest
    {
        public PageBaseValidator()
        {
            RuleFor(i => i.Page).GreaterThanOrEqualTo(1);
            RuleFor(i => i.Take).GreaterThanOrEqualTo(1);
        }
    }
}
