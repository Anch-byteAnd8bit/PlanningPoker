using Telegram.Bot;
using Telegram.Bot.Polling;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TgUpdateReceiveService(ITgBotClient client, IUpdateHandler handler) {
    public Task ReceiveAsync(Cancellation c) {
        return client.ReceiveAsync(handler, cancellationToken: c);
    }
}
