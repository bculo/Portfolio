using AutoMapper;
using Dtos.Common;
using Trend.Application.Interfaces.Models.Repositories;

namespace Trend.Application.MappingProfiles.Common
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap(typeof(PageResQuery<>), typeof(PageResponseDto<>));
        }
    }
}
