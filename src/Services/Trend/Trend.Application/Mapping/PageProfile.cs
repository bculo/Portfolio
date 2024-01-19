using AutoMapper;
using Dtos.Common;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Mapping
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap(typeof(PageResQuery<>), typeof(PageResponseDto<>));
        }
    }
}
