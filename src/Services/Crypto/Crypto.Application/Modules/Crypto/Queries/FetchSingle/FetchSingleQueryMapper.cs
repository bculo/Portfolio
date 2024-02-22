using AutoMapper;
using Events.Common.Crypto;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryMapper : Profile
    {
        public FetchSingleQueryMapper()
        {


            CreateMap<PriceUpdated, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
