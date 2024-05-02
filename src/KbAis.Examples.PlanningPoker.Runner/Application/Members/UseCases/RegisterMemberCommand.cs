using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Members.Services;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Members.UseCases;

using Command = RegisterMemberCommand;

public record RegisterMemberCommand(long telegramId) : ICommand;

internal sealed class RegisterNewMemberCommandHandler(
    IMemberService memberService
) : ICommandHandler<Command> {
    public Task Handle(Command cmd, Cancellation c) {
        throw new NotImplementedException();
    }
}
