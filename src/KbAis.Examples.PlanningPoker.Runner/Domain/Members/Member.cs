using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Members;

public class Member : Entity {
    public long UserId { get; init; }
    public ChatInfo Chat { get; init; }
}