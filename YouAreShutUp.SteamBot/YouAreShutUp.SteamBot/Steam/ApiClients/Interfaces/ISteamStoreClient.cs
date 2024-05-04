using Steam.Models.SteamStore;

namespace YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;

public interface ISteamStoreClient
{
    Task<StoreAppDetailsDataModel?> GetAppDetailsAsync(uint appId);
}