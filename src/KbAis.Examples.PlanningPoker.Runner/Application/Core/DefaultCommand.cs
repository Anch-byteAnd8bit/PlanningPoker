using CSharpFunctionalExtensions;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public record DefaultCommand(Update Update) : ICommand;

public class SampleResponse {
    public int Value { get; init; }
}

internal sealed class DefaultCommandHandler(
    ILogger<DefaultCommandHandler> logger, PlanningPokerDataContext dbContext
) : ICommandHandler<DefaultCommand> {
    public async Task<Result> Handle(DefaultCommand command, Cancellation c) {
        await dbContext.Set<ProjectSample>()
            .AddAsync(new ProjectSample {
                Name = "Sample"
            }, c);

        await dbContext.SaveChangesAsync(c);

        logger.LogDebug("Received update has not been handled: {@Update}", command.Update);

        await dbContext.Set<ProjectSample>().ToListAsync(cancellationToken: c);

        return Result.Success();
    }
}
