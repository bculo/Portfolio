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
    public class SyncSettingRepository : MongoRepository<SyncSetting>, ISyncSettingRepository
    {
        public SyncSettingRepository(IOptions<MongoOptions> options) : base(options)
        {

        }

        public async Task<bool> IsDuplicate(string searchWord, SearchEngine engine)
        {
            var instance = _collection.Find(i => i.SearchWord.ToLower() == searchWord.ToLower() && i.Engine == engine).FirstOrDefault();
            return instance != null;
        }
    }
}
