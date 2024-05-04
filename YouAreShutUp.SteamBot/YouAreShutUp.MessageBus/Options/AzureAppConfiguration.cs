namespace YouAreShutUp.MessageBus.Options;

public class AzureAppConfiguration
{
    public const string Key = nameof(AzureAppConfiguration);
    public required string ConnectionString { get; init; }
}