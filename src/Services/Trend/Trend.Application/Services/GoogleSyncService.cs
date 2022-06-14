using AutoMapper;
using Dtos.Common.v1.Trend;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Models.Dtos.Google;
using Trend.Application.Models.Service.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class GoogleSyncService : IGoogleSyncService
    {
        private readonly ILogger<GoogleSyncService> _logger;
        private readonly IGoogleSearchClient _searchService;
        private readonly IDateTime _time;
        private readonly IMapper _mapper;
        private readonly IRepository<SyncStatus> _syncRepo;
        private readonly IRepository<Article> _articleRepo;

        public GoogleSyncResult Result { get; set; }
        public SyncStatus SyncStatus { get; set; }
        public List<Article> Entities { get; set; }

        public GoogleSyncService(
            ILogger<GoogleSyncService> logger,
            IGoogleSearchClient searchService,
            IDateTime time,
            IMapper mapper,
            IRepository<SyncStatus> syncRepo,
            IRepository<Article> articleRepo)
        {
            _logger = logger;
            _searchService = searchService;
            _mapper = mapper;
            _time = time;
            _syncRepo = syncRepo;
            _articleRepo = articleRepo;

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

            _logger.LogTrace("Strating with sync");
            
            foreach(var articleSync in articleTypesToSync)
            {
                await ExecuteSync(articleSync.Key, articleSync.Value);
            }

            SyncStatus.Finished = _time.DateTime;
            SyncStatus.SucceddedRequests = Result.TotalSuccess;
            SyncStatus.TotalRequests = Result.Total;

            await PersistData();

            return Result;
        }

        private async Task PersistData()
        {
            await PersistSyncStatus();
            await PersistNewArticles();
        }

        private async Task PersistSyncStatus()
        {
            _logger.LogTrace("PersistSyncStatus method called in GoogleSyncService");

            await _syncRepo.Add(SyncStatus);
        }

        private async Task PersistNewArticles()
        {
            if (Entities.Count == 0)
            {
                return;
            }

            _logger.LogTrace("Saving new article entities");

            await _articleRepo.Add(Entities);
        }

        private SyncStatus CreateSyncInstance()
        {
            return new SyncStatus
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Created = _time.DateTime,
                Started = _time.DateTime,
            };
        }

        private async Task ExecuteSync(ContextType type, IReadOnlyList<string> keyWords)
        {
            _logger.LogTrace("Sync for article type {0}", type.ToString());

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

            _logger.LogTrace("HTTP request/requests sent");

            await Task.WhenAll(requestList.Select(i => i.Item1));

            _logger.LogTrace("All HTTP request finished");

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
                ArticleGroupDto articleGroupDto = null;

                if (response.Succedded)
                {
                    SyncStatus.SucceddedRequests++;
                    articleGroupDto = _mapper.Map<ArticleGroupDto>(response.Result);
                    Entities.AddRange(_mapper.Map<List<Article>>(response.Result.Items));
                }

                Result.AddResponse(type, response.SearchWord, response.Succedded, articleGroupDto);
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
