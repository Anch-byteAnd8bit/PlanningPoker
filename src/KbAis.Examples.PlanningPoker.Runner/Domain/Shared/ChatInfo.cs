using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Shared;

public class ChatInfo : ValueObject {
    public TelegramId TelegramId { get; private init; }
    
    // TODO: Add chat kind

    public static Result<ChatInfo> Create(long telegramId) {
        var newChatInfo = new ChatInfo { TelegramId = new(telegramId) };

        return Result.Success(newChatInfo);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents() {
        yield return TelegramId;
    }
}
