using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordRepository : IMongoAuditableRepository<SearchWord>
    {
        Task<SearchWordSyncDetailResQuery?> GetSearchWordSyncInfo(string searchWordId, CancellationToken token = default);
        Task<bool> IsDuplicate(string searchWord, SearchEngine engine, CancellationToken token = default);
        Task<PageResQuery<SearchWord>> Filter(SearchWordFilterReqQuery req, CancellationToken token = default);
    }
}
