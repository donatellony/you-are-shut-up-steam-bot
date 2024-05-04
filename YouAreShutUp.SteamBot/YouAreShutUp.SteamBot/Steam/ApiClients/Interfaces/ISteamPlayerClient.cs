using Steam.Models.SteamCommunity;

namespace YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;

public interface ISteamPlayerClient
{
    Task<IReadOnlyCollection<OwnedGameModel>> GetOwnedGamesAsync(ulong steamId);
}