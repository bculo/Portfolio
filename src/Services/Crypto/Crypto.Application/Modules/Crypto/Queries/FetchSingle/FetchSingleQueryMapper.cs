﻿using AutoMapper;
using Crypto.Core.ReadModels;
using Events.Common.Crypto;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryMapper : Profile
    {
        public FetchSingleQueryMapper()
        {
            CreateMap<CryptoPriceUpdated, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
            
            CreateMap<CryptoLastPriceReadModel, FetchSingleResponseDto>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CryptoId))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.LastPrice));
        }
    }
}
