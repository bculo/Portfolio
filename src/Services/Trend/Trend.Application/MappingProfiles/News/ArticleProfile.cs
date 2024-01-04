using AutoMapper;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Application.Interfaces.Models.Services.Google;
using Trend.Application.MappingProfiles.Actions;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.MappingProfiles.News
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<GoogleSearchEngineResponseDto, ArticleGroupDto>()
                .ForMember(dst => dst.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<GoogleSearchEngineItemDto, ArticleDto>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.Link))
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.DisplayLink))
                .ReverseMap();

            CreateMap<GoogleSearchEngineItemDto, Article>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.DisplayLink))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.ArticleUrl, opt => opt.MapFrom(src => src.Link))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .AfterMap<DefineArticleCreatedDateTimeAction>();

            CreateMap<Article, ArticleDto>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.PageSource))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.ArticleUrl));

            CreateMap<Article, ArticleTypeDto>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.PageSource))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.ArticleUrl))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ArticleDetailResQuery, ArticleTypeDto>()
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

            CreateMap<FetchLatestNewsPageDto, PageReqQuery>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page));

            CreateMap<FetchArticleTypePageDto, PageReqQuery<ContextType>>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dst => dst.Search, opt => opt.MapFrom(src => (ContextType)src.Type));

        }
    }
}
