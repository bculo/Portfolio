using Dtos.Common.v1.Trend.SearchWord;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.Sync
{
    public class SyncSettingCreateDtoValidator : AbstractValidator<SearchWordCreateDto>
    {
        public SyncSettingCreateDtoValidator()
        {
            RuleFor(i => i.SearchWord).MinimumLength(2).NotEmpty();

            RuleFor(i => i.SearchEngine).Must(engine =>
            {
                return Enum.GetValues<SearchEngine>().Cast<int>().Contains(engine);
            }).WithMessage("Selected search engine type not available");

            RuleFor(i => i.ContextType).Must(type =>
            {
                return Enum.GetValues<ContextType>().Cast<int>().Contains(type);
            }).WithMessage("Selected context type not available");
        }
    }
}
