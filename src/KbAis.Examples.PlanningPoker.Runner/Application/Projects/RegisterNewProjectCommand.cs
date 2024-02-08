using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using MediatR;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Projects;

using Command = RegisterNewProjectCommand;

public record RegisterNewProjectCommand(Update Update) : ICommand;

internal sealed class RegisterNewProjectCommandHandler : IRequestHandler<Command> {

    public Task Handle(Command command, CancellationToken c) {
        throw new NotImplementedException();
    }
}
