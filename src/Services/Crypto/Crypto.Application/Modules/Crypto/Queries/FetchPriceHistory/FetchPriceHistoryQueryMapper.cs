using AutoMapper;
using Crypto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryMapper : Profile
    {
        public FetchPriceHistoryQueryMapper()
        {
            CreateMap<CryptoPrice, PriceHistoryDto>()
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
