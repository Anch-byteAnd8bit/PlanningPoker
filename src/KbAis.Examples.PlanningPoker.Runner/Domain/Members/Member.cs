using CSharpFunctionalExtensions;
using System.Net.Sockets;
using KbAis.Examples.PlanningPoker.Runner.Domain.Shared;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Members;

public class Member : Entity {
    public ChatInfo Chat { get; init; }
}
