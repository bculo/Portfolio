using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Application.Services.Models;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Services
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private const int MaxArticlesPerWord = 4;
        
        private readonly ILogger<GoogleSearchEngine> _logger;
        private readonly IGoogleSearchClient _searchService;
        private readonly IDateTimeProvider _time;
        private readonly IMapper _mapper;
        private readonly SyncStatus _syncStatus;
        private readonly List<Article> _articles;
        
        public string EngineName => nameof(GoogleSearchEngine);
        public Dictionary<ContextType, List<SearchEngineReq>>? SearchWordsByCategory { get; set; }
        
        public GoogleSearchEngine(
            ILogger<GoogleSearchEngine> logger,
            IGoogleSearchClient searchService,
            IDateTimeProvider time,
            IMapper mapper)
        {
            _logger = logger;
            _searchService = searchService;
            _mapper = mapper;
            _time = time;
            
            _articles = new List<Article>();
            _syncStatus = new SyncStatus
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Created = _time.Now,
                Started = _time.Now,
            };
        }
        
        public async Task<SearchEngineRes> Sync(
            Dictionary<ContextType, List<SearchEngineReq>> searchWordsByCategory, 
            CancellationToken token = default)
        {
            using var span = Telemetry.Trend.StartActivity(Telemetry.SyncEngine);
            span?.SetTag(Telemetry.SyncEngineNameTag, EngineName);
            SearchWordsByCategory = searchWordsByCategory ?? new();
            
            if(!SearchWordsByCategory.Any())
            {
                _logger.LogInformation("ArticleTypes to fetch are not defined");
                MarkSyncStatusAsDone();
                return new SearchEngineRes(_syncStatus, new List<Article>());
            }
            
            foreach(var (contextType, searchWords) in SearchWordsByCategory)
            {
                await ScrapeDataUsingGoogleClient(contextType, searchWords, token);
            }

            MarkSyncStatusAsDone();
            return new SearchEngineRes (_syncStatus, _articles);
        }

        private void MarkSyncStatusAsDone()
        {
            AttachSyncWordToSyncStatus();
            MarkSyncStatusAsFinished();
            AttachSyncStatusIdentifierToArticles();
        }

        private void MarkSyncStatusAsFinished()
        {
            _syncStatus.Finished = _time.Now;
        }

        private void AttachSyncWordToSyncStatus()
        {
            foreach (var dict in SearchWordsByCategory!)
            {
                var words = dict.Value.Select(item => new SyncStatusWord
                {
                    Type = dict.Key,
                    WordId = item.SearchWordId
                });

                _syncStatus.UsedSyncWords.AddRange(words);
            }
        }

        private void AttachSyncStatusIdentifierToArticles()
        {
            _articles.ForEach(item => item.SyncStatusId = _syncStatus.Id);
        }
        
        private async Task ScrapeDataUsingGoogleClient(ContextType type, 
            List<SearchEngineReq> keyWords,
            CancellationToken token = default)
        {
            var responses = await FetchDataViaGoogleClient(keyWords, token);
            StoreClientResponseInMemory(responses);
        }

        private async Task<List<GoogleClientResponse>> FetchDataViaGoogleClient(List<SearchEngineReq> keyWords, 
            CancellationToken token = default) 
        {
            var requestList = keyWords
                .Select(word => new GoogleClientRequest
                {
                    RequestTask = _searchService.Search(word.SearchWord, token),
                    SearchWordId =  word.SearchWordId
                })
                .ToList();

            await Task.WhenAll(requestList.Select(i => i.RequestTask));

            return requestList.Select(item => 
                new GoogleClientResponse
                {
                    ClientResponse = item.RequestTask.Result,
                    SearchWordId = item.SearchWordId,
                })
                .ToList();
        }

        private void StoreClientResponseInMemory(IEnumerable<GoogleClientResponse> searchWordsResponses)
        {
            foreach(var searchWordResponse in searchWordsResponses)
            {
                _syncStatus.TotalRequests++;
                if (!searchWordResponse.Success) continue;

                var searchWordArticles = 
                    searchWordResponse.ClientResponse?.Items ?? new List<GoogleSearchEngineItemDto>();
                var newArticles = _mapper.Map<List<Article>>(searchWordArticles)
                    .Take(MaxArticlesPerWord)
                    .Select(i =>
                    {
                        i.SearchWordId = searchWordResponse.SearchWordId;
                        i.IsActive = true;
                        return i;
                    })
                    .ToList();
                
                _syncStatus.SucceddedRequests++;
                _articles.AddRange(newArticles);
            }
        }
    }
}
