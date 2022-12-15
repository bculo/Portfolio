using AutoMapper;
using Crypto.Application.Common.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Queries.FetchAll
{
    public class FetchAllPortfoliosMapper : Profile
    {
        public FetchAllPortfoliosMapper()
        {
            CreateMap<Core.Entities.Portfolio, FetchAllPortfoliosDto>()
                .ForMember(i => i.Status, opt => opt.MapFrom(i => i.Status.ToString()))
                .ForMember(i => i.StatusId, opt => opt.MapFrom(i => (int)i.Status))
                .ForMember(i => i.Name, opt => opt.MapFrom(i => i.Name))
                .ForMember(i => i.Id, opt => opt.MapFrom<IdentifierEncodeResolver>());
        }
    }
}
