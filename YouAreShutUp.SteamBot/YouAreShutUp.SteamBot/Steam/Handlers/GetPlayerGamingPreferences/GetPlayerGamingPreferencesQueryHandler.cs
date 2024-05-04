using MediatR;
using Steam.Models.SteamCommunity;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;

namespace YouAreShutUp.SteamBot.Steam.Handlers.GetPlayerGamingPreferences;

public class
    GetPlayerGamingPreferencesQueryHandler : IRequestHandler<GetPlayerGamingPreferencesQuery,
    GetPlayerGamingPreferencesQueryResult>
{
    private readonly ISteamPlayerClient _steamPlayerClient;
    private readonly ISteamStoreClient _steamStoreClient;
    private readonly ILogger<GetPlayerGamingPreferencesQueryHandler> _logger;

    public GetPlayerGamingPreferencesQueryHandler(ISteamPlayerClient steamPlayerClient,
        ISteamStoreClient steamStoreClient, ILogger<GetPlayerGamingPreferencesQueryHandler> logger)
    {
        _steamPlayerClient = steamPlayerClient;
        _steamStoreClient = steamStoreClient;
        _logger = logger;
    }

    public async Task<GetPlayerGamingPreferencesQueryResult> Handle(GetPlayerGamingPreferencesQuery request,
        CancellationToken cancellationToken)
    {
        var ownedGames = await _steamPlayerClient.GetOwnedGamesAsync(request.SteamPlayerId);
        Dictionary<string, double> genreHours = new();
        foreach (var ownedGame in ownedGames)
        {
            try
            {
                await AddAppGenreHours(ownedGame, genreHours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting app details for a game with AppId {AppId}",
                    ownedGame.AppId);
            }
        }

        return new GetPlayerGamingPreferencesQueryResult
        {
            ExternalMessageId = request.ExternalMessageId,
            GamingPreferences = genreHours.Select(
                kv =>
                    new GetPlayerGamingPreferencesQueryResultPart { Genre = kv.Key, TotalHours = kv.Value }
            )
        };
    }

    private async Task AddAppGenreHours(OwnedGameModel? ownedGame, Dictionary<string, double> genreHours)
    {
        if (ownedGame is null)
            return;
        var appDetails = await _steamStoreClient.GetAppDetailsAsync(ownedGame.AppId);
        if (appDetails is null)
            return;
        foreach (var genre in appDetails.Genres)
        {
            if (!genreHours.ContainsKey(genre.Description))
            {
                genreHours[genre.Description] = ownedGame.PlaytimeForever.TotalHours;
                continue;
            }

            genreHours[genre.Description] += ownedGame.PlaytimeForever.TotalHours;
        }
    }
}