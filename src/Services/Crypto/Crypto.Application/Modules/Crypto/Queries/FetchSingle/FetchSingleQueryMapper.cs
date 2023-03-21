using AutoMapper;
using Crypto.Core.Queries.Response;
using Events.Common.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryMapper : Profile
    {
        public FetchSingleQueryMapper()
        {
            CreateMap<CryptoResponseQuery, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<CryptoPriceUpdated, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
