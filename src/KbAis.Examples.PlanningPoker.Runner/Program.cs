using System.Reflection;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tomlyn.Extensions.Configuration;

var hostBuilder = Host.CreateEmptyApplicationBuilder(new() { Args = args });

hostBuilder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole();

hostBuilder.Configuration
    .AddTomlFile("config.toml", optional: false)
    .AddTomlFile("config.dev.toml", optional: true);

hostBuilder.Services
    //.AddPersistenceServices()
    .AddTelegramBotServices();

hostBuilder.Services.
    AddMediatR(config => {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

var host = hostBuilder.Build();
host.Run();
