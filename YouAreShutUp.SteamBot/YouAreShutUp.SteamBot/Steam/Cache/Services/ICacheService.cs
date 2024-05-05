namespace YouAreShutUp.SteamBot.Steam.Cache.Services;

public interface ICacheService<TKey, TValue>
{
    Task<TValue?> GetAsync(TKey key);
    Task<bool> SetAsync(TKey key, TValue value);
    Task<bool> RemoveAsync(TKey key);
}