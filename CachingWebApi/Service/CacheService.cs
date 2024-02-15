
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace CachingWebApi.Service;

public class CacheService : ICacheService
{
    private IDatabase _cacheDb;

    public CacheService()
    {
        var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        _cacheDb = redis.GetDatabase();
    }
    public async Task<T> GetData<T>(string key)
    {
        var value = await _cacheDb.StringGetAsync(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public async Task<object> RemoveData(string key)
    {
        var _exist = await _cacheDb.KeyExistsAsync(key);

        if(_exist)
            return _cacheDb.KeyDeleteAsync(key);
        return false;

    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return await _cacheDb.StringSetAsync(key,JsonSerializer.Serialize(value),expirtyTime);
    }
}
