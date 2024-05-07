using System.Reflection;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Tomlyn.Extensions.Configuration;

var hostBuilder = WebApplication.CreateBuilder(args);

hostBuilder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole();

hostBuilder.Configuration
    .AddTomlFile("config.toml", optional: false)
    .AddTomlFile("config.dev.toml", optional: true);

hostBuilder.AddPersistenceServices();

hostBuilder.Services.AddTelegramBotServices();

hostBuilder.Services.
    AddMediatR(config => {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

// Configure monitoring
hostBuilder.ConfigureOpenTelemetry();

hostBuilder.Services.AddServiceDiscovery();

hostBuilder.Services.ConfigureHttpClientDefaults(config => {
    config.AddServiceDiscovery();
});

hostBuilder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

var host = hostBuilder.Build();

host.Run();

public static class HostBuilderExtensions {
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder) {
        builder.Logging.AddOpenTelemetry(config => {
            config.IncludeFormattedMessage = true;
            config.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(config => {
                config
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(config => {
                config
                    .AddAspNetCoreInstrumentation()
                    .AddSource(TgUpdateHandler.TgUpdateHandlerSourceName);
            });

        builder.Services.AddOpenTelemetry()
            .UseOtlpExporter();

        return builder;
    }
}
