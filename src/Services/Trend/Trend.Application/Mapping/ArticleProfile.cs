using AutoMapper;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;

namespace Trend.Application.Mapping
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<GoogleSearchEngineItemDto, ArticleResDto>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.Link))
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.DisplayLink))
                .ReverseMap();

            CreateMap<GoogleSearchEngineItemDto, Article>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.DisplayLink))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.ArticleUrl, opt => opt.MapFrom(src => src.Link))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<Article, ArticleResDto>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.PageSource))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.ArticleUrl))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ArticleDetailResQuery, ArticleResDto>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.PageSource))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.ArticleUrl))
                .ForMember(dst => dst.TypeName, opt => opt.MapFrom(src => src.ContextType.ToString()))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SearchWord, opt => opt.MapFrom(src => src.SearchWord))
                .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => (int)src.ContextType))
                .ForMember(dst => dst.SearchWordId, opt => opt.MapFrom(src => src.SearchWordId))
                .ForMember(dst => dst.SearchWordImage, opt => opt.MapFrom(src => src.SearchWordImage));
        }
    }
}
