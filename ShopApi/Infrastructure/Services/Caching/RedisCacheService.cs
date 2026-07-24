using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ShopApi.Infrastructure.Services.Caching
{
    public class RedisCacheService:ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        //get from cache
        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);

            if (data is null) return default;
            return JsonSerializer.Deserialize<T>(data);
        }

        //set in cache
        public async Task SetAsync<T>(string key,T value,TimeSpan expiration)
        {
            var json =JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        expiration
                });
        }

        //remove catch
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        //redis ar
        public async Task<T> GetOrCreateAsync<T>(
    string key,
    Func<Task<T>> factory,// create custom dto
    TimeSpan expiration)
        {
            var data =await GetAsync<T>(key);

            if (data is not null)
            {
                Console.WriteLine("FROM REDIS");
                return data;
            }

            Console.WriteLine("FROM SQL");
            data = await factory();

            await SetAsync(
                key,
                data,
                expiration);

            return data;
        }


    }
}
