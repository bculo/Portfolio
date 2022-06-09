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

namespace Trend.Application.Services
{
    public class GoogleSyncService : IGoogleSyncService
    {
        private readonly ILogger<GoogleSyncService> _logger;
        private readonly IGoogleSearchClient _searchService;
        private readonly IDateTime _time;

        public GoogleSyncResult Result { get; set; }
        public SyncStatus SyncStatus { get; set; }

        public GoogleSyncService(
            ILogger<GoogleSyncService> logger,
            IGoogleSearchClient searchService,
            IDateTime time)
        {
            _logger = logger;
            _searchService = searchService;

            Result = new GoogleSyncResult();
        }

        public async Task<GoogleSyncResult> Sync(Dictionary<ArticleType, IReadOnlyList<string>> articleTypesToSync)
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

            return Result;
        }

        private SyncStatus CreateSyncInstance()
        {
            return new SyncStatus
            {
                Id = ObjectId.GenerateNewId(),
                Created = _time.DateTime,
                Started = _time.DateTime,
            };
        }

        private async Task ExecuteSync(ArticleType type, IReadOnlyList<string> keyWords)
        {
            _logger.LogTrace("Sync for article type {0}", type.ToString());

            if(keyWords is null || keyWords.Count == 0)
            {
                _logger.LogInformation("Article type {0} does not contain key words", type.ToString());
            }

            var responses = await FetchData(keyWords!);

            HandleRequestResponse(responses, type);
        }

        private async Task<IReadOnlyList<GoogleResponseStatus>> FetchData(IReadOnlyList<string> keyWords) 
        {
            var requestList = keyWords!.Where(searchWord => !string.IsNullOrWhiteSpace(searchWord))
                                    .Select(searchWord => Tuple.Create(_searchService.Search(searchWord), searchWord))
                                    .ToList();

            _logger.LogTrace("HTTP request/requests sent");

            await Task.WhenAll(requestList.Select(i => i.Item1));

            return requestList.Select(item => new GoogleResponseStatus
            {
                Result = item.Item1.Result, //search response DTO
                SearchWord = item.Item2, //search word
            })
            .ToList()
            .AsReadOnly();
        }

        private void HandleRequestResponse(IReadOnlyList<GoogleResponseStatus> responses, ArticleType type)
        {
            foreach(var response in responses)
            {
                Result.AddResponse(type, response.SearchWord, response.Succedded, response.Result);

                SyncStatus.TotalRequests++;
                if (response.Succedded)
                {
                    SyncStatus.SucceddedRequests++;
                }
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
