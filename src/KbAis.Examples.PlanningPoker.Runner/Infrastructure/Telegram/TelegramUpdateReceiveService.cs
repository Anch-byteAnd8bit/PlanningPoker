using Telegram.Bot;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TelegramUpdateReceiveService(
    ITelegramBotClient telegramBotClient, TelegramUpdateRouter telegramUpdateRouter
) {
    public Task ReceiveAsync(CancellationToken cancellationToken) {
        return telegramBotClient.ReceiveAsync(telegramUpdateRouter, cancellationToken: cancellationToken);
    }
}
