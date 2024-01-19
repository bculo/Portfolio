using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.News
{
    public class FetchArticleTypePageDtoValidator : PageRequestDtoBaseValidator<FetchArticlePageReqDto>
    {
        public FetchArticleTypePageDtoValidator() : base()
        {
            /*
            RuleFor(i => i.Type).Must(engine => Enum.GetValues<ContextType>().Cast<int>().Contains(engine))
                .WithMessage("Selected context type not available");
                */
        }
    }
}
