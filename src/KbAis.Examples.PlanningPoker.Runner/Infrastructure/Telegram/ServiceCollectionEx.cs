using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public static class ServiceCollectionEx {
    public static IServiceCollection AddPlanningPokerTgServices(this IServiceCollection services) {
        services.AddOptions<TgBotOptions>()
            .BindConfiguration(TgBotOptions.ConfigurationSectionPath);

        const string telegramClientName = "telegram-bot-client";

        services.AddHttpClient(telegramClientName)
            .AddTypedClient<ITgBotClient>(static (httpClient, services) => {
                var options = services.GetRequiredService<IOptions<TgBotOptions>>().Value;

                return new TelegramBotClient(options: new(options.Token), httpClient);
            });

        services.AddTransient<TgUpdateReceiveService>();

        services.AddTransient<IUpdateHandler, TgUpdateHandler>();

        services.AddHostedService<TgUpdatePollingService>();

        services.AddTransient<ITgUpdateServices, TgUpdateServices>();

        return services;
    }
}
