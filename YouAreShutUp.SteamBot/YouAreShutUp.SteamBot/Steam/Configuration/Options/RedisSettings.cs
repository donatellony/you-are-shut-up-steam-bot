namespace YouAreShutUp.SteamBot.Steam.Configuration.Options;

public class RedisSettings
{
    public const string Key = nameof(RedisSettings);
    public required string ConnectionString { get; init; }
}