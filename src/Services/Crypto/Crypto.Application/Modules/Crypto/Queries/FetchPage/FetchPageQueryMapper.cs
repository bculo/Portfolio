using AutoMapper;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryMapper : Profile
    {
        public FetchPageQueryMapper()
        {
            CreateMap<CryptoLastPriceReadModel, FetchPageResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CryptoId))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
                .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.LastPrice));

            CreateMap<FetchPageQuery, CryptoPricePageQuery>()
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol));
        }
    }
}
