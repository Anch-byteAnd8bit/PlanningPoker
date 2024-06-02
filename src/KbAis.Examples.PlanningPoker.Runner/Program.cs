using KbAis.Examples.PlanningPoker.Runner.Infrastructure;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Pipeline;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Tomlyn.Extensions.Configuration;

// TODO: Use Generic Host, fix otel
var hostBuilder = WebApplication.CreateBuilder(args);

hostBuilder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole();

hostBuilder.Configuration
    .AddTomlFile("config.toml", optional: false)
    .AddTomlFile("config.dev.toml", optional: true);


hostBuilder.Services
    .AddPlanningPokerTgServices()
    .AddPlanningPokerPipeline();

hostBuilder.AddPlanningPokerPersistenceServices();

hostBuilder.ConfigureOpenTelemetry();

var host = hostBuilder.Build();

host.Run();