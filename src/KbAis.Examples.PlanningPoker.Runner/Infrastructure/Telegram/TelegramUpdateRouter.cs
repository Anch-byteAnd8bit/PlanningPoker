using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public interface ICommand : IRequest;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand;

public record DefaultCommand(Update Update) : ICommand;

internal sealed class DefaultCommandHandler(ILogger<DefaultCommandHandler> logger) : ICommandHandler<DefaultCommand> {
    public Task Handle(DefaultCommand command, CancellationToken c) {
        logger.LogDebug("Received update has not been handled: {@Update}", command.Update);

        return Task.CompletedTask;
    }
}

public interface ITelegramRouterContext {
    ILogger<IUpdateHandler> Logger { get; }

    Task Dispatch<T>(T command, CancellationToken c) where T : ICommand;
}

public class TelegramUpdateRouter(ITelegramRouterContext ctx) : IUpdateHandler {
    public async Task HandleUpdateAsync(ITelegramBotClient client, Update u, CancellationToken c) {
        using var _ = ctx.Logger.BeginScope("Handle Telegram Update: {UpdateId}", u.Id);
        
        var command = u switch {
            { Message: var m, Message.Chat.Type: ChatType.Group } => HandleGroupMessage(client, u, m),
            _ => new DefaultCommand(u),
        };

        // NOTE: Have to await here to prevent logger scope from disposing while handling command.
        await ctx.Dispatch(command, c);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exp, CancellationToken c) {
        ctx.Logger.LogError(exp, "Caught unexpected exception on telegram update polling");

        return Task.CompletedTask;
    }

    private ICommand HandleGroupMessage(ITelegramBotClient client, Update u, Message m) {
        return m switch {
            // Группа был создана с ботом => Группа => Новый проект; Создатель группы => Организатор
            { Type: MessageType.GroupCreated } => new DefaultCommand(u),
            { Type: MessageType.ChatMembersAdded } => HandleChatMembersAdded(client, u, m),
            _ => new DefaultCommand(u)
        };
    }

    private ICommand HandleChatMembersAdded(ITelegramBotClient client, Update u, Message m) {
        if (m.NewChatMembers!.Any(x => x.Id == client.BotId)) {
            // Бот добавлен в группу => Группа => Новый проект; Добавлявший в группы => Организатор
            return new DefaultCommand(u);
        }

        return new DefaultCommand(u);
    }
}
