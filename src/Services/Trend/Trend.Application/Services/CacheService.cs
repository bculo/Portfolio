using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Time.Common.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Options;

namespace Trend.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisOptions _options;
        private readonly IDateTime _time;

        public CacheService(IDistributedCache cache, IOptions<RedisOptions> options, IDateTime time)
        {
            _cache = cache;
            _options = options.Value;
            _time = time;
        }

        public async Task Add(string identifier, object instance)
        {
            if (instance is null)
            {
                return;
            }

            string json = JsonConvert.SerializeObject(instance, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            await _cache.SetStringAsync(identifier, json, GetCacheOptions());
        }

        public async Task<T> Get<T>(string identifier) where T : class
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return null!;
            }

            string value = await _cache.GetStringAsync(identifier);

            if(value == null)
            {
                return null!;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<string> Get(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return null!;
            }

            return await _cache.GetStringAsync(identifier);
        }

        public async Task<List<T>> GetList<T>(string identifier) where T : class
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return null!;
            }

            string value = await _cache.GetStringAsync(identifier);

            if (value == null)
            {
                return null!;
            }

            return JsonConvert.DeserializeObject<List<T>>(value);
        }

        private DistributedCacheEntryOptions GetCacheOptions()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = _time.DateTime + TimeSpan.FromMinutes(_options.RememberTime)
            };
        }
    }
}
