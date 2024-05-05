using StackExchange.Redis;
using YouAreShutUp.SteamBot.Steam.Cache.Models;

namespace YouAreShutUp.SteamBot.Steam.Cache.Services;

public class RedisCacheService : ITypedCacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    public async Task<string?> GetAsync(TypedCacheKey key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var dbKey = key.GetKey();
        var value = await db.StringGetAsync(dbKey);
        _logger.LogDebug("Got the key: {Key} with value {Value}", dbKey, value);
        return value;
    }

    public async Task<bool> SetAsync(TypedCacheKey key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var dbKey = key.GetKey();
        _logger.LogDebug("Set the key: {Key} with value {Value}", dbKey, value);
        return await db.StringSetAsync(dbKey, value, TimeSpan.FromHours(12));
    }

    public async Task<bool> RemoveAsync(TypedCacheKey key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyDeleteAsync(key.GetKey());
    }
}