using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public interface ITgRouterServiceProvider {
    ILogger<IUpdateHandler> Logger { get; }

    Task Dispatch<T>(T command, Cancellation c) where T : ICommand;
}
