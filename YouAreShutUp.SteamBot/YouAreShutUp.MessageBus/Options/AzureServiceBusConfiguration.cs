namespace YouAreShutUp.MessageBus.Options;

public class AzureServiceBusConfiguration
{
    public const string Key = nameof(AzureServiceBusConfiguration);
    public required string ConnectionString { get; init; }
}