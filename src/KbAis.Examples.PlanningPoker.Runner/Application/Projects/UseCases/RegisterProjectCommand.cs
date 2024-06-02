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

/// <summary>
/// Регистрация нового проекта проведения Planning Poker сессий.
/// </summary>
/// <param name="Message"></param>
public record RegisterProjectCommand(Message Message) : ICommand {
    public Maybe<User> FacilitatorUser => Message.From.AsMaybe();

    public Maybe<Chat> ProjectChat => Message.Chat.AsMaybe();
}

internal sealed class RegisterProjectCommandHandler : IRequestHandler<Command, Result> {

    public required ITgBotClient Client { private get; init; }

    public required IEntityFactoryService Efs { private get; init; }

    public async Task<Result> Handle(Command cmd, Cancellation c) {
        if (cmd.ProjectChat is not { HasValue: true, Value.Id: var projectChatIdentifier }) {
            return Result.Failure("Could not get Project's chat Identifier");
        }

        return await CommandContext.Begin(Efs.Get<Project, ProjectId>(), c)
            .Ensure(ctx => ctx.ProjectsRepository
                .NotRegisteredOrFailure(projectChatIdentifier, ctx.Cancellation))
            .BindZip(ctx => ChatInfo.Create(projectChatIdentifier))
            .Map((ctx, chat) => Project.StartNew(chat))
            .Tap((ctx, project) => ctx.ProjectsRepository
                .InsertAsync(project, c: ctx.Cancellation));
    }

    private class CommandContext {

        public IEntityRepository<Project, ProjectId> ProjectsRepository { get; }

        public Cancellation Cancellation { get; }

        private CommandContext(
            IEntityRepository<Project, ProjectId> projectsRepository,
            Cancellation                          cancellation
        ) {
            ProjectsRepository = projectsRepository;
            Cancellation = cancellation;
        }

        public static Result<CommandContext> Begin(
            IEntityRepository<Project, ProjectId> projectsRepository,
            Cancellation                          cancellation
        ) {
            return new CommandContext(projectsRepository, cancellation);
        }
    }
}

public static class ProjectEntityRepository {

    public static async Task<Result> NotRegisteredOrFailure(
        this IEntityRepository<Project, ProjectId> repository, long chatId, Cancellation c = default
    ) {
        var project = await repository.Query()
            .SingleOrDefaultAsync(x => x.Chat.TelegramId == chatId, c);

        return project is not null
            ? Result.Failure(Project.Errors.AlreadyRegisteredByChat)
            : Result.Success();
    }

}
