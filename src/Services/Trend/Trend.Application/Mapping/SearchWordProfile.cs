using AutoMapper;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Mapping
{
    public class SearchWordProfile : Profile
    {
        public SearchWordProfile()
        {
            CreateMap<AddWordReqDto, Domain.Entities.SearchWord>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.SearchWord))
                .ForMember(dst => dst.Engine, opt => opt.MapFrom(src => src.SearchEngine))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.ContextType));

            CreateMap<Domain.Entities.SearchWord, SearchWordResDto>()
                .ForMember(dst => dst.SearchEngineName, opt => opt.MapFrom(src => src.Engine.ToString()))
                .ForMember(dst => dst.SearchWord, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.SearchEngineId, opt => opt.MapFrom(src => (int)src.Engine))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dst => dst.ContextTypeId, opt => opt.MapFrom(src => src.Type))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<FilterSearchWordsReqDto, SearchWordFilterReqQuery>();
            CreateMap<SearchWordSyncDetailResQuery, SearchWordSyncStatisticResDto>();
        }
    }
}
