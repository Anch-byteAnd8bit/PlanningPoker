using Telegram.Bot;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TelegramUpdateReceiveService(
    ITelegramBotClient telegramBotClient, TelegramUpdateHandler telegramUpdateHandler
) {
    public Task ReceiveAsync(CancellationToken cancellationToken) {
        return telegramBotClient.ReceiveAsync(telegramUpdateHandler, cancellationToken: cancellationToken);
    }
}
