using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Members.Services;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Members.UseCases;

using Command = RegisterNewMemberCommand;

public record RegisterNewMemberCommand : ICommand;

internal sealed class RegisterNewMemberCommandHandler(
    IMemberService memberService
) : ICommandHandler<Command> {
    public Task Handle(Command command, Cancellation c) {
        throw new NotImplementedException();
    }
}
