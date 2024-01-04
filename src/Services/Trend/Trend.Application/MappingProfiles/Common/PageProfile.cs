using AutoMapper;
using Dtos.Common.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
