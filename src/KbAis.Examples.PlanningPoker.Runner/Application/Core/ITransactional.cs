using System.Data;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public interface ITransactional {
    IsolationLevel IsolationLevel => IsolationLevel.ReadUncommitted;

    bool AutoSaveChanges => true;
}
