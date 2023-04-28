using AutoMapper;
using Dtos.Common.v1.Trend.Article;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Models.Dtos.Google;
using Trend.Application.Models.Service.Intern.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class GoogleSyncService : IGoogleSyncService
    {
        private readonly ILogger<GoogleSyncService> _logger;
        private readonly IGoogleSearchClient _searchService;
        private readonly IDateTimeProvider _time;
        private readonly IMapper _mapper;
        private readonly IRepository<SyncStatus> _syncRepo;
        private readonly IArticleRepository _articleRepo;
        private readonly ITransaction _session;

        public GoogleSyncResult Result { get; private set; }
        public SyncStatus SyncStatus { get; private set; }
        public List<Article> Entities { get; private set; }

        public GoogleSyncService(
            ILogger<GoogleSyncService> logger,
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
            _syncRepo = syncRepo;
            _articleRepo = articleRepo;
            _session = session;

            Result = new GoogleSyncResult();
            Entities = new List<Article>();
        }

        public async Task<GoogleSyncResult> Sync(Dictionary<ContextType, List<string>> articleTypesToSync)
        {
            _logger.LogTrace("Sync method called in GoogleSyncService");

            if(articleTypesToSync.Count == 0)
            {
                _logger.LogInformation("ArticleTypes to fetch are not defiend");
                return Result;
            }

            SyncStatus = CreateSyncInstance();

            foreach(var articleSync in articleTypesToSync)
            {
                await ExecuteSync(articleSync.Key, articleSync.Value);
            }

            await PersistData(articleTypesToSync);

            return Result;
        }

        private async Task PersistData(Dictionary<ContextType, List<string>> articleTypesToSync)
        {
            //prepare sync instance
            AttachSyncWordToSyncStatus(articleTypesToSync);
            MarkSyncStatusAsFinished();
            Result.SetSyncInstance(SyncStatus);

            if (Result.TotalSuccess == 0)
            {
                await PersistSyncStatus();
                return;
            }

            //fetch current active instances (fetched instances that will be deactivated)
            var oldActiveArticles = await _articleRepo.GetActiveArticles();
            var oldActiveIds = oldActiveArticles.Select(i => i.Id).ToList();

            //prepare article instances
            AttachSyncStatusIdentifierToArticles();

            await _session.StartTransaction();

            //save sync status and new articles
            await PersistSyncStatus();
            await PersistNewArticles();

            //deactivate old articles
            await _articleRepo.DeactivateArticles(oldActiveIds);

            await _session.CommitTransaction();
        }

        private void MarkSyncStatusAsFinished()
        {
            SyncStatus.Finished = _time.Now;
            SyncStatus.SucceddedRequests = Result.TotalSuccess;
            SyncStatus.TotalRequests = Result.Total;
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
            Entities.ForEach(item => item.SyncStatusId = SyncStatus.Id);
        }

        private async Task PersistSyncStatus()
        {
            await _syncRepo.Add(SyncStatus);
        }

        private async Task PersistNewArticles()
        {
            if (Entities.Count == 0)
            {
                return;
            }

            await _articleRepo.Add(Entities);
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
                _logger.LogInformation("Article type {0} does not contain key words", type.ToString());
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

        private async Task CreateResponse(IReadOnlyList<GoogleResponseStatus> responses, ContextType type)
        {
            foreach(var response in responses)
            {
                SyncStatus.TotalRequests++;

                if (response.Succedded)
                {
                    SyncStatus.SucceddedRequests++;
                    Entities.AddRange(_mapper.Map<List<Article>>(response.Result.Items));
                }

                Result.AddResponse(type, response.SearchWord, response.Succedded, response.Succedded ? response.Result : null);
            }
        }

        private class GoogleResponseStatus
        {
            public GoogleSearchEngineResponseDto Result { get; set; }
            public string SearchWord { get; set; }
            public bool Succedded => Result != null;
        }
    }
}
