using CSharpFunctionalExtensions;
using KbAis.Examples.PlanningPoker.Runner.Domain.Members;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Members.Services;

public interface IMembersGateway {
    Task<Result<Member>> FetchOrRegisterAsync(long memberId, Cancellation c = default);
}
