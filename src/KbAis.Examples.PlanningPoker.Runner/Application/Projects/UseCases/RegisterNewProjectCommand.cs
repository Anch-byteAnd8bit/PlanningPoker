using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Domain.Members;
using KbAis.Examples.PlanningPoker.Runner.Domain.Projects;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;
using Command = RegisterNewProjectCommand;

public record RegisterNewProjectCommand(Update Update) : ICommand {
    public Message Message => Update.Message!;
}

internal sealed class RegisterNewProjectCommandHandler(
    ITgBotClient client,
    PlanningPokerDataContext ctx
) : IRequestHandler<Command>
{

    public Task Handle(Command command, Cancellation c)
    {
        var projectChatId = command.Message.Chat.Id;
        var facilitatorId = command.Message.From!.Id;

        ProjectService projectService = new ProjectService(ctx);
        if (!projectService.IsTgUserFree(facilitatorId) && !projectService.IsTgChatFree(projectChatId))
        {
            ProjectChat projectChat = new ProjectChat(projectChatId);
            Member member = new Member
            {
                UserId = facilitatorId,
                Chat = new ChatInfo()
            };
            var newProject = Project.StartNew(projectChat, member);
            ProjectForDB project = new ProjectForDB
            {
                id = Random.Shared.NextInt64(),
                started_at = DateTime.Today,
                name = command.Message.Chat.Title,
                chatId = projectChatId
            };
            MemberForDB memberdb = new MemberForDB
            {
                user_id = facilitatorId,
                project_id = project.id,
            };

            var message = command.Message.From.Username + " - мастер";
            outbox_table newmsg = new outbox_table { id = Random.Shared.NextInt64(), textmessage = message };
            ctx.projects.Add(project);
            ctx.members.Add(memberdb);
            ctx.outbox_table.Add(newmsg);
            ctx.SaveChanges();

            var notifier = new TgNotificationService();
            notifier.SendTgNotification(client, ctx,  c, projectChatId, newmsg.id);
        }
        return client.SendTextMessageAsync(projectChatId, "пользователь или чат занят", cancellationToken: c);
    }
}

