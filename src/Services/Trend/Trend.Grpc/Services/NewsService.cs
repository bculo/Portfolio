
using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Trend.Application.Interfaces;
using Trend.Application.Utils.Validation;
using Trend.Application.Validators.News;
using Trend.Domain.Enums;
using Trend.Grpc.Protos;

namespace Trend.Grpc.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NewsService : News.NewsBase
    {
        private readonly IArticleService _service;
        private readonly ILogger<NewsService> _logger;
        private readonly IMapper _mapper;

        public NewsService(IArticleService service, IMapper mapper, ILogger<NewsService> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task GetAllNews(Empty request, IServerStreamWriter<ArticleTypeItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetAllNews method called in NewsService");

            await foreach (var item in _service.GetAllEnumerable())
            {
                var responseItem = _mapper.Map<ArticleTypeItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }

        public override async Task GetLatestNews(Empty request, IServerStreamWriter<ArticleItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetLatestNews method called in NewsService");

            await foreach (var item in _service.GetLatestNewsEnumerable())
            {
                var responseItem = _mapper.Map<ArticleItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }

        public override async Task GetLatestNewsForType(ArticleTypeRequest request, IServerStreamWriter<ArticleItem> responseStream, ServerCallContext context)
        {
            _logger.LogTrace("GetLatestNewsForType method called in NewsService");

            var requestType = (ContextType)(int)request.Type;

            await foreach (var item in _service.GetLatestNewsEnumerable(requestType))
            {
                var responseItem = _mapper.Map<ArticleItem>(item);
                await responseStream.WriteAsync(responseItem);
            }
        }

        public override async Task<ArticleItemExtendedPageResponse> GetLatestNewsPage(FetchLatestNewsPageRequest request, ServerCallContext context)
        {
            _logger.LogTrace("GetLatestNewsPage method called in NewsService");

            var serviceRequest = _mapper.Map<FetchLatestNewsPageDto>(request);

            ValidationUtils.Validate(serviceRequest, new FetchLatestNewsPageDtoValidator());

            var serviceResponse = await _service.GetLatestNewsPage(serviceRequest);

            var response = new ArticleItemExtendedPageResponse
            { 
                Count = serviceResponse.Count,
            };

            response.Items.AddRange(_mapper.Map<List<ArticleTypeItem>>(serviceResponse.Items));

            return response;
        }
    }
}
