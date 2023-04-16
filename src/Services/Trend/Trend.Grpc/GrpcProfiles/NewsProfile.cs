using AutoMapper;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Trend.Grpc.Protos.v1;

namespace Trend.Grpc.GrpcProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<ArticleTypeDto, ArticleTypeItem>().ReverseMap();
            CreateMap<ArticleTypeDto, ArticleItem>().ReverseMap();

            CreateMap<ArticleDto, ArticleItem>().ReverseMap();
            CreateMap<ArticleDto, ArticleTypeItem>().ReverseMap();

            CreateMap<FetchLatestNewsPageRequest, FetchLatestNewsPageDto>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Page.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page.PageNum));

            CreateMap<FetchArticleTypePageRequest, FetchArticleTypePageDto>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Page.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page.PageNum))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => (int)src.Type));
        }
    }
}
