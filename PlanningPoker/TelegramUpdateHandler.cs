using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PlanningPoker;

public class TelegramUpdateHandler(ILogger<IUpdateHandler> logger) : IUpdateHandler {
    public async Task HandleUpdateAsync(
        ITelegramBotClient clinet, Update update, CancellationToken cancellationToken
    ) {
        using var _ = logger.BeginScope("Handle Telegram Update: {UpdateId}", update.Id);

        var handleTask = update switch {
            { Message.Text: { } messageText } when messageText.StartsWith("/")
                => HandleCommand(clinet, update, messageText, cancellationToken),
            _
                => HandleDefault(clinet, update, cancellationToken)
        };

        await handleTask;
    }

    private Task HandleCommand(
        ITelegramBotClient client, Update update, string messageText, CancellationToken cancellationToken
    ) {
        return messageText switch {
            "/session"
                => HandleCommandNewSession(client, update, cancellationToken),
            _
                => HandleDefault(client, update, cancellationToken)
        };
    }

    private Task<Message> HandleCommandNewSession(
        ITelegramBotClient client, Update update, CancellationToken cancellationToken
    ) {
        var chatId = update.Message!.Chat.Id;

        var messageId = update.Message.MessageId;

        logger.LogDebug("Received a command to start new session in chat {ChatId}", chatId);

        return client.SendTextMessageAsync(
            chatId, "Принятор", replyToMessageId: messageId, cancellationToken: cancellationToken
        );
    }

    private Task HandleDefault(
        ITelegramBotClient clinet, Update update, CancellationToken cancellationToken
    ) {
        logger.LogDebug("Received update has not been handled: {@Update}", update);

        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient client, Exception exception, CancellationToken cancellationToken
    ) {
        logger.LogError(exception, "Caught unexpected exception during telegram update handle");

        return Task.CompletedTask;
    }
}
