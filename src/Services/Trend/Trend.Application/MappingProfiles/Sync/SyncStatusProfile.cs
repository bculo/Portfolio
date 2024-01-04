using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Models.Services.Google;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Sync
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
                .ForMember(dst => dst.SucceddedRequests, opt => opt.MapFrom(src => src.SucceddedRequests))
                .ForMember(dst => dst.SearchWords, opt => opt.MapFrom(src => src.UsedSyncWords));

            CreateMap<SyncStatusWord, SyncStatusWordDto>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dst => dst.ContextTypeId, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<GoogleSyncResult, SyncResultDto>()
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.SyncStatus))
                .ForMember(dst => dst.SearchResult, opt => opt.MapFrom(src => src.GetInstances()));
        }
    }
}
