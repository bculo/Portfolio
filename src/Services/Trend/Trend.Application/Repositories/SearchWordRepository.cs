using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Repositories
{
    public class SearchWordRepository : MongoAuditableRepository<SearchWord>, ISearchWordRepository
    {
        public SearchWordRepository(IMongoClient client, 
            IOptions<MongoOptions> options,
            IDateTimeProvider timeProvider) 
            : base(client, options, timeProvider)
        {
        }

        public Task<bool> IsDuplicate(string searchWord, SearchEngine engine, CancellationToken token)
        {
            var instance = _collection
                .Find(i => string.Equals(i.Word, searchWord, StringComparison.CurrentCultureIgnoreCase) && i.Engine == engine)
                .FirstOrDefault();
            
            return Task.FromResult(instance != null);
        }      
    }
}
