using System.Collections.Immutable;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;

namespace YouAreShutUp.SteamBot.Steam.ApiClients.Implementations;

public class SteamPlayerClient : ISteamPlayerClient
{
    private readonly IPlayerService _playerService;

    public SteamPlayerClient(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    
    public async Task<IReadOnlyCollection<OwnedGameModel>> GetOwnedGamesAsync(ulong steamId)
    {
        var steamResult = await _playerService.GetOwnedGamesAsync(steamId, includeAppInfo: true, includeFreeGames: true);
        if (steamResult is null)
            return Enumerable.Empty<OwnedGameModel>().ToImmutableArray();
        return steamResult.Data.OwnedGames;
    }
}