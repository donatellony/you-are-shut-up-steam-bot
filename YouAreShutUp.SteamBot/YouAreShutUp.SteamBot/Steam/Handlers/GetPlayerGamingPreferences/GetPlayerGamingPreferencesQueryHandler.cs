using System.Text.Json;
using MediatR;
using Steam.Models.SteamCommunity;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;
using YouAreShutUp.SteamBot.Steam.Cache.Models;
using YouAreShutUp.SteamBot.Steam.Cache.Services;

namespace YouAreShutUp.SteamBot.Steam.Handlers.GetPlayerGamingPreferences;

public class
    GetPlayerGamingPreferencesQueryHandler : IRequestHandler<GetPlayerGamingPreferencesQuery,
    GetPlayerGamingPreferencesQueryResult>
{
    private readonly ISteamPlayerClient _steamPlayerClient;
    private readonly ISteamStoreClient _steamStoreClient;
    private readonly ILogger<GetPlayerGamingPreferencesQueryHandler> _logger;
    private readonly ITypedCacheService _cacheService;

    public GetPlayerGamingPreferencesQueryHandler(ISteamPlayerClient steamPlayerClient,
        ISteamStoreClient steamStoreClient, ILogger<GetPlayerGamingPreferencesQueryHandler> logger,
        ITypedCacheService cacheService)
    {
        _steamPlayerClient = steamPlayerClient;
        _steamStoreClient = steamStoreClient;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<GetPlayerGamingPreferencesQueryResult> Handle(GetPlayerGamingPreferencesQuery request,
        CancellationToken cancellationToken)
    {
        var cachedResponse = await TryGetCachedPreferences(request);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

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

        var result = new GetPlayerGamingPreferencesQueryResult
        {
            ExternalMessageId = request.ExternalMessageId,
            GamingPreferences = genreHours.Select(
                kv =>
                    new GetPlayerGamingPreferencesQueryResultPart { Genre = kv.Key, TotalHours = kv.Value }
            )
        };
        await SetValueToCache(request.SteamPlayerId, result);
        return result;
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

    private async Task<GetPlayerGamingPreferencesQueryResult?> TryGetCachedPreferences(
        GetPlayerGamingPreferencesQuery request)
    {
        var key = new TypedCacheKey(nameof(GetPlayerGamingPreferencesQueryResult), request.SteamPlayerId.ToString());
        var value = await _cacheService.GetAsync(key);
        if (value is null)
            return null;
        var cachedResult = JsonSerializer.Deserialize<GetPlayerGamingPreferencesQueryResult>(value);
        if (cachedResult is null)
            return null;
        var finalCachedResponse = new GetPlayerGamingPreferencesQueryResult
        {
            ExternalMessageId = request.ExternalMessageId, GamingPreferences = cachedResult.GamingPreferences
        };
        return finalCachedResponse;
    }

    private async Task<bool> SetValueToCache(ulong steamPlayerId, GetPlayerGamingPreferencesQueryResult result)
    {
        var key = new TypedCacheKey(nameof(GetPlayerGamingPreferencesQueryResult), steamPlayerId.ToString());
        var value = JsonSerializer.Serialize(result);
        return await _cacheService.SetAsync(key, value);
    }
}