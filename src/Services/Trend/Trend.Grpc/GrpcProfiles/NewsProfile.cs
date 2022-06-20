using AutoMapper;
using Dtos.Common.v1.Trend;
using Trend.Grpc.Protos;

namespace Trend.Grpc.GrpcProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<ArticleTypeDto, ArticleTypeItem>();
            CreateMap<ArticleDto, ArticleItem>();
        }
    }
}
