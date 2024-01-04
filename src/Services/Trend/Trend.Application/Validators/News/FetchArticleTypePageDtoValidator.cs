using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Validators.Common;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.News
{
    public class FetchArticleTypePageDtoValidator : PageRequestDtoBaseValidator<ArticleFetchPageReqDto>
    {
        public FetchArticleTypePageDtoValidator() : base()
        {
            RuleFor(i => i.Type).Must(engine => Enum.GetValues<ContextType>().Cast<int>().Contains(engine))
                .WithMessage("Selected context type not available");
        }
    }
}
