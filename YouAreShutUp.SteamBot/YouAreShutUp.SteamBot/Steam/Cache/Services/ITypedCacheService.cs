using YouAreShutUp.SteamBot.Steam.Cache.Models;

namespace YouAreShutUp.SteamBot.Steam.Cache.Services;

public interface ITypedCacheService : ICacheService<TypedCacheKey, string>;