using Microsoft.Extensions.Options;
using Steam.Models.SteamStore;
using SteamWebAPI2.Interfaces;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;
using YouAreShutUp.SteamBot.Steam.Configuration.Options;

namespace YouAreShutUp.SteamBot.Steam.ApiClients.Implementations;

public class SteamStoreClient : ISteamStoreClient
{
    // Cannot inject ISteamStore as it is internal :\
    private readonly SteamStore _steamStore;
    private readonly IOptions<SteamStoreSettings> _storeSettings;

    public SteamStoreClient(SteamStore steamStore, IOptions<SteamStoreSettings> storeSettings)
    {
        _steamStore = steamStore;
        _storeSettings = storeSettings;
    }

    public async Task<StoreAppDetailsDataModel?> GetAppDetailsAsync(uint appId)
    {
        var appDetails = await _steamStore.GetStoreAppDetailsAsync(appId, language: _storeSettings.Value.Language);
        return appDetails;
    }
}