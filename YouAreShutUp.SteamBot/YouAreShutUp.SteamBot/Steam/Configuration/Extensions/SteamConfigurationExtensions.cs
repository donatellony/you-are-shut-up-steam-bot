using Microsoft.Extensions.Options;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using YouAreShutUp.SteamBot.Steam.ApiClients.Implementations;
using YouAreShutUp.SteamBot.Steam.ApiClients.Interfaces;
using YouAreShutUp.SteamBot.Steam.Configuration.Options;

namespace YouAreShutUp.SteamBot.Steam.Configuration.Extensions;

internal static class SteamConfigurationExtensions
{
    internal static WebApplicationBuilder AddSteam(this WebApplicationBuilder builder)
    {
        AddConfiguration(builder);
        AddServices(builder.Services);
        return builder;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ISteamWebInterfaceFactory>(provider =>
        {
            var steamApiKey = provider.GetRequiredService<IOptions<SteamApiSettings>>().Value.DevApiKey;
            return new SteamWebInterfaceFactory(steamApiKey);
        });
        services.AddSingleton<IPlayerService>(provider =>
        {
            var webInterfaceFactory = provider.GetRequiredService<ISteamWebInterfaceFactory>();
            return webInterfaceFactory.CreateSteamWebInterface<PlayerService>();
        });
        services.AddSingleton<SteamStore>(provider =>
        {
            var webInterfaceFactory = provider.GetRequiredService<ISteamWebInterfaceFactory>();
            return webInterfaceFactory.CreateSteamStoreInterface();
        });

        services.AddSingleton<ISteamPlayerClient, SteamPlayerClient>();
        services.AddSingleton<ISteamStoreClient, SteamStoreClient>();
    }

    private static void AddConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.Configure<SteamApiSettings>(builder.Configuration.GetSection(SteamApiSettings.Key));
        builder.Services.Configure<SteamStoreSettings>(builder.Configuration.GetSection(SteamStoreSettings.Key));
    }
}