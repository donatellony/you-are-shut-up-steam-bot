using YouAreShutUp.MessageBus;
using YouAreShutUp.SteamBot.Steam.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddSteam();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.AddYouAreShutUpAzureBus();
var app = builder.Build();
app.Run();