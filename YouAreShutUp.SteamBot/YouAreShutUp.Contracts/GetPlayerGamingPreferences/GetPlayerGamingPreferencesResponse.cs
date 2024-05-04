namespace YouAreShutUp.Contracts.GetPlayerGamingPreferences;

public class GetPlayerGamingPreferencesResponse
{
    public required ulong ExternalMessageId { get; init; }
    public required IEnumerable<GetPlayerGamingPreferencesResponsePart> GamingPreferences { get; init; }
}

public class GetPlayerGamingPreferencesResponsePart
{
    public required string Genre { get; init; }
    public required double TotalHours { get; set; }
}