using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure;

internal static class OtleHostBuilderEx {
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