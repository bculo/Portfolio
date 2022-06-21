using Dtos.Common.v1.Trend;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.News
{
    public class FetchLatestNewsPageDtoValidator : PageRequestDtoBaseValidator<FetchLatestNewsPageDto>
    {
        public FetchLatestNewsPageDtoValidator() : base()
        {

        }
    }
}
