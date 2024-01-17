using AutoMapper;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Interfaces.Models.Services.Google;
using Trend.Domain.Entities;

namespace Trend.Application.Mapping.Sync
{
    public class SyncStatusProfile : Profile
    {
        public SyncStatusProfile()
        {
            CreateMap<SyncStatus, SyncStatusResDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Finished, opt => opt.MapFrom(src => src.Finished))
                .ForMember(dst => dst.Started, opt => opt.MapFrom(src => src.Started))
                .ForMember(dst => dst.TotalRequests, opt => opt.MapFrom(src => src.TotalRequests))
                .ForMember(dst => dst.SucceddedRequests, opt => opt.MapFrom(src => src.SucceddedRequests))
                .ForMember(dst => dst.SearchWords, opt => opt.MapFrom(src => src.UsedSyncWords));

            CreateMap<SyncStatusWord, SyncStatusWordResDto>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.WordId))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dst => dst.ContextTypeId, opt => opt.MapFrom(src => (int)src.Type));
        }
    }
}
