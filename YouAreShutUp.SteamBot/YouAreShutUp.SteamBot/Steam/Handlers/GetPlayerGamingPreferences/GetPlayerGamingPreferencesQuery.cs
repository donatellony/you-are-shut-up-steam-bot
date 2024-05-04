using MediatR;

namespace YouAreShutUp.SteamBot.Steam.Handlers.GetPlayerGamingPreferences;

public class GetPlayerGamingPreferencesQuery : IRequest<GetPlayerGamingPreferencesQueryResult>
{
    public required ulong SteamPlayerId { get; init; }
    public required ulong ExternalMessageId { get; init; }
}