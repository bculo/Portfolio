using AutoMapper;
using Crypto.Core.Entities;
using Events.Common.Crypto;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryMapper : Profile
    {
        public FetchSingleQueryMapper()
        {
            CreateMap<PriceUpdated, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
            
            CreateMap<CryptoPrice, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Crypto.Name))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CryptoId))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Crypto.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
