﻿using System.Diagnostics;
using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TgUpdateReceiveService(ITgBotClient client, IUpdateHandler handler) {
    public Task ReceiveAsync(Cancellation c) {
        return client.ReceiveAsync(handler, cancellationToken: c);
    }
}

public class TgUpdateHandler(ITgUpdateServices services) : IUpdateHandler {
    public const string TgUpdateHandlerSourceName = "TgUpdatePollingService";

    private static readonly ActivitySource TgUpdateHandlerActivitySource = new(TgUpdateHandlerSourceName);

    public async Task HandleUpdateAsync(ITgBotClient client, Update upd, Cancellation c) {
        using var _0 = TgUpdateHandlerActivitySource.StartActivity("HandleUpdate");

        using var _1 = services.Logger.BeginScope("TelegramUpdate: {UpdateId}", upd.Id);

        services.Logger.LogDebug("Handling a new Telegram update");

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

public static class TgUpdateRouter {
    private delegate bool TgUpdatePredicate(TgUpdateContext tgUpdate);

    private delegate ICommand CommandFactory(Update update);

    private static readonly List<(TgUpdatePredicate Predicate, CommandFactory Command)> Predicates = [
        ( // Register a new project on bot adding to the project group
            ctx => ctx is { Message.Type: MessageType.GroupCreated } || BotHasBeenAdded(ctx),
            upd => new RegisterProjectCommand(upd.Message ?? throw new Exception("An invalid update"))
        ),
        // If all predicates return false, then route the update to the default handler to discard.
        (_ => true, upd => new DefaultCommand(upd))
    ];
    
    public static ICommand MapToCommand(this TgUpdateContext ctx) =>
        Predicates.First(p => p.Predicate(ctx)).Command(ctx.Update);

    private static bool BotHasBeenAdded(TgUpdateContext ctx) =>
        ctx.Message?.NewChatMembers?.Any(x => x.Id == ctx.BotId) ?? false;
}
