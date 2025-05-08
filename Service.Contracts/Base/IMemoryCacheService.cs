namespace Service.Contracts.Base;

public interface IMemoryCacheService
{
    Task<T?> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T value, TimeSpan expiration, string? prefix = null);

    void Set<T>(string key, T value, TimeSpan duration);

    Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? absoluteExpireTime = null, string? prefix = null);

    void Remove(string key);

    void RemoveByPrefix(string prefix);
}
