using YouAreShutUp.Contracts.GetPlayerGamingPreferences;
using YouAreShutUp.SteamBot.Steam.Handlers.GetPlayerGamingPreferences;

namespace YouAreShutUp.SteamBot.Steam.Mappers;

internal static class SteamMapper
{
    internal static GetPlayerGamingPreferencesQuery MapToQuery(this GetPlayerGamingPreferencesRequest request)
    {
        return new GetPlayerGamingPreferencesQuery
            { SteamPlayerId = request.SteamPlayerId, ExternalMessageId = request.ExternalMessageId };
    }

    internal static GetPlayerGamingPreferencesResponse MapToResponse(this GetPlayerGamingPreferencesQueryResult result)
    {
        return new GetPlayerGamingPreferencesResponse
        {
            ExternalMessageId = result.ExternalMessageId,
            GamingPreferences = result.GamingPreferences
                .Select(
                    x => new GetPlayerGamingPreferencesResponsePart
                    {
                        Genre = x.Genre,
                        TotalHours = x.TotalHours
                    }
                )
        };
    }
}