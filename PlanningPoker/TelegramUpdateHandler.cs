using Microsoft.Extensions.Logging;
using PlanningPoker.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PlanningPoker;
public class TelegramUpdateHandler(ILogger<IUpdateHandler> logger) : IUpdateHandler {

    private MasterSetting? master;

    public async Task HandleUpdateAsync(
        ITelegramBotClient client, Update update, CancellationToken cancellationToken
    )
    {
        using var _ = logger.BeginScope("Handle Telegram Update: {UpdateId}", update.Id);
        
        var handleTask = update switch
        {
            { Message.Text: { } messageText } when messageText.StartsWith("/")
                => HandleCommand(client, update, messageText, cancellationToken),
            { Message.From.Id: { } messageId } when master != null && messageId == master.Id 
            => HandleCheckCode(client, update, cancellationToken),
            _
                => HandleDefault(client, update, cancellationToken)
        };

        await handleTask;
    }

    private Task HandleCommand(
        ITelegramBotClient client, Update update, string messageText, CancellationToken cancellationToken
    ) {
        return messageText switch {
            "/session"
                => HandleCommandNewSession(client, update, cancellationToken),
            "/master:set" 
                => HandleCommandSetMaster(client, update, cancellationToken),
            _
                => HandleDefault(client, update, cancellationToken)
        };
    }

    private Task<Message> HandleCommandNewSession(
        ITelegramBotClient client, Update update, CancellationToken cancellationToken
    )
    {
        var chatId = update.Message!.Chat.Id;

        var messageId = update.Message.MessageId;

        logger.LogDebug("Received a command to start new session in chat {ChatId}", chatId);
        return client.SendTextMessageAsync(
            chatId, "Принято", replyToMessageId: messageId, cancellationToken: cancellationToken
        );
    }

    private Task<Message> HandleCommandSetMaster(
        ITelegramBotClient client, Update update, CancellationToken cancellationToken
    )
    {
        var sendId = update.Message.From.Id;
        var chatId = update.Message.Chat.Id;
        var messageId = update.Message.MessageId;
        logger.LogDebug("Received a command to start new session in chat {ChatId}", sendId);
        if (master == null || !master.IsMaster)
        {
            string newcode = MasterSetting.GenerateCode();
            master = new MasterSetting(sendId, chatId, newcode);
            return client.SendTextMessageAsync(
                sendId, newcode, cancellationToken: cancellationToken
            );
        }
        return client.SendTextMessageAsync(
            master.ChatId, "Мастер уже назначен!!!", replyToMessageId: messageId, cancellationToken: cancellationToken);

    }

    private Task<Message> HandleCheckCode(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var messageId = update.Message.MessageId;
        var message = update.Message;
        master.IsMaster = MasterSetting.CheckCode(message.Text, master.MasterCode);
        logger.LogDebug("Received update has not been handled: {@Update}", update);
        if (master.IsMaster)
            return client.SendTextMessageAsync(
            master.ChatId, "Мастер назначен", replyToMessageId: messageId, cancellationToken: cancellationToken);
        
        return client.SendTextMessageAsync(
            master.ChatId, "неправильный код", replyToMessageId: messageId, cancellationToken: cancellationToken);
    }

    private Task HandleDefault(
        ITelegramBotClient client, Update update, CancellationToken cancellationToken
    )
    {
        logger.LogDebug("Received update has not been handled: {@Update}", update);

        return Task.CompletedTask;
    }
    public Task HandlePollingErrorAsync(
        ITelegramBotClient client, Exception exception, CancellationToken cancellationToken
    )
    {
        logger.LogError(exception, "Caught unexpected exception during telegram update handle");

        return Task.CompletedTask;
    }
}
