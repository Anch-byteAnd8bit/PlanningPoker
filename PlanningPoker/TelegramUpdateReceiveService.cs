using Telegram.Bot;

namespace PlanningPoker;

public class TelegramUpdateReceiveService(
    ITelegramBotClient telegramBotClient, TelegramUpdateHandler telegramUpdateHandler
) {
    public Task ReceiveAsync(CancellationToken cancellationToken) {
        return telegramBotClient.ReceiveAsync(telegramUpdateHandler, cancellationToken: cancellationToken);
    }
}
