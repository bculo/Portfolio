using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Application.MappingProfiles.Actions;
using Trend.Application.Models.Dtos.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Queries.Requests.Common;
using Trend.Domain.Queries.Responses.Common;

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
                .ForMember(dst => dst.TypeName, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => (int)src.Type));

            CreateMap<FetchLatestNewsPageDto, PageRequest>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page));

            CreateMap<FetchArticleTypePageDto, PageRequest<ContextType>>()
                .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
                .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dst => dst.Search, opt => opt.MapFrom(src => (ContextType)src.Type));

            CreateMap<PageResponse<Article>, PageResponseDto<ArticleDto>>()
                .ForMember(dst => dst.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dst => dst.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageResponse<Article>, PageResponseDto<ArticleTypeDto>>()
                .ForMember(dst => dst.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dst => dst.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
