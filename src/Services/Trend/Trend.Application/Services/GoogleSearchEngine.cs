using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Services;
using Trend.Application.Interfaces.Models.Services.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Services
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private readonly ILogger<GoogleSearchEngine> _logger;
        private readonly IGoogleSearchClient _searchService;
        private readonly IDateTimeProvider _time;
        private readonly IMapper _mapper;

        private SyncStatus SyncStatus { get; set; }
        private List<Article> Articles { get; set; }

        public string EngineName => nameof(GoogleSearchEngine);
        
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

            Articles = new List<Article>();
        }

        public async Task<SearchEngineResult> Sync(
            Dictionary<ContextType, List<SearchEngineWord>> articleTypesToSync, 
            CancellationToken token)
        {
            SyncStatus = CreateSyncInstance();
            
            if(articleTypesToSync.Count == 0)   
            {
                _logger.LogInformation("ArticleTypes to fetch are not defined");
                await MarkSyncStatusAsDone(articleTypesToSync, token);
                return new SearchEngineResult(SyncStatus, new List<Article>());
            }
            
            foreach(var (contextType, searchWords) in articleTypesToSync)
            {
                if(!searchWords.Any())
                {
                    continue;
                }
                
                await ScrapeDataFromGoogleEngine(contextType, searchWords);
            }

            await MarkSyncStatusAsDone(articleTypesToSync, token);
            
            return new SearchEngineResult (SyncStatus, Articles);
        }

        private Task MarkSyncStatusAsDone(Dictionary<ContextType, List<SearchEngineWord>> articleTypesToSync, CancellationToken token)
        {
            AttachSyncWordToSyncStatus(articleTypesToSync);
            MarkSyncStatusAsFinished();

            if (SyncStatus.SucceddedRequests > 0)
            {
                AttachSyncStatusIdentifierToArticles();
            }

            return Task.CompletedTask;
        }

        private void MarkSyncStatusAsFinished()
        {
            SyncStatus.Finished = _time.Now;
        }

        private void AttachSyncWordToSyncStatus(Dictionary<ContextType, List<SearchEngineWord>> articleTypesToSync)
        {
            foreach (var dict in articleTypesToSync)
            {
                var words = dict.Value.Select(item => new SyncStatusWord
                {
                    Type = dict.Key,
                    Word = item.SearchWord
                });

                SyncStatus.UsedSyncWords.AddRange(words);
            }
        }

        private void AttachSyncStatusIdentifierToArticles()
        {
            Articles.ForEach(item => item.SyncStatusId = SyncStatus.Id);
        }
        
        private SyncStatus CreateSyncInstance()
        {
            return new SyncStatus
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Created = _time.Now,
                Started = _time.Now,
            };
        }

        private async Task ScrapeDataFromGoogleEngine(ContextType type, List<SearchEngineWord> keyWords)
        {
            var responses = await FetchData(keyWords!);
            await CreateResponse(responses);
        }

        private async Task<IReadOnlyList<GoogleResponseStatus>> FetchData(List<SearchEngineWord> keyWords) 
        {
            var requestList = keyWords
                .Select(word => Tuple.Create(_searchService.Search(word.SearchWord), word.SearchWordId))
                .ToList();

            await Task.WhenAll(requestList.Select(i => i.Item1));

            return requestList.Select(item => new GoogleResponseStatus
                {
                    Result = item.Item1.Result, //Google client response DTO
                    SearchWordId = item.Item2, //Search word ID
                })
                .ToList();
        }

        private Task CreateResponse(IEnumerable<GoogleResponseStatus> responses)
        {
            foreach(var response in responses)
            {
                SyncStatus.TotalRequests++;
                if (!response.Success) continue;
                
                var newArticles = _mapper.Map<List<Article>>(response.Result.Items)
                    .Take(4)
                    .Select(i =>
                    {
                        i.SearchWordId = response.SearchWordId;
                        return i;
                    })
                    .ToList();
                
                Articles.AddRange(newArticles);
            }

            return Task.CompletedTask;
        }
    }
    
    public class GoogleResponseStatus
    {
        public GoogleSearchEngineResponseDto Result { get; init; }
        public string SearchWordId { get; init; }
        public bool Success => Result != null;
    }
}
