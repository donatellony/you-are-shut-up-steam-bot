namespace YouAreShutUp.SteamBot.Steam.Configuration.Options;

public class SteamApiSettings
{
    public const string Key = nameof(SteamApiSettings);
    public required string DevApiKey { get; init; }
}