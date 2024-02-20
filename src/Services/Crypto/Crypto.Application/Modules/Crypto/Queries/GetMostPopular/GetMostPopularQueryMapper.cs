using AutoMapper;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryMapper : Profile
    {
        public GetMostPopularQueryMapper()
        {
            // CreateMap<CryptoMostPopularQuery, GetMostPopularResponse>()
            //     .ForMember(dst => dst.Counter, opt => opt.MapFrom(src => src.Counter))
            //     .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol));
        }
    }
}
