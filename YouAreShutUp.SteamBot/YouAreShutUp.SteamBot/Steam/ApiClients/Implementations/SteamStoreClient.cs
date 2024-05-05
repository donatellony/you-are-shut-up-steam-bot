using System.Text.Json;
using Microsoft.Extensions.Options;
using Steam.Models.SteamStore;
using SteamWebAPI2.Interfaces;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;
using YouAreShutUp.SteamBot.Steam.Cache.Models;
using YouAreShutUp.SteamBot.Steam.Cache.Services;
using YouAreShutUp.SteamBot.Steam.Configuration.Options;

namespace YouAreShutUp.SteamBot.Steam.ApiClients.Implementations;

public class SteamStoreClient : ISteamStoreClient
{
    // Cannot inject ISteamStore as it is internal :\
    private readonly SteamStore _steamStore;
    private readonly IOptions<SteamStoreSettings> _storeSettings;
    private readonly ITypedCacheService _cacheService;

    public SteamStoreClient(SteamStore steamStore, IOptions<SteamStoreSettings> storeSettings,
        ITypedCacheService cacheService)
    {
        _steamStore = steamStore;
        _storeSettings = storeSettings;
        _cacheService = cacheService;
    }

    public async Task<StoreAppDetailsDataModel?> GetAppDetailsAsync(uint appId)
    {
        var cachedDetails = await TryGetAppDetailsFromCache(appId);
        if (cachedDetails is not null)
            return cachedDetails;
        var appDetails = await _steamStore.GetStoreAppDetailsAsync(appId, language: _storeSettings.Value.Language);
        if (appDetails is not null)
            await SetValueToCache(appId, appDetails);
        return appDetails;
    }

    private async Task<StoreAppDetailsDataModel?> TryGetAppDetailsFromCache(uint appId)
    {
        var key = new TypedCacheKey(nameof(StoreAppDetailsDataModel), appId.ToString());
        var value = await _cacheService.GetAsync(key);
        if (value is null)
            return null;
        var cachedResult = JsonSerializer.Deserialize<StoreAppDetailsDataModel>(value);
        return cachedResult;
    }

    private async Task<bool> SetValueToCache(uint appId, StoreAppDetailsDataModel steamResponse)
    {
        var key = new TypedCacheKey(nameof(StoreAppDetailsDataModel), appId.ToString());
        var value = JsonSerializer.Serialize(steamResponse);
        return await _cacheService.SetAsync(key, value);
    }
}