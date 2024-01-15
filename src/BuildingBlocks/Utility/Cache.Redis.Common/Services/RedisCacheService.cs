using System.Text.Json;
using Cache.Abstract.Contracts;
using Cache.Redis.Common.Redis;
using Cache.Redis.Common.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Cache.Redis.Common.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Add(string identifier, object instance)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            ArgumentNullException.ThrowIfNull(instance);
            
            var json = JsonSerializer.Serialize(instance,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            
            await _cache.SetStringAsync(identifier, json);
        }

        public async Task AddWithSlidingExp(string identifier, object instance, TimeSpan span)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            ArgumentNullException.ThrowIfNull(instance);
            TimeSpanGuard(span);
            
            var cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = span
            };
            
            var json = JsonSerializer.Serialize(instance,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            
            await _cache.SetStringAsync(identifier, json, cacheOptions);
        }

        public async Task AddWithAbsoluteExp(string identifier, object instance, TimeSpan span)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            ArgumentNullException.ThrowIfNull(instance);
            TimeSpanGuard(span);
            
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = span,
            };
            
            var json = JsonSerializer.Serialize(instance,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            
            await _cache.SetStringAsync(identifier, json, cacheOptions);
        }

        public async Task<T?> Get<T>(string identifier) where T : class
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            
            var value = await _cache.GetStringAsync(identifier);
            if (value is null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<string?> Get(string identifier)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

            return await _cache.GetStringAsync(identifier);
        }
        
        private void TimeSpanGuard(TimeSpan span)
        {
            if (span.Milliseconds < 0)
            {
                throw new RedisCacheOptionException("Invalid time span argument. Must be greater than 0");
            }
        }
    }
}
