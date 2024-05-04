namespace YouAreShutUp.SteamBot.Steam.Handlers.GetPlayerGamingPreferences;

public class GetPlayerGamingPreferencesQueryResult
{
    public required ulong ExternalMessageId { get; init; }
    public required IEnumerable<GetPlayerGamingPreferencesQueryResultPart> GamingPreferences { get; init; }
}

public class GetPlayerGamingPreferencesQueryResultPart
{
    public required string Genre { get; init; }
    public required double TotalHours { get; set; }
}