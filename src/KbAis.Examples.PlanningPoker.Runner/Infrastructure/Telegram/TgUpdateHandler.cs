using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TgUpdateReceiveService(ITgBotClient client, IUpdateHandler updHandler) {
    public Task ReceiveAsync(Cancellation c) {
        return client.ReceiveAsync(updHandler, cancellationToken: c);
    }
}

public class TgUpdateHandler(ITgRouterServiceProvider services) : IUpdateHandler {
    public async Task HandleUpdateAsync(ITgBotClient client, Update upd, Cancellation c) {
        using var _ = services.Logger.BeginScope("Handle Telegram Update: {UpdateId}", upd.Id);

        var updCtx = new TgUpdateContext { BotId = client.BotId!.Value, Update = upd };

        // NOTE: Have to await here to prevent logger scope from disposing while handling command.
        await services.Dispatch(updCtx.MapToCommand(), c);
    }

    public Task HandlePollingErrorAsync(ITgBotClient client, Exception exp, Cancellation c) {
        services.Logger.LogError(exp, "Caught unexpected exception on telegram update polling");

        return Task.CompletedTask;
    }
}

public class TgUpdateContext {
    public required long BotId { get; init; }

    public required Update Update { get; init; }

    public Message? Message => Update.Message;
}

public static class TgUpdateToCommandMapper {
    private delegate bool UpdatePredicate(TgUpdateContext tgUpdate);

    private delegate ICommand CommandFactory(Update update);

    private readonly static List<(UpdatePredicate Predicate, CommandFactory Command)> Predicates = [
        ( // Register a new project on bot adding
            ctx => ctx is { Message.Type: MessageType.GroupCreated } || BotHasBeenAdded(ctx),
            upd => new RegisterNewProjectCommand(upd)
        ),
        (_ => true, u => new DefaultCommand(u))
    ];
    
    public static ICommand MapToCommand(this TgUpdateContext ctx) =>
        Predicates.First(p => p.Predicate(ctx)).Command(ctx.Update);

    private static bool BotHasBeenAdded(TgUpdateContext ctx) =>
        ctx.Message?.NewChatMembers?.Any(x => x.Id == ctx.BotId) ?? false;
}