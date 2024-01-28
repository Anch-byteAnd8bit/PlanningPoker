using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddTelegramBotServices(this IServiceCollection services) {
        services.AddOptions<TelegramBotOptions>()
            .BindConfiguration(TelegramBotOptions.ConfigurationSectionPath);

        services.AddHttpClient("telegram-bot-client")
            .AddTypedClient<ITelegramBotClient>(static (httpClient, services) => {
                var options = services.GetRequiredService<IOptions<TelegramBotOptions>>().Value;

                return new TelegramBotClient(options: new(options.Token), httpClient);
            });

        services.AddTransient<TelegramUpdateReceiveService>();

        services.AddTransient<TelegramUpdateHandler>();

        services.AddHostedService<TelegramBotPollingService>();

        return services;
    }
}