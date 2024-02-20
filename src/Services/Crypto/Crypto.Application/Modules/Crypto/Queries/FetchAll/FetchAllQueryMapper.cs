using AutoMapper;
using Crypto.Core.Entities;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAll
{
    public class FetchAllQueryMapper : Profile
    {
        public FetchAllQueryMapper()
        {
            CreateMap<CryptoLastPrice, FetchAllResponseDto>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
                .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dst => dst.Logo, opt => opt.MapFrom(src => src.Logo));
        }
    }
}
