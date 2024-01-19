using AutoMapper;
using Trend.Application.Interfaces.Models;
using Trend.gRPC.Protos.v1;

namespace Trend.gRPC.GrpcProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<ArticleResDto, ArticleTypeItem>().ReverseMap();
            CreateMap<ArticleResDto, ArticleItem>().ReverseMap();

            CreateMap<ArticleResDto, ArticleItem>().ReverseMap();
            CreateMap<ArticleResDto, ArticleTypeItem>().ReverseMap();

            CreateMap<FetchArticleTypePageRequest, FilterArticlesReqDto>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Page.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page.PageNum))
                .ForMember(dst => dst.Context, opt => opt.MapFrom(src => (int)src.Type));
        }
    }
}
