using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public record DefaultCommand(Update Update) : ICommand;

internal sealed class DefaultCommandHandler(ILogger<DefaultCommandHandler> logger) : ICommandHandler<DefaultCommand> {
    public Task Handle(DefaultCommand command, Cancellation c) {
        logger.LogDebug("Received update has not been handled: {@Update}", command.Update);

        return Task.CompletedTask;
    }
}
