using Microsoft.Extensions.Caching.Memory;
using Service.Contracts.Base;
using System.Collections.Concurrent;

namespace Service.Base;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _prefixKeyMap = new();
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    private string GetCacheKey(string key, string? prefix) => string.IsNullOrWhiteSpace(prefix) ? key : $"{prefix}:{key}";

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, string? prefix = null)
    {
        var fullKey = GetCacheKey(key, prefix);

        if (_cache.TryGetValue(fullKey, out T value))
        {
            Console.WriteLine($"[CACHE HIT] {fullKey}");
            return value;
        }

        Console.WriteLine($"[CACHE MISS] {fullKey}");
        value = await factory();

        _cache.Set(fullKey, value, expiration ?? DefaultExpiration);
        if (!string.IsNullOrWhiteSpace(prefix)) RegisterKeyWithPrefix(prefix!, fullKey);

        return value;
    }

    public async Task<T?> GetAsync<T>(string key, string? prefix = null)
    {
        var fullKey = GetCacheKey(key, prefix);
        return _cache.TryGetValue(fullKey, out T value) ? value : default;
    }

    public bool TryGet<T>(string key, out T? value, string? prefix = null)
    {
        var fullKey = GetCacheKey(key, prefix);
        if (_cache.TryGetValue(fullKey, out T val))
        {
            value = val;
            return true;
        }
        value = default;
        return false;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, string? prefix = null)
    {
        Set(key, value, expiration, prefix);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan expiration, string? prefix = null)
    {
        var fullKey = GetCacheKey(key, prefix);
        _cache.Set(fullKey, value, expiration);

        if (!string.IsNullOrWhiteSpace(prefix))
            RegisterKeyWithPrefix(prefix!, fullKey);
    }

    public void Remove(string key, string? prefix = null)
    {
        var fullKey = GetCacheKey(key, prefix);
        _cache.Remove(fullKey);
    }

    public void RemoveByPrefix(string prefix)
    {
        if (_prefixKeyMap.TryRemove(prefix, out var keys))
        {
            foreach (var key in keys.Keys)
            {
                _cache.Remove(key);
            }
        }
    }

    private void RegisterKeyWithPrefix(string prefix, string key)
    {
        var keys = _prefixKeyMap.GetOrAdd(prefix, _ => new ConcurrentDictionary<string, byte>());
        keys.TryAdd(key, 0);
    }
}
