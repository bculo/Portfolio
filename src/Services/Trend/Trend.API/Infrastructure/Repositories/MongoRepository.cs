using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.API.Entities;
using Trend.API.Interfaces;
using Trend.API.Options;

namespace Trend.API.Infrastructure.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IDocument
    {
        private readonly MongoOptions _options;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<MongoOptions> options)
        {
            _options = options.Value;

            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            _collection = database.GetCollection<T>(nameof(T));
        }
    }
}
