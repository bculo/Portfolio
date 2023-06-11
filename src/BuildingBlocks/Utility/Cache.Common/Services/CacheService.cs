using Cache.Abstract.Contracts;
using Cache.Common.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Time.Abstract.Contracts;

namespace Cache.Common.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisOptions _options;
        private readonly IDateTimeProvider _time;

        public CacheService(IDistributedCache cache, 
            IOptions<RedisOptions> options, 
            IDateTimeProvider time)
        {
            _cache = cache;
            _options = options.Value;
            _time = time;
        }

        public async Task Add(string identifier, object instance, bool useExpirationTime = true)
        {
            if (instance is null)
            {
                return;
            }

            string json = JsonConvert.SerializeObject(instance,
                Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            if (useExpirationTime)
            {
                await _cache.SetStringAsync(identifier, json, GetCacheOptions());
                return;
            }

            await _cache.SetStringAsync(identifier, json);
        }

        public async Task<T> Get<T>(string identifier) where T : class
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
                AbsoluteExpiration = _time.Now + TimeSpan.FromMinutes(_options.ExpirationTime)
            };
        }
    }
}
