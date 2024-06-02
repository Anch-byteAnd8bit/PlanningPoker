using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Pipeline;

internal class UnitOfWorkBehaviour<T, K>(
    PlanningPokerDataContext dataContext,
    ILogger<UnitOfWorkBehaviour<T, K>> logger
) : IPipelineBehavior<T, K> where T : ITransactional {

    public async Task<K> Handle(T req, RequestHandlerDelegate<K> next, Cancellation c) {
        await using var transactionScope = await dataContext.Database
            .BeginTransactionAsync(req.IsolationLevel, c);

        try {
            var response = await next();

            if (req.AutoSaveChanges) {
                logger.LogDebug("Performing auto save of DbContext");

                var entriesWritten = await dataContext.SaveChangesAsync(c);

                logger.LogDebug("Number of {Entries} entries has been written", entriesWritten);
            }

            await transactionScope.CommitAsync(c);

            return response;
        } catch {
            await transactionScope.RollbackAsync(c);

            throw;
        }
    }

}
