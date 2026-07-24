namespace ShopApi.Infrastructure.Services.Caching
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration);

        Task RemoveAsync(string key);

        //**redis
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan expiration);
    }
}
