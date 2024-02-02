using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Core.Models.Common;

namespace Stock.Application.Common.Models
{
    public record PageRequestDto
    {
        public int Page { get; set; }
        public int Take { get; set; }
    }

    public static class PageRequestExtensions
    {
        public static PageQuery ToPageQuery(this PageRequestDto pageRequestDto)
        {
            return new PageQuery(pageRequestDto.Page, pageRequestDto.Take);
        }
    }

    public class PageRequestDtoValidator : AbstractValidator<PageRequestDto>
    {
        public PageRequestDtoValidator()
        {
            RuleFor(i => i.Page)
                .GreaterThanOrEqualTo(1);
            
            RuleFor(i => i.Take)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(100);
        }
    }
}
