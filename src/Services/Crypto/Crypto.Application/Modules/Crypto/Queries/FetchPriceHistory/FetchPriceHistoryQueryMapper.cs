using AutoMapper;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryMapper : Profile
    {
        public FetchPriceHistoryQueryMapper()
        {
            CreateMap<CryptoTimeFrameReadModel, PriceHistoryDto>()
                .ForMember(dst => dst.LastPrice, opt => opt.MapFrom(src => Math.Round(src.LastPrice, 2)))
                .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => Math.Round(src.MaxPrice, 2)))
                .ForMember(dst => dst.MinPrice, opt => opt.MapFrom(src => Math.Round(src.MinPrice, 2)))
                .ForMember(dst => dst.AvgPrice, opt => opt.MapFrom(src => Math.Round(src.AvgPrice, 2)));
        }
    }
}
