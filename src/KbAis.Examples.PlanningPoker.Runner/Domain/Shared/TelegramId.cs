using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Shared;

public class TelegramId(long id) : SimpleValueObject<long>(id);
