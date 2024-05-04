using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YouAreShutUp.MessageBus.Options;

namespace YouAreShutUp.MessageBus;

public static class DependencyInjection
{
    private static IServiceCollection AddYouAreShutUpMassTransit(this IServiceCollection services)
    {
        return services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            // By default, sagas are in-memory, but should be changed to a durable
            // saga repository.
            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddConsumers(entryAssembly);
            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingAzureServiceBus((context, cfg) =>
            {
                var connectionString = 
                    context
                    .GetRequiredService<IOptions<AzureServiceBusConfiguration>>()
                    .Value
                    .ConnectionString;
                
                cfg.Host(connectionString);
                cfg.ConfigureEndpoints(context);
            });
        });
    }

    private static IConfigurationManager AddYouAreShutUpAzureBus(this IConfigurationManager config)
    {
        var azureConnectionString = config.GetSection(AzureAppConfiguration.Key).GetValue<string>("ConnectionString");
        config.AddAzureAppConfiguration(options => { options.Connect(azureConnectionString); });
        return config;
    }

    public static WebApplicationBuilder AddYouAreShutUpAzureBus(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddYouAreShutUpAzureBus();
        builder.Services.Configure<AzureAppConfiguration>(builder.Configuration.GetSection(AzureAppConfiguration.Key));
        builder.Services.Configure<AzureServiceBusConfiguration>(builder.Configuration.GetSection(AzureServiceBusConfiguration.Key));
        builder.Services.AddAzureAppConfiguration();
        builder.Services.AddYouAreShutUpMassTransit();
        return builder;
    }
}