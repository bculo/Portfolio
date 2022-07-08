﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos
{
    public class FetchAllCryptosQueryMapper : Profile
    {
        public FetchAllCryptosQueryMapper()
        {
            CreateMap<Core.Entities.Crypto, FetchAllCryptosResponseDto>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
                .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.WebSite))
                .ForMember(dst => dst.Logo, opt => opt.MapFrom(src => src.Logo));
        }
    }
}
