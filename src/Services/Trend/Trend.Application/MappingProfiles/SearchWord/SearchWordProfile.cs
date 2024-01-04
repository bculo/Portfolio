using AutoMapper;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.MappingProfiles.Actions;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.MappingProfiles.SearchWord
{
    public class SearchWordProfile : Profile
    {
        public SearchWordProfile()
        {
            CreateMap<SearchWordCreateReqDto, Domain.Entities.SearchWord>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.SearchWord))
                .ForMember(dst => dst.Engine, opt => opt.MapFrom(src => (SearchEngine)src.SearchEngine))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => (SearchEngine)src.ContextType))
                .AfterMap<DefineSyncSettingCreatedDateTimeAction>();

            CreateMap<Domain.Entities.SearchWord, SearchWordResDto>()
                .ForMember(dst => dst.SearchEngineName, opt => opt.MapFrom(src => src.Engine.ToString()))
                .ForMember(dst => dst.SearchWord, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.SearchEngineId, opt => opt.MapFrom(src => (int)src.Engine))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
