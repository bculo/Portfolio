using AutoMapper;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Modules.Crypto.Queries.SearchBySymbol
{
    public class SearchBySymbolQueryMapper : Profile
    {
        public SearchBySymbolQueryMapper()
        {
            CreateMap<CryptoLastPrice, SearchBySymbolResponse>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.TimeBucket))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
                .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.AvgPrice));
        }
    }
}
