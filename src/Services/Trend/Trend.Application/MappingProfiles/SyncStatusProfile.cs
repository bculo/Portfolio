using AutoMapper;
using Dtos.Common.v1.Trend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles
{
    public class SyncStatusProfile : Profile
    {
        public SyncStatusProfile()
        {
            CreateMap<SyncStatus, SyncStatusDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Finished, opt => opt.MapFrom(src => src.Finished))
                .ForMember(dst => dst.Started, opt => opt.MapFrom(src => src.Started))
                .ForMember(dst => dst.TotalRequests, opt => opt.MapFrom(src => src.TotalRequests))
                .ForMember(dst => dst.SucceddedRequests, opt => opt.MapFrom(src => src.SucceddedRequests));

            CreateMap<SyncStatusWord, SyncStatusWordDto>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dst => dst.ContextTypeId, opt => opt.MapFrom(src => (int)src.Type));
        }
    }
}
