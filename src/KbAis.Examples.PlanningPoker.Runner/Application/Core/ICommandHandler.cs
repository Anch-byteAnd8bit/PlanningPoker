using MediatR;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand;
