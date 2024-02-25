using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Members;

public class ChatInfo : ValueObject {

    protected override IEnumerable<IComparable> GetEqualityComponents() {
        throw new NotImplementedException();
    }
}
