using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Shared;

public class ChatInfo : ValueObject {
    public long Identifier { get; private init; }

    public static Result<ChatInfo> Create(long identifier) {
        var newChatInfo = new ChatInfo { Identifier = identifier };

        return Result.Success(newChatInfo);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents() {
        yield return Identifier;
    }
}
