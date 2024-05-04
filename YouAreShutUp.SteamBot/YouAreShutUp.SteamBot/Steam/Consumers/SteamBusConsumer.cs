using MassTransit;
using MediatR;
using YouAreShutUp.Contracts.GetPlayerGamingPreferences;
using YouAreShutUp.SteamBot.Steam.Mappers;

namespace YouAreShutUp.SteamBot.Steam.Consumers;

public class SteamBusConsumer : IConsumer<GetPlayerGamingPreferencesRequest>
{
    private readonly IMediator _mediator;
    private readonly IBus _bus;

    public SteamBusConsumer(IMediator mediator, IBus bus)
    {
        _mediator = mediator;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<GetPlayerGamingPreferencesRequest> context)
    {
        var query = context.Message.MapToQuery();
        var response = await _mediator.Send(query);
        var mappedResponse = response.MapToResponse();
        await _bus.Publish(mappedResponse);
    }
}