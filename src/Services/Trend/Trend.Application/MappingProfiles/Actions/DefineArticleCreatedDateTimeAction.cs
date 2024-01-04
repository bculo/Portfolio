using AutoMapper;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces.Models.Services.Google;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Actions
{
    public class DefineArticleCreatedDateTimeAction : IMappingAction<GoogleSearchEngineItemDto, Article>
    {
        private readonly IDateTimeProvider _time;

        public DefineArticleCreatedDateTimeAction(IDateTimeProvider time)
        {
            _time = time;
        }

        public void Process(GoogleSearchEngineItemDto source, Article destination, ResolutionContext context)
        {
            destination.IsActive = true;
            destination.Created = _time.Now;
        }
    }
}
