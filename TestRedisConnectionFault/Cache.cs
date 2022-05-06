using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace TestRedisConnectionFault;

public class Cache
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> Get<T>(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                return value != null ? JsonConvert.DeserializeObject<T>(value) : default;
            }
            catch (Exception e)
            {
                //We ignore the error because the service should not fail because of a faulty cache.
                return default;
            }
        }
    }
}