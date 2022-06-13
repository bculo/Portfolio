using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Repositories
{
    public interface IMongoContext : IDisposable
    {
        IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);
        void AddCommand(Func<Task> command);
        Task<int> SaveChangesAsync();
    }
}
