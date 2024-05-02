using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Domain.Members;
using KbAis.Examples.PlanningPoker.Runner.Domain.Projects;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;

using Command = RegisterProjectCommand;

public record RegisterProjectCommand(Update Update) : ICommand {
    public Message Message => Update.Message!;
}

internal sealed class RegisterProjectCommandHandler(ITgBotClient client) : IRequestHandler<Command> {

    public Task Handle(Command command, Cancellation c) {
        // TODO: Register Initiator by Telegram ID if not already

        // TODO: Register Project by TG Group (check if already exist)

        // TODO: Set Group Facilitator
        var tgUserFacilitator = command.Message.From;

        var tgChatPproject = command.Message.Chat;

        return Task.CompletedTask;
    }
}

