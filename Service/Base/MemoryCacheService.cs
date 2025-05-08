using Microsoft.Extensions.Caching.Memory;
using Service.Contracts.Base;
using System.Collections.Concurrent;

namespace Service.Base;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _prefixKeyMap = new();

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetOrCreateAsync<T>(
        string cacheKey,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpireTime = null,
        string? prefix = null)
    {
        if (_cache.TryGetValue(cacheKey, out T value))
        {
            Console.WriteLine($"[CACHE HIT] {cacheKey}");
            return value;
        }

        Console.WriteLine($"[CACHE MISS] {cacheKey}");
        value = await factory();

        _cache.Set(cacheKey, value, absoluteExpireTime ?? TimeSpan.FromMinutes(5));

        if (!string.IsNullOrWhiteSpace(prefix))
            RegisterKeyWithPrefix(prefix, cacheKey);

        return value;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, string? prefix = null)
    {
        _cache.Set(key, value, expiration);

        if (!string.IsNullOrWhiteSpace(prefix))
            RegisterKeyWithPrefix(prefix, key);

        return Task.CompletedTask;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return _cache.TryGetValue(key, out T value) ? value : default;
    }

    public void Set<T>(string key, T value, TimeSpan duration)
    {
        _cache.Set(key, value, duration);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void RemoveByPrefix(string prefix)
    {
        if (_prefixKeyMap.TryRemove(prefix, out var keys))
        {
            foreach (var key in keys)
            {
                _cache.Remove(key);
            }
        }
    }

    private void RegisterKeyWithPrefix(string prefix, string key)
    {
        _prefixKeyMap.AddOrUpdate(
            prefix,
            _ => new ConcurrentBag<string> { key },
            (_, existing) =>
            {
                existing.Add(key);
                return existing;
            });
    }
}
