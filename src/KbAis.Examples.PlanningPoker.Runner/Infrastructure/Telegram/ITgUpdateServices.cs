using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public interface ITgUpdateServices {
    ILogger<IUpdateHandler> Logger { get; }

    Task Dispatch<T>(T cmd, Cancellation c) where T : ICommand;
}

internal sealed class TgUpdateServices(ILogger<IUpdateHandler> logger, ISender sender) : ITgUpdateServices {
    public ILogger<IUpdateHandler> Logger => logger;

    public Task Dispatch<T>(T cmd, Cancellation c) where T : ICommand =>
        sender.Send(cmd, c);
}
