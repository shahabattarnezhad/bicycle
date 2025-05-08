 namespace Service.Contracts.Base;

public interface IMemoryCacheService
{
    Task<T?> GetAsync<T>(string key, string? prefix = null);

    Task SetAsync<T>(string key, T value, TimeSpan expiration, string? prefix = null);

    void Set<T>(string key, T value, TimeSpan expiration, string? prefix = null);

    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, string? prefix = null);

    bool TryGet<T>(string key, out T? value, string? prefix = null);

    void Remove(string key, string? prefix = null);

    void RemoveByPrefix(string prefix);
}
