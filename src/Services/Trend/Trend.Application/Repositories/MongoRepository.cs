using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
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
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public void Add(T entity)
        {
            _collection.InsertOne(entity);
        }
    }
}
