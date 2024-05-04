namespace YouAreShutUp.SteamBot.Steam.Configuration.Options;

public class SteamStoreSettings
{
    public const string Key = nameof(SteamStoreSettings);
    public required string Language { get; init; }
}