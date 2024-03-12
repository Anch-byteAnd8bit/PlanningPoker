namespace KbAis.Examples.PlanningPoker.Runner;

public interface ITgProjectService
{
    public bool IsTgUserFree(long telegramUserId);
    public bool IsTgChatFree(long chatId);
}

