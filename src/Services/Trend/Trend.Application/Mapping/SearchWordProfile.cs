using AutoMapper;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;

namespace Trend.Application.Mapping
{
    public class SearchWordProfile : Profile
    {
        public SearchWordProfile()
        {
            CreateMap<AddWordReqDto, SearchWord>()
                .ForMember(dst => dst.Word, opt => opt.MapFrom(src => src.SearchWord))
                .ForMember(dst => dst.Engine, opt => opt.MapFrom(src => src.SearchEngine))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.ContextType));

            CreateMap<SearchWord, SearchWordResDto>()
                .ForMember(dst => dst.SearchEngineName, opt => opt.MapFrom(src => src.Engine.DisplayValue))
                .ForMember(dst => dst.SearchWord, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.SearchEngineId, opt => opt.MapFrom(src => src.Engine.Value))
                .ForMember(dst => dst.ContextTypeName, opt => opt.MapFrom(src => src.Type.DisplayValue))
                .ForMember(dst => dst.ContextTypeId, opt => opt.MapFrom(src => src.Type.Value))
                .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<FilterSearchWordsReqDto, SearchWordFilterReqQuery>();
            CreateMap<SearchWordSyncDetailResQuery, SearchWordSyncStatisticResDto>();
        }
    }
}
