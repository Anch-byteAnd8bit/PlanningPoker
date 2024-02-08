using KbAis.Examples.PlanningPoker.Runner.Application;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public interface ICommand : IRequest;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand;

public interface ITelegramRouterContext {
    ILogger<IUpdateHandler> Logger { get; }

    Task Dispatch<T>(T command, CancellationToken c) where T : ICommand;
}

public class UpdateContext {
    public required long BotId { get; init; }

    public required Update Update { get; init; }

    public Message? Message => Update.Message;
}

public static class TelegramUpdateToCommandMapper {
    private delegate bool UpdatePredicate(UpdateContext update);

    private delegate ICommand CommandFactory(Update update);

    private readonly static List<(UpdatePredicate Predicate, CommandFactory Command)> Predicates = [
        ( // Register a new project on bot adding
            ctx => ctx is { Message.Type: MessageType.GroupCreated } || BotHasBeenAdded(ctx),
            upd => new RegisterNewProjectCommand(upd)
        ),
        (_ => true, u => new DefaultCommand(u))
    ];

    private static bool BotHasBeenAdded(UpdateContext ctx) {
        return ctx.Message?.NewChatMembers?.Any(x => x.Id == ctx.BotId) ?? false;
    }

    public static ICommand MapToCommand(this UpdateContext ctx) =>
        Predicates.First(p => p.Predicate(ctx)).Command(ctx.Update);
}

public class TelegramUpdateHandler(ITelegramRouterContext ctx) : IUpdateHandler {
    public async Task HandleUpdateAsync(ITelegramBotClient client, Update upd, CancellationToken c) {
        using var _ = ctx.Logger.BeginScope("Handle Telegram Update: {UpdateId}", upd.Id);

        var updCtx = new UpdateContext { BotId = client.BotId!.Value, Update = upd };

        // NOTE: Have to await here to prevent logger scope from disposing while handling command.
        await ctx.Dispatch(updCtx.MapToCommand(), c);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exp, CancellationToken c) {
        ctx.Logger.LogError(exp, "Caught unexpected exception on telegram update polling");

        return Task.CompletedTask;
    }
}
