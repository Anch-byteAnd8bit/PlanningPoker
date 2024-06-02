using System.Diagnostics;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

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