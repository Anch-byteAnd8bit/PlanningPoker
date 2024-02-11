using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;

using Command = RegisterNewProjectCommand;

public record RegisterNewProjectCommand(Update Update) : ICommand {
    public Message Message => Update.Message!;
}

internal sealed class RegisterNewProjectCommandHandler(
    ITgBotClient client
) : IRequestHandler<Command> {

    public Task Handle(Command command, Cancellation c) {
        var projectChatId = command.Message.Chat.Id;

        var facilitatorId = command.Message.From!.Id;

        throw new NotImplementedException();
    }
}
