using AutoMapper;
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

namespace Trend.Application.MappingProfiles
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
                .ForMember(dst => dst.Snippet, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.Link))
                .ReverseMap();

            CreateMap<GoogleSearchEngineItemDto, Article>()
                .ForMember(dst => dst.PageSource, opt => opt.MapFrom(src => src.Page))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Snippet))
                .ForMember(dst => dst.ArticleUrl, opt => opt.MapFrom(src => src.Link))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .AfterMap<DefineArticleCreatedDateTimeAction>();
        }
    }
}
