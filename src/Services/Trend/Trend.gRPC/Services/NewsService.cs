using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;
using Trend.Application.Interfaces;
using Trend.Application.Utils;
using Trend.Application.Validators.News;
using Trend.Domain.Enums;
using Trend.gRPC.Protos;
using Trend.gRPC.Protos.v1;

namespace Trend.gRPC.Services
{
    [Authorize]
    public class NewsService(
        IArticleServiceEnumerable service,
        IMapper mapper,
        ILogger<NewsService> logger)
        : News.NewsBase
    {
        private readonly ILogger<NewsService> _logger = logger;

        public override async Task GetAllNews(Empty request, IServerStreamWriter<ArticleTypeItemReply> responseStream, ServerCallContext context)
        {
            await foreach (var item in service.GetAllEnumerable(default))
            {
                var responseItem = mapper.Map<ArticleTypeItemReply>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
        
        public override async Task GetLatestNews(Empty request, IServerStreamWriter<ArticleItemReply> responseStream, ServerCallContext context)
        {
            await foreach (var item in service.GetLatestEnumerable(default))
            {
                var responseItem = mapper.Map<ArticleItemReply>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
        
        public override async Task GetLatestNewsForType(ArticleTypeRequest request, IServerStreamWriter<ArticleItemReply> responseStream, ServerCallContext context)
        {
            var requestType = (ContextType)(int)request.Type;
            await foreach (var item in service.GetLatestByContextEnumerable(requestType, default))
            {
                var responseItem = mapper.Map<ArticleItemReply>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
    }
}
