using OrderProcessingSystem.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderProcessingSystem.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _cache;

    public CacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetDataAsync<T>(
    string key,
    T value,
    TimeSpan? expiry = null)
    {
        var jsonData = JsonSerializer.Serialize(value);

        await _cache.StringSetAsync(
            key,
            jsonData,
            expiry ?? TimeSpan.FromMinutes(5));
    }

    public async Task RemoveDataAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }
}