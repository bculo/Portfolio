using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

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
            IMapper mapper,
            IRepository<SyncStatus> syncRepo,
            IArticleRepository articleRepo,
            ITransaction session)
        {
            _logger = logger;
            _searchService = searchService;
            _mapper = mapper;
            _time = time;

            Articles = new List<Article>();
        }

        public async Task<(SyncStatus syncIteration, List<Article> syncArticles)> Sync(Dictionary<ContextType, List<string>> articleTypesToSync, CancellationToken token)
        {
            if(articleTypesToSync.Count == 0)   
            {
                _logger.LogInformation("ArticleTypes to fetch are not defined");
                await StatusSyncFinalTouch(articleTypesToSync, token);
                return (SyncStatus, new List<Article>());
            }

            SyncStatus = CreateSyncInstance();

            foreach(var articleSync in articleTypesToSync)
            {
                await ExecuteSync(articleSync.Key, articleSync.Value);
            }

            await StatusSyncFinalTouch(articleTypesToSync, token);
            
            return (SyncStatus, Articles);
        }

        private Task StatusSyncFinalTouch(Dictionary<ContextType, List<string>> articleTypesToSync, CancellationToken token)
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

        private void AttachSyncWordToSyncStatus(Dictionary<ContextType, List<string>> articleTypesToSync)
        {
            foreach (var dict in articleTypesToSync)
            {
                var words = dict.Value.Select(item => new SyncStatusWord
                {
                    Type = dict.Key,
                    Word = item
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

        private async Task ExecuteSync(ContextType type, IReadOnlyList<string> keyWords)
        {
            if(keyWords is null || keyWords.Count == 0)
            {
                _logger.LogInformation("Article type {0} doesn't contain any key words", type.ToString());
            }

            var responses = await FetchData(keyWords!);
            await CreateResponse(responses, type);
        }

        private async Task<IReadOnlyList<GoogleResponseStatus>> FetchData(IReadOnlyList<string> keyWords) 
        {
            var requestList = keyWords!.Where(searchWord => !string.IsNullOrWhiteSpace(searchWord))
                                    .Select(searchWord => Tuple.Create(_searchService.Search(searchWord), searchWord))
                                    .ToList();

            await Task.WhenAll(requestList.Select(i => i.Item1));

            return requestList.Select(item => new GoogleResponseStatus
            {
                Result = item.Item1.Result, //search response DTO
                SearchWord = item.Item2, //search word
            })
            .ToList()
            .AsReadOnly();
        }

        private Task CreateResponse(IEnumerable<GoogleResponseStatus> responses, ContextType type)
        {
            foreach(var response in responses)
            {
                SyncStatus.TotalRequests++;

                if (!response.Succedded) continue;
                
                SyncStatus.SucceddedRequests++;
                var articles = _mapper.Map<List<Article>>(response.Result.Items);
                foreach (var article in articles)
                {
                    article.SearchWord = response.SearchWord;
                }
                
                Articles.AddRange(articles);
            }

            return Task.CompletedTask;
        }

        private class GoogleResponseStatus
        {
            public GoogleSearchEngineResponseDto Result { get; set; }
            public string SearchWord { get; set; }
            public bool Succedded => Result != null;
        }
    }
}
