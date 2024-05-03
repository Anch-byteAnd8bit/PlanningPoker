using CSharpFunctionalExtensions;
using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Core.Extensions;
using KbAis.Examples.PlanningPoker.Runner.Domain.Projects;
using KbAis.Examples.PlanningPoker.Runner.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;

using Command = RegisterProjectCommand;

public record RegisterProjectCommand(Message Message) : ICommand {
    public Maybe<User> FaciliatorUser => Message.From.AsMaybe();

    public Maybe<Chat> ProjectChat => Message.Chat.AsMaybe();
}

internal sealed class RegisterProjectCommandHandler : IRequestHandler<Command, Result> {

    public required ITgBotClient Client { private get; init; }

    public required IEntityFactoryService Efs { private get; init; }

    public async Task<Result> Handle(Command cmd, Cancellation c) {
        if (cmd.ProjectChat is not { HasValue: true, Value.Id: var projectChatIdentifier }) {
            return Result.Failure("Could not get Project's chat Identifier");
        }

        var pipelineContext = new {
            ProjectsRepository = Efs.Get<Project, ProjectId>(),
            C = c
        };

        await Result.Success(pipelineContext)
            .Ensure (ctx            => ctx.ProjectsRepository
                .NotRegisteredOrFailure(projectChatIdentifier, ctx.C))
            .BindZip(ctx            => ChatInfo.Create(projectChatIdentifier))
            .Map    ((ctx, chat)    => Project.StartNew(chat))
            .Tap    ((ctx, project) => ctx.ProjectsRepository.InsertAsync(project, c: ctx.C));

        throw new NotImplementedException();
    }

}

public static class ProjectEntityRepository {

    public static async Task<Result> NotRegisteredOrFailure(
        this IEntityRepository<Project, ProjectId> repository, long projectChatIdentifier, Cancellation c = default
    ) {
        var project = await repository.Query()
            .SingleOrDefaultAsync(x => x.Chat.Identifier == projectChatIdentifier, c);

        return project is not null ? Result.Failure(Project.Errors.AlreadyRregisteredByChat) : Result.Success();
    }

}
