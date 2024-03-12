using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

namespace KbAis.Examples.PlanningPoker.Runner;
public class ProjectService(PlanningPokerDataContext ctx) : ITgProjectService
{
    public bool IsTgUserFree(long telegramUserId)
    {
        var members = ctx.members.ToList();
        return members.Any(x => x.user_id == telegramUserId);
    }

    public bool IsTgChatFree(long chatId)
    {
        var projects = ctx.projects.ToList();
        return projects.Any(x => x.chatId == chatId);
    }
}
