using Dtos.Common.v1.Trend;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Application.Validators
{
    public class SyncSettingCreateDtoValidator : AbstractValidator<SyncSettingCreateDto>
    {
        public SyncSettingCreateDtoValidator()
        {
            var test = Enum.GetValues<SearchEngine>().Cast<int>();

            RuleFor(i => i.SearchWord).MinimumLength(2).NotEmpty();
            RuleFor(i => i.SearchEngine).Must(engine =>
            {
                return Enum.GetValues<SearchEngine>().Cast<int>().Contains(engine);
            });
        }
    }
}
