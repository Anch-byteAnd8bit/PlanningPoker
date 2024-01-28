using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateEmptyApplicationBuilder(new() { Args = args });

hostBuilder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole();

hostBuilder.Configuration
    .AddJsonFile("appsettings.json", optional: false);

hostBuilder.Services
    .AddPersistenceServices()
    .AddTelegramBotServices();

var host = hostBuilder.Build();

host.Run();
