using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner;

public class TgNotificationService
{
    public Task<Message> SendTgNotification(ITgBotClient client, PlanningPokerDataContext ctx, Cancellation c, long chatId, long messageId)
    {
        var outbox = ctx.outbox_table.ToList();
        if (outbox.Any(x => x.id == messageId))
        {
            var message = outbox.First(x => x.id == messageId);
            return client.SendTextMessageAsync(chatId, message.textmessage, cancellationToken: c);
        }
        return null;
    }
}