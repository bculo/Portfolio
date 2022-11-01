using AutoMapper;
using Crypto.Core.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchGroup
{
    public class FetchGroupQueryMapper : Profile
    {
        public FetchGroupQueryMapper()
        {
            CreateMap<CryptoResponseQuery, FetchGroupResponseDto>()
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
