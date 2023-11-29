using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class SearchWordRepository : MongoRepository<SearchWord>, ISearchWordRepository
    {
        public SearchWordRepository(IMongoClient client, IOptions<MongoOptions> options) 
            : base(client, options)
        {

        }

        public Task<bool> IsDuplicate(string searchWord, SearchEngine engine, CancellationToken token)
        {
            var instance = _collection.Find(i => string.Equals(i.Word, searchWord, StringComparison.CurrentCultureIgnoreCase) && i.Engine == engine).FirstOrDefault();
            return Task.FromResult(instance != null);
        }      
    }
}
