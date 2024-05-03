using CSharpFunctionalExtensions;
using MediatR;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public interface ICommand : IRequest<Result>;
