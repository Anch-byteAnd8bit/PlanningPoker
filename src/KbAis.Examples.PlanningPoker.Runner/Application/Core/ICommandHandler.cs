using CSharpFunctionalExtensions;
using MediatR;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;
