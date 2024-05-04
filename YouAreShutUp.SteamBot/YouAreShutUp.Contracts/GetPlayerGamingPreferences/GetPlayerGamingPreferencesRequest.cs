namespace YouAreShutUp.Contracts.GetPlayerGamingPreferences;

public class GetPlayerGamingPreferencesRequest
{
    public required ulong SteamPlayerId { get; init; }
    public required ulong ExternalMessageId { get; init; }
}