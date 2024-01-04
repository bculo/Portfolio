using AutoMapper;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.MappingProfiles.Actions
{
    public class DefineSyncSettingCreatedDateTimeAction : IMappingAction<SearchWordAddReqDto, Domain.Entities.SearchWord>
    {
        private readonly IDateTimeProvider _time;

        public DefineSyncSettingCreatedDateTimeAction(IDateTimeProvider time)
        {
            _time = time;
        }

        public void Process(SearchWordAddReqDto source, Domain.Entities.SearchWord destination, ResolutionContext context)
        {
            destination.Created = _time.Now;
            destination.IsActive = true;
        }
    }
}
