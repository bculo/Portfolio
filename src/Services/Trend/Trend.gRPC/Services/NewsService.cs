﻿using AutoMapper;
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
    public class NewsService : News.NewsBase
    {
        private readonly IArticleServiceEnumerable _service;
        private readonly ILogger<NewsService> _logger;
        private readonly IMapper _mapper;

        public NewsService(
            IArticleServiceEnumerable service, 
            IMapper mapper,
            ILogger<NewsService> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
        
        public override async Task GetAllNews(Empty request, IServerStreamWriter<ArticleTypeItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetAllNews method called in NewsService");
            
            await foreach (var item in _service.GetAllEnumerable(default))
            {
                var responseItem = _mapper.Map<ArticleTypeItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
        
        public override async Task GetLatestNews(Empty request, IServerStreamWriter<ArticleItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetLatestNews method called in NewsService");
            
            await foreach (var item in _service.GetLatestEnumerable(default))
            {
                var responseItem = _mapper.Map<ArticleItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
        
        public override async Task GetLatestNewsForType(ArticleTypeRequest request, IServerStreamWriter<ArticleItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetLatestNewsForType method called in NewsService");

            var requestType = (ContextType)(int)request.Type;

            await foreach (var item in _service.GetLatestByContextEnumerable(requestType, default))
            {
                var responseItem = _mapper.Map<ArticleItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }
    }
}
