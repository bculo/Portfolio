using AutoMapper;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryMapper : Profile
    {
        public GetMostPopularQueryMapper()
        {
            CreateMap<MostPopularReadModel, GetMostPopularResponse>();
        }
    }
}
