using CSharpFunctionalExtensions;
using KbAis.Examples.PlanningPoker.Runner.Domain.Members;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Shared;

public class ChatInfo : ValueObject {
    public TelegramId Telegram { get; init; }

    protected override IEnumerable<IComparable> GetEqualityComponents() {
        throw new NotImplementedException();
    }
}
