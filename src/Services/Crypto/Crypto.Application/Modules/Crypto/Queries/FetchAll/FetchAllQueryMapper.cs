using AutoMapper;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAll
{
    public class FetchAllQueryMapper : Profile
    {
        public FetchAllQueryMapper()
        {
            CreateMap<CryptoLastPrice, FetchAllResponseDto>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.TimeBucket))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
                .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.Website));
        }
    }
}
