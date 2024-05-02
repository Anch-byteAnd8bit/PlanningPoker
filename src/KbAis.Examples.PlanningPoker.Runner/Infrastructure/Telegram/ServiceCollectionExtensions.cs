using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddTelegramBotServices(this IServiceCollection services) {
        services.AddOptions<TgBotOptions>()
            .BindConfiguration(TgBotOptions.ConfigurationSectionPath);

        services.AddHttpClient("telegram-bot-client")
            .AddTypedClient<ITgBotClient>(static (httpClient, services) => {
                var options = services.GetRequiredService<IOptions<TgBotOptions>>().Value;

                return new TelegramBotClient(options: new(options.Token), httpClient);
            });

        services.AddTransient<TgUpdateReceiveService>();

        services.AddTransient<IUpdateHandler, TgUpdateHandler>();

        services.AddHostedService<TgBotPollingService>();

        services.AddTransient<ITgRouterServices, TgRouterServices>();

        return services;
    }
}
